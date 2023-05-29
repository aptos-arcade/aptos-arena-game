using System;
using Characters;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MainMenu
{
    public class RankedMenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject walletConnectScreen;
        [SerializeField] private GameObject characterSelectScreen;
        [SerializeField] private GameObject roomSelectScreen;

        [SerializeField] private Button backToGameModeSelectButton;
        [SerializeField] private Button backButton;
        [SerializeField] private Button walletContinueButton;
    
        private void Start()
        {
            walletConnectScreen.SetActive(false);
            characterSelectScreen.SetActive(true);
            roomSelectScreen.SetActive(false);
        
            walletContinueButton.onClick.AddListener(ContinueToCharacterSelect);
            backButton.gameObject.SetActive(false);
            
            CharacterCard.OnSelect += ContinueToRoomSelect;
        }
    
        private void ContinueToCharacterSelect()
        {
            walletConnectScreen.SetActive(false);
            characterSelectScreen.SetActive(true);
            backToGameModeSelectButton.gameObject.SetActive(false);
            SetBackButtonHandler(BackToWalletConnect);
        }
    
        private void BackToWalletConnect()
        {
            characterSelectScreen.SetActive(false);
            walletConnectScreen.SetActive(true);
            backToGameModeSelectButton.gameObject.SetActive(true);
            backButton.gameObject.SetActive(false);
        }

        private void ContinueToRoomSelect()
        {
            characterSelectScreen.SetActive(false);
            roomSelectScreen.SetActive(true);
            backToGameModeSelectButton.gameObject.SetActive(false);
            SetBackButtonHandler(BackToCharacterSelect);
        }
    
        private void BackToCharacterSelect()
        {
            roomSelectScreen.SetActive(false);
            characterSelectScreen.SetActive(true);
            backToGameModeSelectButton.gameObject.SetActive(true);
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
            CharacterCard.OnSelect -= ContinueToRoomSelect;
        }
    }
}
