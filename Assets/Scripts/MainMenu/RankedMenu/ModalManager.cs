using UnityEngine;
using UnityEngine.UI;

namespace MainMenu.RankedMenu
{
    public class ModalManager : MonoBehaviour
    {
        [SerializeField] private GameObject mainOverlay;
        [SerializeField] private GameObject loadingOverlay;
    
        [SerializeField] private Button closeButton;
    
        [Header("Modals")]
        [SerializeField] private AvailableCharactersModal availableCharactersModal;

        private void Start()
        {
            CloseModals();
        
            closeButton.onClick.AddListener(CloseModals);
        }

        public void OpenAvailableCharactersModal()
        {
            CloseModals();
            mainOverlay.SetActive(true);
            availableCharactersModal.gameObject.SetActive(true);
        }

        public void CloseModals()
        {
            availableCharactersModal.gameObject.SetActive(false);
            mainOverlay.SetActive(false);
            loadingOverlay.SetActive(false);
        }
    
        public void ShowLoading(bool active)
        {
            loadingOverlay.SetActive(active);
        }
    }
}
