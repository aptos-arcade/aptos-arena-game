using AptosIntegration;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu.RankedMenu
{
    public class WalletConnectManager : MonoBehaviour
    {
        [SerializeField] private Button walletContinueButton;
        [SerializeField] private Button walletConnectButton;
        
        [SerializeField] private TMP_Text walletAddressText;
        [SerializeField] private TMP_Text messageText;
        
        [SerializeField] private TMP_InputField walletAddressInputField;
    
        private void Start()
        {
            OnWalletConnected();
            
            WalletManager.OnConnect += OnWalletConnected;

            walletConnectButton.onClick.AddListener(WalletManager.OpenConnectWalletModal);

            walletAddressInputField.gameObject.SetActive(false);
            #if UNITY_EDITOR
                walletAddressInputField.gameObject.SetActive(true);
                walletAddressInputField.onEndEdit.AddListener((val) => WalletManager.Instance.SetAccountAddress(val));
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