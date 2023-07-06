// using TMPro;
// using UnityEngine;
// using UnityEngine.UI;
//
// namespace AptosIntegration.WalletManager
// {
//     public class WalletButton : MonoBehaviour
//     {
//         [SerializeField] private Button walletButton;
//         [SerializeField] private TMP_Text buttonText;
//         
//         [SerializeField] private string walletName;
//         
//         private void Start()
//         {
//             walletButton.onClick.AddListener(OnClick);
//             buttonText.text = walletName;
//         }
//         
//         private void OnClick()
//         {
//             WalletManager.ConnectWallet(walletName);
//         }
//     }
// }