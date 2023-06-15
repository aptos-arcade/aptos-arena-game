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
        
        [SerializeField] private Button backToModeSelectButton;
        [SerializeField] private Button backButton;
        
        // Start is called before the first frame update
        private void Start()
        {
            characterSelect.SetActive(true);
            roomSelect.SetActive(false);
            var playerProperties = new Hashtable()
            {
                // { CharacterKey, GetRandomCharacter() },
                { SwordKey, Random.Range(0, 5)},
                { GunKey, Random.Range(0, 5)}
                
            };
            PhotonNetwork.SetPlayerCustomProperties(playerProperties);
            PhotonNetwork.NickName = AuthenticationService.Instance.PlayerName;
            CharacterCard.OnSelect += OnCharacterSelect;
        }

        private void OnCharacterSelect()
        {
            characterSelect.SetActive(false);
            roomSelect.SetActive(true);
            backToModeSelectButton.gameObject.SetActive(false);
            SetBackButtonHandler(OnBackToCharacterSelect);
        }
        
        private void OnBackToCharacterSelect()
        {
            characterSelect.SetActive(true);
            roomSelect.SetActive(false);
            backToModeSelectButton.gameObject.SetActive(true);
            backButton.gameObject.SetActive(false);
        }

        private static CharactersEnum GetRandomCharacter()
        {
            var values = Enum.GetValues(typeof(CharactersEnum));
            return (CharactersEnum)values.GetValue(Random.Range(0, values.Length));
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
