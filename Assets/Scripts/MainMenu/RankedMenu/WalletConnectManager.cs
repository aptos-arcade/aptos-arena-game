using AptosIntegration;
using AptosIntegration.WalletManager;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu.RankedMenu
{
    public class WalletConnectManager : MonoBehaviour
    {

        [SerializeField] private ModalManager modalManager;
        
        [SerializeField] private Button walletContinueButton;
        [SerializeField] private Button walletConnectButton;
        
        [SerializeField] private TMP_Text walletAddressText;
        [SerializeField] private TMP_Text messageText;
    
        private void Start()
        {
            OnWalletConnected();
            
            WalletManager.OnConnect += OnWalletConnected;

            walletConnectButton.onClick.AddListener(WalletManager.OpenConnectWalletModal);

            #if UNITY_EDITOR
                WalletManager.Instance.SetAccountAddress("0xc09622c20bdd49b2b83b7e05c264a62cfedeb45eaf5c629d0f0174917d801aef");
            #endif
        }
        
        private void OnWalletConnected()
        {
            walletContinueButton.gameObject.SetActive(WalletManager.Instance.IsLoggedIn);
            walletConnectButton.gameObject.SetActive(!WalletManager.Instance.IsLoggedIn);
            if (WalletManager.Instance.IsLoggedIn)
            {
                walletAddressText.text = WalletManager.Instance.AddressEllipsized;
                StartCoroutine(AnsResolver.ResolveAns(ResolveAnsHandler, WalletManager.Instance.Address));
                messageText.gameObject.SetActive(false);
            }
            else
            {
                walletAddressText.text = "No Wallet Connected";
                messageText.gameObject.SetActive(true);
                messageText.text = "Please connect your wallet in your browser";
            }

        }

        private void ResolveAnsHandler(string ansName)
        {
            if (ansName == string.Empty) return;
            PhotonNetwork.NickName = $"{ansName}.apt";
            walletAddressText.text = $"{ansName}.apt";
        }

        private void OnDestroy()
        {
            WalletManager.OnConnect -= OnWalletConnected;
        }
    }
}