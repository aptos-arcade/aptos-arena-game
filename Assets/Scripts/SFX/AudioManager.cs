using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace SFX
{
    public class AudioManager : MonoBehaviour
    {

        [SerializeField] private AudioMixer mixer;
        
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider gameSfxVolumeSlider;
        [SerializeField] private Slider menuSfxVolumeSlider;
        
        private const string MasterVolumeKey = "MasterVolume";
        private const string MusicVolumeKey = "MusicVolume";
        private const string GameSfxVolumeKey = "GameSfxVolume";
        private const string MenuSfxVolumeKey = "MenuSfxVolume";
        
        private const float DefaultValue = 1f;

        private void Start()
        {

            // add listeners to each slider
            masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
            gameSfxVolumeSlider.onValueChanged.AddListener(SetGameSfxVolume);
            menuSfxVolumeSlider.onValueChanged.AddListener(SetMenuSfxVolume);
            
            
            // set slider values to player prefs values
            masterVolumeSlider.value = PlayerPrefs.GetFloat(MasterVolumeKey, DefaultValue);
            musicVolumeSlider.value = PlayerPrefs.GetFloat(MusicVolumeKey, DefaultValue);
            gameSfxVolumeSlider.value = PlayerPrefs.GetFloat(GameSfxVolumeKey, DefaultValue);
            menuSfxVolumeSlider.value = PlayerPrefs.GetFloat(MenuSfxVolumeKey, DefaultValue);
        }
        
        private void SetMasterVolume(float volume)
        {
            var masterVolume = ScaleVolumeSliderValue(volume);
            mixer.SetFloat(MasterVolumeKey, masterVolume);
            PlayerPrefs.SetFloat(MasterVolumeKey, volume);
        }
        
        private void SetMusicVolume(float volume)
        {
            var musicVolume = ScaleVolumeSliderValue(volume);
            mixer.SetFloat(MusicVolumeKey, musicVolume);
            PlayerPrefs.SetFloat(MusicVolumeKey, volume);
        }
        
        private void SetGameSfxVolume(float volume)
        {
            var gameSfxVolume = ScaleVolumeSliderValue(volume);
            mixer.SetFloat(GameSfxVolumeKey, gameSfxVolume);
            PlayerPrefs.SetFloat(GameSfxVolumeKey, volume);
        }
        
        private void SetMenuSfxVolume(float volume)
        {
            var menuSfxVolume = ScaleVolumeSliderValue(volume);
            mixer.SetFloat(MenuSfxVolumeKey, menuSfxVolume);
            PlayerPrefs.SetFloat(MenuSfxVolumeKey, volume);
        }
        
        private static float ScaleVolumeSliderValue(float value)
        {
            return Mathf.Log10(value) * 20;
        }
    }
}