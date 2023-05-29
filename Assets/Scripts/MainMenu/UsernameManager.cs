using Photon.Pun;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainMenu
{
    public class UsernameManager : MonoBehaviour
    {
        [SerializeField] private TMP_InputField usernameInputField;
        [SerializeField] private Button setUsernameButton;
        [SerializeField] private TMP_Text usernameErrorText;
        
        private const string NextScene = "ModeSelectScene";
        
        private async void Start()
        {
            if (AuthenticationService.Instance.PlayerName == null)
            {
                var username = await AuthenticationService.Instance.GetPlayerNameAsync();
                if(username != null) OnSetUsername(username);
                else setUsernameButton.onClick.AddListener(SetUsername);
            }
            else
            {
                OnSetUsername(AuthenticationService.Instance.PlayerName);
            }
        }

        private async void SetUsername()
        {
            try
            {
                var username = await AuthenticationService.Instance.UpdatePlayerNameAsync(usernameInputField.text);
                OnSetUsername(username);
            }
            catch (RequestFailedException e)
            {
                usernameErrorText.gameObject.SetActive(true);
                usernameErrorText.text = e.Message;
            }
            
        }
        
        private static void OnSetUsername(string username)
        {
            PhotonNetwork.NickName = username;
            SceneManager.LoadScene(NextScene);
        }
    }
}