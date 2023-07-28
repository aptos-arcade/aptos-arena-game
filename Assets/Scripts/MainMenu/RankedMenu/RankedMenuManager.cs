using AptosIntegration;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MainMenu.RankedMenu
{
    public class RankedMenuManager : MonoBehaviour
    {
        [Header("Screens")]
        [SerializeField] private GameObject walletConnectScreen;
        [SerializeField] private GameObject brawlerMenuScreen;
        [SerializeField] private GameObject roomSelectScreen;

        [Header("Continue Buttons")]
        [SerializeField] private Button continueToBrawlerMenuButton;
        [SerializeField] private Button continueToRoomSelectButton;
        
        [Header("Back Button")]
        [SerializeField] private Button backButton;
        [SerializeField] private Button backToGameModeSelectButton;

        [Header("Header Text")]
        [SerializeField] private TMP_Text headerText;
    
        private void Start()
        {
            CloseMenus();
            walletConnectScreen.SetActive(true);
            backToGameModeSelectButton.gameObject.SetActive(true);

            continueToBrawlerMenuButton.onClick.AddListener(ContinueToBrawlerMenu);
            continueToRoomSelectButton.onClick.AddListener(ContinueToRoomSelect);

            backButton.gameObject.SetActive(false);
            
            WalletManager.OnConnect += OnWalletConnect;
        }

        private void ContinueToBrawlerMenu()
        {
            CloseMenus();
            brawlerMenuScreen.SetActive(true);
            SetBackButtonHandler(BackToWalletConnect);
            headerText.text = "Your Brawler";
        }

        private void BackToWalletConnect()
        {
            CloseMenus();
            walletConnectScreen.SetActive(true);
            backToGameModeSelectButton.gameObject.SetActive(true);
            headerText.text = "Ranked";
        }

        private void ContinueToRoomSelect()
        {
            CloseMenus();
            roomSelectScreen.SetActive(true);
            SetBackButtonHandler(BackToBrawlerMenu);
            headerText.text = "Game Modes";
        }
        
        private void BackToBrawlerMenu()
        {
            CloseMenus();
            brawlerMenuScreen.SetActive(true);
            SetBackButtonHandler(BackToWalletConnect);
            headerText.text = "Your Brawler";
        }
        
        private void CloseMenus()
        {
            walletConnectScreen.SetActive(false);
            brawlerMenuScreen.SetActive(false);
            roomSelectScreen.SetActive(false);
            backToGameModeSelectButton.gameObject.SetActive(false);
            backButton.gameObject.SetActive(false);
        }

        private void SetBackButtonHandler(UnityAction handler)
        {
            backButton.gameObject.SetActive(true);
            backButton.onClick.RemoveAllListeners();
            backButton.onClick.AddListener(handler);
        }
        
        private void OnWalletConnect()
        {
            if(walletConnectScreen.activeSelf) return;
            CloseMenus();
            walletConnectScreen.SetActive(true);
            backToGameModeSelectButton.gameObject.SetActive(true);
        }

        private void OnDestroy()
        {
            WalletManager.OnConnect -= OnWalletConnect;
        }
    }
}
