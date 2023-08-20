using ApiServices;
using Photon.Pun;
using TMPro;
using UIComponents;
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
        [SerializeField] private Modal loadingModal;
        
        private const string NextScene = "ModeSelectScene";

        private const string HasSetNameKey = "HasSetName";
        
        private void Start()
        {
            if (AuthenticationService.Instance.PlayerName != null)
            {
                var hasSetName = PlayerPrefs.GetInt(HasSetNameKey, 0);
                if (hasSetName != 0)
                {
                    OnSetUsername(AuthenticationService.Instance.PlayerName.Split('#')[0]);
                }
            }
            setUsernameButton.onClick.AddListener(SetUsername);
        }

        private async void SetUsername()
        {
            try
            {
                loadingModal.Show();
                await AuthenticationService.Instance.UpdatePlayerNameAsync(usernameInputField.text);
                StartCoroutine(PlayerServices.SetPlayerName(AuthenticationService.Instance.PlayerId, 
                    usernameInputField.text, success =>
                {
                    if (success)
                    {
                        PlayerPrefs.SetInt(HasSetNameKey, 1);
                        OnSetUsername(usernameInputField.text);
                    }
                    else
                    {
                        usernameErrorText.gameObject.SetActive(true);
                        usernameErrorText.text = "Username already taken";
                        loadingModal.Hide();
                    }
                }));
            }
            catch (RequestFailedException e)
            {
                usernameErrorText.gameObject.SetActive(true);
                usernameErrorText.text = e.Message;
                loadingModal.Hide();
            }
        }
        
        private static void OnSetUsername(string username)
        {
            PhotonNetwork.NickName = username;
            SceneManager.LoadScene(NextScene);
        }
    }
}