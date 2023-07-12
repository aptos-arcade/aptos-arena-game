using UnityEngine;
using UnityEngine.UI;

namespace SFX
{
    public class AudioModalManager : MonoBehaviour
    {
        
        [SerializeField] private Button openAudioModalButton;
        [SerializeField] private Button closeAudioModalButton;
        
        [SerializeField] private GameObject audioModal;
        
        private void Start()
        {
            openAudioModalButton.onClick.AddListener(() => audioModal.SetActive(true));
            closeAudioModalButton.onClick.AddListener(() => audioModal.SetActive(false));
        }
    }
}