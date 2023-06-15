using Photon.Pun;
using UnityEngine;
using static Photon.PlayerPropertyKeys;

namespace AptosIntegration
{
    public class WalletManager : MonoBehaviour
    {

        public static WalletManager Instance;
    
        private void Awake()
        {
            Instance = this;
        }

        public string Address { get; private set; }
        public bool IsLoggedIn => Address != null;
        public string AddressEllipsized => Ellipsize(Address);
    
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

    }
}
