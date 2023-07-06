using System;
using Characters;
using Photon.Pun;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static Photon.PlayerPropertyKeys;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Random = UnityEngine.Random;

namespace MainMenu
{
    public class CasualMenuManager : MonoBehaviour
    {

        [SerializeField] private GameObject characterSelect;
        [SerializeField] private GameObject roomSelect;
        
        [SerializeField] private CharacterDisplay characterDisplay;
        
        [SerializeField] private Button continueToRoomsButton;
        [SerializeField] private Button backToModeSelectButton;
        [SerializeField] private Button backButton;
        
        // Start is called before the first frame update
        private void Start()
        {
            characterSelect.SetActive(true);
            characterDisplay.gameObject.SetActive(true);
            roomSelect.SetActive(false);
            
            var playerProperties = new Hashtable()
            {
                { CharacterKey, Characters.Characters.GetRandomCharacter() },
                { SwordKey, Random.Range(0, 5) },
                { GunKey, Random.Range(0, 5) }
            };
            PhotonNetwork.SetPlayerCustomProperties(playerProperties);
            characterDisplay.UpdateCharacter();
            
            PhotonNetwork.NickName = AuthenticationService.Instance.PlayerName;
            CharacterCard.OnSelect += OnCharacterSelect;
            continueToRoomsButton.onClick.AddListener(OnContinueToRooms);
        }

        private void OnCharacterSelect()
        {
            characterDisplay.UpdateCharacter();
            
        }

        private void OnContinueToRooms()
        {
            characterSelect.SetActive(false);
            characterDisplay.gameObject.SetActive(false);
            continueToRoomsButton.gameObject.SetActive(false);
            roomSelect.SetActive(true);
            backToModeSelectButton.gameObject.SetActive(false);
            SetBackButtonHandler(OnBackToCharacterSelect);
        }
        
        private void OnBackToCharacterSelect()
        {
            characterSelect.SetActive(true);
            characterDisplay.gameObject.SetActive(true);
            continueToRoomsButton.gameObject.SetActive(true);
            roomSelect.SetActive(false);
            backToModeSelectButton.gameObject.SetActive(true);
            backButton.gameObject.SetActive(false);
        }

        private void SetBackButtonHandler(UnityAction handler)
        {
            backButton.gameObject.SetActive(true);
            backButton.onClick.RemoveAllListeners();
            backButton.onClick.AddListener(handler);
        }
        
        private void OnDestroy()
        {
            CharacterCard.OnSelect -= OnCharacterSelect;
        }
    }
}
