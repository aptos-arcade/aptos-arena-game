using AptosIntegration;
using UnityEngine;

namespace MainMenu.RankedMenu
{
    public class ConnectWalletModal : MonoBehaviour
    {
        [SerializeField] private ModalManager modalManager;
        
        private void OnEnable()
        {
            WalletManager.OnConnect += OnWalletConnect;
        }

        private void OnDisable()
        {
            WalletManager.OnConnect -= OnWalletConnect;
        }

        private void OnWalletConnect()
        {
            modalManager.CloseModals();
        }
    }
}