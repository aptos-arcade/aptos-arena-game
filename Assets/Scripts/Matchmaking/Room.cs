using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace Matchmaking
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private byte numberOfPlayers;
        [SerializeField] private Global.GameModes gameMode;
        [SerializeField] private int numTeams;

        private Button joinButton;

        public const string ModePropKey = "m";
        public const string NumTeamsPropKey = "nt";
        public const string MatchAddressPropKey = "ma";
    
        private void Start()
        {
            joinButton = GetComponent<Button>();
            joinButton.onClick.AddListener(JoinRoom);
        }

        private void JoinRoom()
        {
            var roomProperties = new Hashtable() { { ModePropKey, gameMode }, { NumTeamsPropKey, numTeams } };
            var roomOptions = new RoomOptions
            {
                MaxPlayers = numberOfPlayers,
                CustomRoomPropertiesForLobby = new []{ ModePropKey, NumTeamsPropKey },
                CustomRoomProperties = roomProperties
            };
            PhotonNetwork.JoinRandomOrCreateRoom(
                roomOptions: roomOptions, 
                expectedMaxPlayers: numberOfPlayers,
                expectedCustomRoomProperties: roomProperties
            );
        }
    }
}
