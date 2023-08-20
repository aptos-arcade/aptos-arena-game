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
    public class AccountMenuManager : MonoBehaviour
    {
        
        [Header("Username Panel")]
        [SerializeField] private GameObject setUsernamePanel;
        [SerializeField] private TMP_InputField usernameInputField;
        [SerializeField] private Button setUsernameButton;
        [SerializeField] private Button closeUsernamePanelButton;
        [SerializeField] private TMP_Text usernameErrorText;
        
        [Header("Buttons Panel")]
        [SerializeField] private GameObject buttonsPanel;
        [SerializeField] private Button openUsernamePanelButton;
        [SerializeField] private Button signOutButton;
        
        [Header("Displays")]
        [SerializeField] private TMP_Text usernameText;
        [SerializeField] private Modal loadingModal;
        
        private void Start()
        {
            openUsernamePanelButton.onClick.AddListener(OpenUsernamePanel);
            closeUsernamePanelButton.onClick.AddListener(CloseUsernamePanel);
            setUsernameButton.onClick.AddListener(SetUsername);
            
            #if UNITY_EDITOR
                signOutButton.gameObject.SetActive(true);
                signOutButton.onClick.AddListener(SignOut);
            #endif
            
            usernameText.text = AuthenticationService.Instance.PlayerName.Split("#")[0];
        }
        
        private void OpenUsernamePanel()
        {
            setUsernamePanel.SetActive(true);
            buttonsPanel.SetActive(false);
        }
        
        private void CloseUsernamePanel()
        {
            setUsernamePanel.SetActive(false);
            buttonsPanel.SetActive(true);
        }
        
        private async void SetUsername()
        {
            try
            {
                loadingModal.Show();
                await AuthenticationService.Instance.UpdatePlayerNameAsync(usernameInputField.text);
                PhotonNetwork.NickName = usernameInputField.text;
                StartCoroutine(PlayerServices.SetPlayerName(AuthenticationService.Instance.PlayerId, 
                    usernameInputField.text, success =>
                {
                    if (success)
                    {
                        usernameText.text = usernameInputField.text;
                        CloseUsernamePanel();
                    }
                    else
                    {
                        usernameErrorText.gameObject.SetActive(true);
                        usernameErrorText.text = "Username already taken";
                    }
                    loadingModal.Hide();
                }));
            }
            catch (RequestFailedException e)
            {
                usernameErrorText.gameObject.SetActive(true);
                usernameErrorText.text = e.Message;
                loadingModal.Hide();
            }
        }
        
        private static void SignOut()
        {
            AuthenticationService.Instance.SignOut();
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene("LaunchScene");
        }

    }
}