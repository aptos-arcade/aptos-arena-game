using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace SFX
{
    public class AudioUiManager : MonoBehaviour
    {
        [Header("Volume Sliders")]
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider gameSfxVolumeSlider;
        [SerializeField] private Slider menuSfxVolumeSlider;
        
        [Header("Modal Controller")]
        [SerializeField] private Button openAudioModalButton;
        [SerializeField] private Button closeAudioModalButton;
        [SerializeField] private GameObject audioModal;

        private const float DefaultValue = 0.5f;

        private void Start()
        {
            openAudioModalButton.onClick.AddListener(() => audioModal.SetActive(true));
            closeAudioModalButton.onClick.AddListener(() => audioModal.SetActive(false));

            // add listeners to each slider
            masterVolumeSlider.onValueChanged.AddListener(AudioManager.Instance.SetMasterVolume);
            musicVolumeSlider.onValueChanged.AddListener(AudioManager.Instance.SetMusicVolume);
            gameSfxVolumeSlider.onValueChanged.AddListener(AudioManager.Instance.SetGameSfxVolume);
            menuSfxVolumeSlider.onValueChanged.AddListener(AudioManager.Instance.SetMenuSfxVolume);
            
            // set slider values to player prefs values
            masterVolumeSlider.value = AudioManager.MasterVolume;
            musicVolumeSlider.value = AudioManager.MusicVolume;
            gameSfxVolumeSlider.value = AudioManager.GameSfxVolume;
            menuSfxVolumeSlider.value = AudioManager.MenuSfxVolume;
        }
    }
}