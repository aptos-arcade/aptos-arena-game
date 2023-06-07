using Characters;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MainMenu
{
    public class RankedMenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject walletConnectScreen;
        [SerializeField] private GameObject roomSelectScreen;
        [SerializeField] private GameObject customPlayerScreen;
        [SerializeField] private GameObject background;

        [SerializeField] private Button continueToRoomSelectButton;
        [SerializeField] private Button backToGameModeSelectButton;
        [SerializeField] private Button backButton;
        [SerializeField] private Button walletContinueButton;
        
        [SerializeField] private TMP_Text walletAddressText;
        [SerializeField] private TMP_Text headerText;
    
        private void Start()
        {
            walletConnectScreen.SetActive(true);
            customPlayerScreen.SetActive(false);
            roomSelectScreen.SetActive(false);

            walletContinueButton.onClick.AddListener(ContinueToCustomPlayer);
            continueToRoomSelectButton.onClick.AddListener(ContinueToRoomSelect);
            
            if(WalletManager.Instance.IsLoggedIn) OnWalletConnected();
            else walletContinueButton.interactable = false;
            
            backButton.gameObject.SetActive(false);
            
            WalletManager.OnConnect += OnWalletConnected;
            
            #if UNITY_EDITOR
                WalletManager.Instance.SetAccountAddress("0xc09622c20bdd49b2b83b7e05c264a62cfedeb45eaf5c629d0f0174917d801aef");
            #endif
        }
        
        private void OnWalletConnected()
        {
            walletContinueButton.interactable = true;
            walletAddressText.text = WalletManager.Instance.AddressEllipsized;
        }

        private void ContinueToCustomPlayer()
        {
            walletConnectScreen.SetActive(false);
            customPlayerScreen.SetActive(true);
            background.SetActive(false);
            backToGameModeSelectButton.gameObject.SetActive(false);
            SetBackButtonHandler(BackToWalletConnect);
            headerText.text = "Your Brawler";
        }
    
        // private void ContinueToCharacterSelect()
        // {
        //     walletConnectScreen.SetActive(false);
        //     characterSelectScreen.SetActive(true);
        //     backToGameModeSelectButton.gameObject.SetActive(false);
        //     SetBackButtonHandler(BackToWalletConnect);
        // }
    
        private void BackToWalletConnect()
        {
            background.SetActive(true);
            customPlayerScreen.SetActive(false);
            walletConnectScreen.SetActive(true);
            backToGameModeSelectButton.gameObject.SetActive(true);
            backButton.gameObject.SetActive(false);
            headerText.text = "Ranked";
        }

        private void ContinueToRoomSelect()
        {
            background.SetActive(true);
            customPlayerScreen.SetActive(false);
            roomSelectScreen.SetActive(true);
            backToGameModeSelectButton.gameObject.SetActive(false);
            SetBackButtonHandler(BackToCustomPlayer);
            headerText.text = "Ranked";
        }
        
        private void BackToCustomPlayer()
        {
            background.SetActive(false);
            roomSelectScreen.SetActive(false);
            customPlayerScreen.SetActive(true);
            backToGameModeSelectButton.gameObject.SetActive(false);
            SetBackButtonHandler(BackToWalletConnect);
            headerText.text = "Your Brawler";
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
