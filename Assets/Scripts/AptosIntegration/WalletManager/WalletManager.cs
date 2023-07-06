using System.Runtime.InteropServices;
using Photon.Pun;
using UnityEngine;
using static Photon.PlayerPropertyKeys;

namespace AptosIntegration.WalletManager
{
    public class WalletManager : MonoBehaviour
    {

        public static WalletManager Instance;
    
        private void Awake()
        {
            Instance = this;
        }

        public string Address { get; private set; }
        public bool IsLoggedIn => !string.IsNullOrEmpty(Address);
        public string AddressEllipsized => Ellipsize(Address);
        
        [DllImport("__Internal")]
        private static extern void SetConnectModalOpen(int isOpen);
    
        public delegate void WalletConnectedAction();
        public static event WalletConnectedAction OnConnect;
    
        public void SetAccountAddress(string accountAddress)
        {
            Address = accountAddress;
            PlayerPrefs.SetString(AccountAddressKey, accountAddress);
            var playerProperties = PhotonNetwork.LocalPlayer.CustomProperties;
            playerProperties[AccountAddressKey] = accountAddress;
            PhotonNetwork.SetPlayerCustomProperties(playerProperties);
            OnConnect?.Invoke();
        }
    
        private static string Ellipsize(string str, int length = 6)
        {
            return str[..length] + "..." + str[^length..];
        }
        
        public static void OpenConnectWalletModal()
        {
            #if UNITY_WEBGL == true && UNITY_EDITOR == false
                SetConnectModalOpen(1);
            #endif
        }

    }
}
