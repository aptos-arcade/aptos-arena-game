using UnityEngine;
using UnityEngine.Audio;

namespace SFX
{
    public class AudioManager : MonoBehaviour
    {
        
        public static AudioManager Instance { get; private set; }

        [SerializeField] private AudioMixer mixer;
        
        public static float MasterVolume => PlayerPrefs.GetFloat(MasterVolumeKey, DefaultMasterValue);
        public static float MusicVolume => PlayerPrefs.GetFloat(MusicVolumeKey, DefaultMusicValue);
        public static float GameSfxVolume => PlayerPrefs.GetFloat(GameSfxVolumeKey, DefaultGameSfxValue);
        public static float MenuSfxVolume => PlayerPrefs.GetFloat(MenuSfxVolumeKey, DefaultMenuSfxValue);

        private const string MasterVolumeKey = "MasterVolume";
        private const string MusicVolumeKey = "MusicVolume";
        private const string GameSfxVolumeKey = "GameSfxVolume";
        private const string MenuSfxVolumeKey = "MenuSfxVolume";
        
        private const float DefaultMasterValue = 1f;
        private const float DefaultMusicValue = 0.25f;
        private const float DefaultGameSfxValue = 0.5f;
        private const float DefaultMenuSfxValue = 0.5f;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                return;
            }
            
            Destroy(gameObject);
        }

        private void Start()
        {
            SetMasterVolume(MasterVolume);
            SetMusicVolume(MusicVolume);
            SetGameSfxVolume(GameSfxVolume);
            SetMenuSfxVolume(MenuSfxVolume);
        }
        
        public void SetMasterVolume(float volume)
        {
            var masterVolume = ScaleVolumeSliderValue(volume);
            mixer.SetFloat(MasterVolumeKey, masterVolume);
            PlayerPrefs.SetFloat(MasterVolumeKey, volume);
        }
        
        public void SetMusicVolume(float volume)
        {
            var musicVolume = ScaleVolumeSliderValue(volume);
            mixer.SetFloat(MusicVolumeKey, musicVolume);
            PlayerPrefs.SetFloat(MusicVolumeKey, volume);
        }
        
        public void SetGameSfxVolume(float volume)
        {
            var gameSfxVolume = ScaleVolumeSliderValue(volume);
            mixer.SetFloat(GameSfxVolumeKey, gameSfxVolume);
            PlayerPrefs.SetFloat(GameSfxVolumeKey, volume);
        }
        
        public void SetMenuSfxVolume(float volume)
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