using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class MusicManager : MonoBehaviour
    {

        private AudioSource _musicSource;

        [SerializeField] private Image muteIcon;
        [SerializeField] private Button muteButton;
        
        private static MusicManager _instance;
        
        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            _musicSource = GetComponent<AudioSource>();
            muteButton.onClick.AddListener(ToggleMusic);
        }

        private void ToggleMusic()
        {
            if(_musicSource.isPlaying)
                _musicSource.Pause();
            else
                _musicSource.Play();
            muteIcon.enabled = _musicSource.isPlaying;
        }
    }
}