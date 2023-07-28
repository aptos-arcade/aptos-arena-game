using System;
using System.Collections.Generic;
using System.Linq;
using ApiServices;
using AptosIntegration;
using Characters;
using ExitGames.Client.Photon;
using Global;
using Photon.Pun;
using Photon.Realtime;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Photon.PlayerPropertyKeys;
using Room = Matchmaking.Room;

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

        private enum GameState
        {
            Waiting,
            Playing,
            MatchOver
        }
        
        public PlayerScript Player { get; set; }
        
        [Header("Game Objects")]
        [SerializeField] private Camera sceneCamera;
        
        [Header("Managers")]
        [SerializeField] private RespawnManager respawnManager;
        [SerializeField] private OutOfLivesManager outOfLivesManager;
        [SerializeField] private FeedManager feedManager;
        [SerializeField] private KillTextManager killTextManager;
        [SerializeField] private ConnectedPlayersManager connectedPlayersManager;
        
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
        
        public readonly List<PlayerInfo> PlayerInfos = new();
        
        private EnergyUIController energyUIController;

        private void Start()
        {
            energyUIController = energyUI.GetComponent<EnergyUIController>();
            SpawnPosition = spawnPositions[(int)PhotonNetwork.LocalPlayer.CustomProperties[TeamKey]].position;
            var characterPrefabName = Characters.Characters.AvailableCharacters[(CharactersEnum)PhotonNetwork.LocalPlayer
                .CustomProperties[CharacterKey]].PrefabName;
            PhotonNetwork.Instantiate(characterPrefabName, SpawnPosition, Quaternion.identity);
            gameState = GameState.Playing;
            NewPlayerSend();
            respawnManager.StartRespawn();
        }
        
        public void SetPlayerCameraActive(bool active)
        {
            Player.PlayerComponents.PlayerCamera.gameObject.SetActive(active);
            sceneCamera.gameObject.SetActive(!active);
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
                PhotonNetwork.LocalPlayer.CustomProperties[CharacterKey],
                0
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
            var kills = (int)data[5];

            var playerInfo = new PlayerInfo(username, actorNumber, lives, team, character, kills);
            PlayerInfos.Add(playerInfo);
            ListPlayersSend();
        }
        
        private void ListPlayersSend()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            var package = new object[PlayerInfos.Count + 2];
            package[0] = gameState;
            package[1] = winningTeam;
            for (var i = 0; i < PlayerInfos.Count; i++)
            {
                package[i + 2] = new object[]
                {
                    PlayerInfos[i].Name,
                    PlayerInfos[i].ActorNumber,
                    PlayerInfos[i].Lives,
                    PlayerInfos[i].Team,
                    PlayerInfos[i].Character,
                    PlayerInfos[i].Kills
                };
            }

            PhotonNetwork.RaiseEvent((byte)EventCodes.ListPlayers, package,
                new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
        }

        private void ListPlayersReceive(IReadOnlyList<object> data)
        {
            PlayerInfos.Clear();
            gameState = (GameState)data[0];
            winningTeam = (int)data[1];
            foreach (object[] playerInfoData in data.Skip(2))
            {
                var playerInfo = new PlayerInfo(
                    (string)playerInfoData[0],
                    (int)playerInfoData[1],
                    (int)playerInfoData[2],
                    (int)playerInfoData[3],
                    (CharactersEnum)playerInfoData[4],
                    (int)playerInfoData[5]
                );
                PlayerInfos.Add(playerInfo);
            }
            PlayerInfos.Sort((a, b) =>
            {
                var killsComparison = b.Kills.CompareTo(a.Kills);
                return killsComparison != 0 ? killsComparison : b.Lives.CompareTo(a.Lives);
            });
            connectedPlayersManager.ListAllPlayers();
            StateCheck();
        }

        public static void PlayerDeathSend(int actorDeath, int actorKill)
        {
            object[] data = {actorDeath, actorKill};
            PhotonNetwork.RaiseEvent((byte)EventCodes.PlayerDeath, data,
                new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
        }

        private void PlayerDeathReceive(IReadOnlyList<object> data)
        {
            var actorDeath = (int)data[0];
            var actorKill = (int)data[1];
            
            DeathFeedMessage(actorDeath, actorKill);
            
            var killPlayerIndex = PlayerInfos.FindIndex(x => x.ActorNumber == actorKill);
            if (killPlayerIndex >= 0 && killPlayerIndex < PlayerInfos.Count) PlayerInfos[killPlayerIndex].Kills++;
            if (actorKill == PhotonNetwork.LocalPlayer.ActorNumber) killTextManager.OnKill();

            var deathPlayerIndex = PlayerInfos.FindIndex(x => x.ActorNumber == actorDeath);
            if (deathPlayerIndex >= 0 && deathPlayerIndex < PlayerInfos.Count) PlayerInfos[deathPlayerIndex].Lives--;
            if (actorDeath == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                if(PlayerInfos[deathPlayerIndex].Lives > 0)
                    OnPlayerDeath();
                else
                    OnPlayerOutOfLives();
            }
            ScoreCheck();
            ListPlayersSend();
        }

        private void DeathFeedMessage(int actorDeath, int actorKill)
        {
            var deathPlayerName = PhotonNetwork.CurrentRoom.GetPlayer(actorDeath).NickName;
            if (actorKill == 0)
            {
                feedManager.WriteMessage(deathPlayerName + " died!", 5f);
            }
            else
            {
                var killPlayerName = PhotonNetwork.CurrentRoom.GetPlayer(actorKill).NickName;
                feedManager.WriteMessage(killPlayerName + " killed " + deathPlayerName + "!", 5f);
            }
        }

        private void OnPlayerDeath()
        {
            respawnManager.StartRespawn();
        }

        private void OnPlayerOutOfLives()
        {
            if(gameState == GameState.MatchOver) return;
            outOfLivesUI.SetActive(true);
        }
        
        public void SetEnergyUIActive(bool active)
        {
            energyUI.SetActive(active);
        }

        public void NoEnergy(EnergyUIController.EnergyType energyType)
        {
            energyUIController.NoEnergy(energyType);
        }
        
        private void ScoreCheck()
        {
            if (PhotonNetwork.CurrentRoom.MaxPlayers == 1 || !PhotonNetwork.IsMasterClient ||
                gameState == GameState.MatchOver) return;
            // check if there is only one team left that has players with more than one life
            var winnerFound = false;
            var curWinningTeam = -1;
            foreach (var playerInfo in PlayerInfos.Where(playerInfo => playerInfo.Lives > 0))
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
            OnWinnerFound(curWinningTeam);
        }
        
        private void OnWinnerFound(int winnerIndex)
        {
            gameState = GameState.MatchOver;
            winningTeam = winnerIndex;
            switch ((GameModes)PhotonNetwork.CurrentRoom.CustomProperties[Room.ModePropKey])
            {
                case GameModes.Casual:
                    var matchId = (string)PhotonNetwork.CurrentRoom.CustomProperties[Room.MatchIdPropKey];
                    StartCoroutine(CasualMatchServices.SetMatchResult(matchId, winnerIndex, OnCasualMatchResultReported));
                    break;
                case GameModes.Ranked:
                    var matchAddress = (string)PhotonNetwork.CurrentRoom.CustomProperties[Room.MatchAddressPropKey];
                    StartCoroutine(RankedMatchServices.SetMatchResult(matchAddress, winnerIndex, OnRankedMatchResultReported));
                    break;
                case GameModes.Training:
                default:
                    break;
            }
        }
        
        private static void OnCasualMatchResultReported(bool success, string message)
        {
            Debug.Log(message);
        }

        private static void OnRankedMatchResultReported(bool success, string message)
        {
            Debug.Log(message);
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
            PlayerInfos.RemoveAt(PlayerInfos.FindIndex(x => x.ActorNumber == otherPlayer.ActorNumber));
            ScoreCheck();
            ListPlayersSend();
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
    public int Kills { get; set; }
    
    public PlayerInfo(string name, int actorNumber, int lives, int team, CharactersEnum character, int kills)
    {
        Name = name;
        ActorNumber = actorNumber;
        Lives = lives;
        Team = team;
        Character = character;
        Kills = kills;
    }
}
