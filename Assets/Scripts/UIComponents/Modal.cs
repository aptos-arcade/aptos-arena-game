using UnityEngine;

namespace UIComponents
{
    public class Modal : MonoBehaviour
    {
        [SerializeField] private GameObject modalOverlay;
        
        public void Show()
        {
            modalOverlay.SetActive(true);
        }
        
        public void Hide()
        {
            modalOverlay.SetActive(false);
        }
    }
}