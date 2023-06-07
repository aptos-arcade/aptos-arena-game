using Characters;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Photon.PlayerPropertyKeys;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace MainMenu
{
    public class TrainingMenuManager : MonoBehaviourPunCallbacks
    {

        private const string ModePropKey = "m";
    
        private void Start()
        {
            CharacterCard.OnSelect += JoinTrainingRoom;
        }

        private static void JoinTrainingRoom()
        {
            var playerProperties = new Hashtable()
            {
                { SwordKey, Random.Range(0, 5)},
                { GunKey, Random.Range(0, 5)}
            };
            PhotonNetwork.SetPlayerCustomProperties(playerProperties);
            var roomOptions = new RoomOptions
            {
                MaxPlayers = 1,
                CustomRoomPropertiesForLobby = new []{ ModePropKey },
                CustomRoomProperties = new Hashtable() { { ModePropKey, Global.GameModes.Training } }
            };
            var expectedCustomRoomProperties = new Hashtable() { { ModePropKey, Global.GameModes.Training } };
            PhotonNetwork.JoinRandomOrCreateRoom(
                roomOptions: roomOptions,
                expectedMaxPlayers: 1,
                expectedCustomRoomProperties: expectedCustomRoomProperties
            );
        }
        
        public override void OnJoinedRoom()
        {
            SceneManager.LoadScene("MatchmakingScene");
        }

        private void OnDestroy()
        {
            CharacterCard.OnSelect -= JoinTrainingRoom;
        }
    }
}
