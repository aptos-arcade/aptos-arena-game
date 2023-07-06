using Characters;
using Photon.Pun;
using Photon.Realtime;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Photon.PlayerPropertyKeys;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace MainMenu
{
    public class TrainingMenuManager : MonoBehaviourPunCallbacks
    {
        
        [SerializeField] private CharacterDisplay characterDisplay;
        
        [SerializeField] private Button continueButton;

        private const string ModePropKey = "m";
    
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
            CharacterCard.OnSelect -= OnCharacterSelect;
        }
    }
}
