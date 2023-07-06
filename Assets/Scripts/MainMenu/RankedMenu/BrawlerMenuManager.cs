using System.Collections;
using AptosIntegration;
using AptosIntegration.WalletManager;
using TMPro;
using UnityEngine;

namespace MainMenu.RankedMenu
{
    public class BrawlerMenuManager : MonoBehaviour
    {
        
        [SerializeField] private TMP_Text loadingText;
        
        [Header("Managers")]
        [SerializeField] private MintBrawlerManager mintBrawlerManager;
        [SerializeField] private YourBrawlerManager yourBrawlerManager;

        private bool playerHasBrawler;
        
        private void OnEnable()
        {
            StartCoroutine(LoadCoroutine());
            TransactionHandler.OnTransactionResult += OnTransactionResult;
            WalletManager.OnConnect += OnWalletConnect;
        }
        
        private void OnDisable()
        {
            TransactionHandler.OnTransactionResult -= OnTransactionResult;
            WalletManager.OnConnect -= OnWalletConnect;
        }
        
        private void ShowLoading()
        {
            mintBrawlerManager.gameObject.SetActive(false);
            yourBrawlerManager.gameObject.SetActive(false);
            loadingText.text = "Loading...";
            loadingText.gameObject.SetActive(true);
        }

        private IEnumerator LoadCoroutine()
        {
            if(!WalletManager.Instance.IsLoggedIn) yield break;
            ShowLoading();
            yield return StartCoroutine(ApiServices.ApiClient.FetchBrawlerAddress(HandleFetchBrawlerAddress,
                WalletManager.Instance.Address));
        }

        private void HandleFetchBrawlerAddress(string brawlerAddress)
        {
            playerHasBrawler = brawlerAddress != string.Empty;
            loadingText.gameObject.SetActive(false);
            if (playerHasBrawler)
            {
                yourBrawlerManager.gameObject.SetActive(true);
            }
            else
            {
                mintBrawlerManager.gameObject.SetActive(true);
            }
            
        }

        private void OnTransactionResult(bool successful)
        {
            if (!successful || playerHasBrawler) return;
            StartCoroutine(LoadCoroutine());
        }

        private void OnWalletConnect()
        {
            StartCoroutine(LoadCoroutine());
        }
    }
}