using System;
using System.Collections.Generic;
using System.Linq;
using Characters;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Photon.PlayerPropertyKeys;

namespace Gameplay
{
    public class MatchManager : MonoBehaviourPunCallbacks, IOnEventCallback
    {
        // singleton pattern
        public static MatchManager Instance;
        
        private void Awake()
        {
            Instance = this;
        }
        
        private enum EventCodes : byte
        {
            NewPlayer,
            ListPlayers,
            PlayerDeath
        }

        public enum GameState
        {
            Waiting,
            Playing,
            MatchOver
        }
        
        public PlayerScript Player { get; set; }
        
        [Header("Managers")]
        [SerializeField] private SpawnManager spawnManager;
        [SerializeField] private RespawnManager respawnManager;
        [SerializeField] private OutOfLivesManager outOfLivesManager;
        
        [Header("UI")]
        [SerializeField] private GameObject energyUI;
        [SerializeField] private GameObject outOfLivesUI;
        [SerializeField] private GameObject winUI;
        [SerializeField] private GameObject loseUI;
        
        [Header("Spawn Positions")]
        [SerializeField] private Transform[] spawnPositions;
        
        public Vector3 SpawnPosition { get; private set; }
        
        // game state
        private GameState gameState = GameState.Waiting;
        private int winningTeam;
        
        private readonly List<PlayerInfo> playerInfos = new();
        private int localPlayerIndex;
        
        private EnergyUIController energyUIController;

        private void Start()
        {
            energyUIController = energyUI.GetComponent<EnergyUIController>();
            SpawnPosition = spawnPositions[(int)PhotonNetwork.LocalPlayer.CustomProperties[TeamKey]].position;
            var playerLayer = LayerMask.NameToLayer("Player");
            Physics2D.IgnoreLayerCollision(playerLayer, playerLayer);
            spawnManager.ShowSpawnPanel();
            gameState = GameState.Playing;
            NewPlayerSend();
        }
        
        public void OnEvent(EventData photonEvent)
        {
            if (photonEvent.Code >= 200) return;
            var eventCode = (EventCodes)photonEvent.Code;
            var data = (object[])photonEvent.CustomData;

            Debug.Log("Received event: " + eventCode);

            switch (eventCode)
            {
                case EventCodes.PlayerDeath:
                    PlayerDeathReceive(data);
                    break;
                case EventCodes.NewPlayer:
                    NewPlayerReceive(data);
                    break;
                case EventCodes.ListPlayers:
                    ListPlayersReceive(data);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public override void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        public override void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        private static void NewPlayerSend()
        {
            object[] package =
            {
                PhotonNetwork.NickName, 
                PhotonNetwork.LocalPlayer.ActorNumber, 
                3,
                PhotonNetwork.LocalPlayer.CustomProperties[TeamKey],
                PhotonNetwork.LocalPlayer.CustomProperties[CharacterKey]
            };
            PhotonNetwork.RaiseEvent((byte)EventCodes.NewPlayer, package,
                new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient }, SendOptions.SendReliable);
        }
        
        private void NewPlayerReceive(IReadOnlyList<object> data)
        {
            var username = (string)data[0];
            var actorNumber = (int)data[1];
            var lives = (int)data[2];
            var team = (int)data[3];
            var character = (CharactersEnum)data[4];
            
            
            var playerInfo = new PlayerInfo(username, actorNumber, lives, team, character);
            playerInfos.Add(playerInfo);
            ListPlayersSend();
        }
        
        private void ListPlayersSend()
        {
            var package = new object[playerInfos.Count + 2];
            package[0] = gameState;
            package[1] = winningTeam;
            for (var i = 0; i < playerInfos.Count; i++)
            {
                package[i + 2] = new object[]
                {
                    playerInfos[i].Name,
                    playerInfos[i].ActorNumber,
                    playerInfos[i].Lives,
                    playerInfos[i].Team,
                    playerInfos[i].Character
                };
            }

            PhotonNetwork.RaiseEvent((byte)EventCodes.ListPlayers, package,
                new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
        }

        private void ListPlayersReceive(IReadOnlyList<object> data)
        {
            playerInfos.Clear();
            gameState = (GameState)data[0];
            winningTeam = (int)data[1];
            foreach (object[] playerInfoData in data.Skip(2))
            {
                var playerInfo = new PlayerInfo(
                    (string)playerInfoData[0],
                    (int)playerInfoData[1],
                    (int)playerInfoData[2],
                    (int)playerInfoData[3],
                    (CharactersEnum)playerInfoData[4]
                );
                playerInfos.Add(playerInfo);
                if (playerInfo.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
                    localPlayerIndex = playerInfos.Count - 2;
            }
            StateCheck();
        }

        public static void PlayerDeathSend(int actorDeath)
        {
            object[] data = {actorDeath};
            PhotonNetwork.RaiseEvent((byte)EventCodes.PlayerDeath, data,
                new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
        }

        private void PlayerDeathReceive(IReadOnlyList<object> data)
        {
            var actorDeath = (int)data[0];
            var deathPlayerIndex = playerInfos.FindIndex(x => x.ActorNumber == actorDeath);
            playerInfos[deathPlayerIndex].Lives--;
            if (actorDeath == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                if(playerInfos[deathPlayerIndex].Lives > 0)
                    OnPlayerDeath();
                else
                    OnPlayerOutOfLives();
            }
            ScoreCheck();
        }

        private void OnPlayerDeath()
        {
            respawnManager.StartRespawn();
        }

        private void OnPlayerOutOfLives()
        {
            outOfLivesUI.SetActive(true);
        }
        
        public void SetEnergyUIActive(bool active)
        {
            energyUI.SetActive(active);
        }

        public void NoEnergy(Global.Weapons weapon)
        {
            energyUIController.NoEnergy(weapon);
        }
        
        private void ScoreCheck()
        {
            if (PhotonNetwork.CurrentRoom.MaxPlayers == 1 || !PhotonNetwork.IsMasterClient ||
                gameState == GameState.MatchOver) return;
            // check if there is only one team left that has players with more than one life
            var winnerFound = false;
            var curWinningTeam = -1;
            foreach (var playerInfo in playerInfos.Where(playerInfo => playerInfo.Lives > 0))
            {
                if (curWinningTeam == -1 || curWinningTeam == playerInfo.Team)
                {
                    curWinningTeam = playerInfo.Team;
                    winnerFound = true;
                    continue;
                }
                winnerFound = false;
                break;
            }
            if (!winnerFound) return;
            gameState = GameState.MatchOver;
            winningTeam = curWinningTeam;
            ListPlayersSend();
        }
        
        private void StateCheck()
        {
            if(gameState == GameState.MatchOver) EndGame();
        }
        
        private void EndGame()
        {
            gameState = GameState.MatchOver;
            if(winningTeam == (int)PhotonNetwork.LocalPlayer.CustomProperties[TeamKey])
                OnWin();
            else
                OnLose();
        }
        
        private void OnWin()
        {
            winUI.SetActive(true);
            outOfLivesManager.SetOutOfLivesUI(false);
        }
        
        private void OnLose()
        {
            loseUI.SetActive(true);
            outOfLivesManager.SetOutOfLivesUI(false);
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene("ModeSelectScene");
        }

        public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
        {
            if (!PhotonNetwork.IsMasterClient) return;
            playerInfos.RemoveAt(playerInfos.FindIndex(x => x.ActorNumber == otherPlayer.ActorNumber));
            ScoreCheck();
        }
    }
}

[Serializable]
public class PlayerInfo
{
    public string Name { get; set; }
    public int ActorNumber { get; set; }
    public int Team { get; set; }
    public CharactersEnum Character { get; set; }
    public int Lives { get; set; }
    
    public PlayerInfo(string name, int actorNumber, int lives, int team, CharactersEnum character)
    {
        Name = name;
        ActorNumber = actorNumber;
        Lives = lives;
        Team = team;
        Character = character;
    }
}
