using System;
using Photon.Pun;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
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
        
        
        
        
        
        private void Start()
        {
            openUsernamePanelButton.onClick.AddListener(OpenUsernamePanel);
            closeUsernamePanelButton.onClick.AddListener(CloseUsernamePanel);
            setUsernameButton.onClick.AddListener(SetUsername);
            
            signOutButton.onClick.AddListener(SignOut);
            
            usernameText.text = AuthenticationService.Instance.PlayerName;
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
                var username = await AuthenticationService.Instance.UpdatePlayerNameAsync(usernameInputField.text);
                usernameText.text = username;
                PhotonNetwork.NickName = username;
                CloseUsernamePanel();
            }
            catch (RequestFailedException e)
            {
                usernameErrorText.gameObject.SetActive(true);
                usernameErrorText.text = e.Message;
            }
            
            
        }
        
        private static void SignOut()
        {
            AuthenticationService.Instance.SignOut();
        }

    }
}