using System.Collections.Generic;
using System.Linq;
using ApiServices;
using ApiServices.Models.RankedMatch;
using Characters;
using Global;
using Photon.Pun;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Photon.PlayerPropertyKeys;

namespace Matchmaking
{
    public class MatchmakingManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TMP_Text roomName;
        [SerializeField] private TMP_Text waitingText;
        [SerializeField] private CharacterImages characterImages;
        [SerializeField] private GameObject playersList;
        [SerializeField] private RoomPlayer roomPlayerPrefab;
        [SerializeField] private Button backButton;
    
        private readonly List<RoomPlayer> allPlayers = new();

        private void Start()
        {
            var numTeams = (int)PhotonNetwork.CurrentRoom.CustomProperties[Room.NumTeamsPropKey];
            roomName.text = RoomTitle(numTeams, PhotonNetwork.CurrentRoom.MaxPlayers / numTeams);
            ListAllPlayers();
            backButton.onClick.AddListener(LeaveRoom);
        }

        
        private static string RoomTitle(int numTeams, int numPlayersPerTeam)
        {
            var roomTitle = "";
            for(var i = 0; i < numTeams; i++)
            {
                roomTitle += $"{numPlayersPerTeam}";
                if(i < numTeams - 1)
                {
                    roomTitle += "v";
                }
            }
            return roomTitle;
        }

        private void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        private void ListAllPlayers()
        {
            foreach (var roomPlayer in allPlayers)
            {
                Destroy(roomPlayer.gameObject);
            }
            allPlayers.Clear();
        
            foreach (var player in PhotonNetwork.PlayerList)
            {
                OnPlayerEnteredRoom(player);
            }
            
            SetWaitingText();
        }

        public override void OnPlayerEnteredRoom(Photon.Realtime.Player player)
        {
            var roomPlayer = Instantiate(roomPlayerPrefab, playersList.transform);
            var playerCharacter = (CharactersEnum)player.CustomProperties[CharacterKey];
            roomPlayer.SetPlayerInfo(player.NickName, characterImages.GetCharacterSprite((int)playerCharacter));
            allPlayers.Add(roomPlayer);
            if (CheckRoomFull())
            {
                StartGame();
            }
            SetWaitingText();
        }

        private static bool CheckRoomFull()
        {
            return PhotonNetwork.IsMasterClient && PhotonNetwork.PlayerList.Length == PhotonNetwork.CurrentRoom.MaxPlayers;
        }

        private void StartGame()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            PhotonNetwork.CurrentRoom.IsOpen = false;
            AssignTeams();
            switch ((GameModes)PhotonNetwork.CurrentRoom.CustomProperties[Room.ModePropKey])
            {
                case GameModes.Casual:
                    CreateCasualMatch();
                    break;
                case GameModes.Ranked:
                    CreateRankedMatch();
                    break;
                case GameModes.Training:
                default:
                    LoadGame();
                    break;
            }
        }

        private static void AssignTeams()
        {
            for (var i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                var playerProperties = PhotonNetwork.PlayerList[i].CustomProperties;
                var team = i % (int)PhotonNetwork.CurrentRoom.CustomProperties[Room.NumTeamsPropKey];
                if (!playerProperties.TryAdd(TeamKey, team)) playerProperties[TeamKey] = team;
                PhotonNetwork.PlayerList[i].SetCustomProperties(playerProperties);
            }
        }

        private void CreateCasualMatch()
        {
            List<List<string>> teams = new();
            foreach (var player in PhotonNetwork.PlayerList)
            {
                var team = player.CustomProperties[TeamKey];
                if (teams.Count <= (int)team) teams.Add(new List<string>());
                teams[(int)team].Add(player.CustomProperties[PlayerIdKey].ToString());
            }
            StartCoroutine(CasualMatchServices.CreateMatch(teams, OnCasualMatchCreated));
        }

        private void OnCasualMatchCreated(bool success, string message)
        {
            if (!success)
            {
                OnMatchCreateError();
                return;
            }
            var roomProperties = PhotonNetwork.CurrentRoom.CustomProperties;
            roomProperties[Room.MatchIdPropKey] = message;
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);
            LoadGame();
        }
        
        private void CreateRankedMatch()
        {
            List<List<string>> teams = new();
            foreach (var player in PhotonNetwork.PlayerList)
            {
                var team = player.CustomProperties[TeamKey];
                if (teams.Count <= (int)team) teams.Add(new List<string>());
                teams[(int)team].Add(player.CustomProperties[AccountAddressKey].ToString()[2..]);
            }
            
            StartCoroutine(RankedMatchServices.CreateMatch(
                teams.Select(team => new CreateRankedMatchTeam(team)).ToList(), OnRankedMatchCreated));
        }

        private void OnRankedMatchCreated(bool success, string message)
        {
            if (!success)
            {
                OnMatchCreateError();
                return;
            }
            var roomProperties = PhotonNetwork.CurrentRoom.CustomProperties;
            roomProperties[Room.MatchAddressPropKey] = message;
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);
            LoadGame();
        }

        private static void LoadGame()
        {
            PhotonNetwork.LoadLevel("GameplayScene");
        }
    
        public override void OnPlayerLeftRoom(Photon.Realtime.Player player)
        {
            ListAllPlayers();
        }
    
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene("ModeSelectScene");
        }

        private void OnMatchCreateError()
        {
            waitingText.text = "Error creating match";
        }

        private void SetWaitingText()
        {
            var remainingPlayers = PhotonNetwork.CurrentRoom.MaxPlayers - PhotonNetwork.CurrentRoom.PlayerCount;
            if (remainingPlayers == 0)
            {
                waitingText.text = "Loading the game...";
                backButton.interactable = false;
            }
            else
            {
                var suffix = remainingPlayers == 1 ? "" : "s";
                waitingText.text =
                    $"Waiting for {remainingPlayers} player{suffix}...";
            }
        }
    }
}
