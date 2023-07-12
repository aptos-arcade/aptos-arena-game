using Characters;
using Photon.Pun;
using Photon.Realtime;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Photon.PlayerPropertyKeys;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Room = Matchmaking.Room;

namespace MainMenu
{
    public class TrainingMenuManager : MonoBehaviourPunCallbacks
    {
        
        [SerializeField] private CharacterDisplay characterDisplay;
        
        [SerializeField] private Button continueButton;

        private void Start()
        {
            var playerProperties = new Hashtable()
            {
                { CharacterKey, Characters.Characters.GetRandomCharacter() },
                { SwordKey, Random.Range(0, 5)},
                { GunKey, Random.Range(0, 5)}
            };
            PhotonNetwork.SetPlayerCustomProperties(playerProperties);
            PhotonNetwork.NickName = AuthenticationService.Instance.PlayerName;
            characterDisplay.UpdateCharacter();
            CharacterCard.OnSelect += OnCharacterSelect;
            continueButton.onClick.AddListener(JoinTrainingRoom);
        }
        
        private void OnCharacterSelect()
        {
            characterDisplay.UpdateCharacter();
        }

        private static void JoinTrainingRoom()
        {
            var roomProperties = new Hashtable()
            {
                { Room.ModePropKey, Global.GameModes.Training }, 
                { Room.NumTeamsPropKey, 1 }
            };
            var roomOptions = new RoomOptions
            {
                MaxPlayers = 1,
                CustomRoomPropertiesForLobby = new []{ Room.ModePropKey, Room.NumTeamsPropKey },
                CustomRoomProperties = roomProperties
            };
            PhotonNetwork.JoinRandomOrCreateRoom(
                roomOptions: roomOptions,
                expectedMaxPlayers: 1,
                expectedCustomRoomProperties: roomProperties
            );
        }
        
        public override void OnJoinedRoom()
        {
            SceneManager.LoadScene("MatchmakingScene");
        }

        private void OnDestroy()
        {
            CharacterCard.OnSelect -= OnCharacterSelect;
        }
    }
}
