using System.Collections.Generic;
using Characters;
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

        private static readonly Dictionary<int, string> GameModes = new()
        {
            {1, "Training"},
            {2, "Duel (1v1)"},
            {4, "Duos (2v2)"},
            {8, "Squads (4v4)"}
        };

        private void Start()
        {
            roomName.text = GameModes[PhotonNetwork.CurrentRoom.MaxPlayers];
            ListAllPlayers();
            backButton.onClick.AddListener(LeaveRoom);
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

        private static void StartGame()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            PhotonNetwork.CurrentRoom.IsOpen = false;
            for (var i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                var playerProperties = PhotonNetwork.PlayerList[i].CustomProperties;
                var team = i % 2;
                if(!playerProperties.TryAdd(TeamKey, team)) playerProperties[TeamKey] = team;
                PhotonNetwork.PlayerList[i].SetCustomProperties(playerProperties);
            }
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

        private void SetWaitingText()
        {
            var remainingPlayers = PhotonNetwork.CurrentRoom.MaxPlayers - PhotonNetwork.CurrentRoom.PlayerCount;
            var suffix = remainingPlayers == 1 ? "" : "s";
            waitingText.text =
                $"Waiting for {remainingPlayers} player{suffix}...";
        }
    }
}
