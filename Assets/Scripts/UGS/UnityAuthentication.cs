using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UGS
{
    public class UnityAuthentication : MonoBehaviour
    {
        private async void Awake()
        {
            try
            {
                await UnityServices.InitializeAsync();
                SetupEvents();
                await SignInAnonymouslyAsync();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private static async Task SignInAnonymouslyAsync()
        {
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log("Sign in anonymously succeeded!");

                // Shows how to get the playerID
                Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

            }
            catch (AuthenticationException ex)
            {
                // Compare error code to AuthenticationErrorCodes
                // Notify the player with the proper error message
                Debug.LogException(ex);
            }
            catch (RequestFailedException ex)
            {
                // Compare error code to CommonErrorCodes
                // Notify the player with the proper error message
                Debug.LogException(ex);
            }
        }

        private static void SetupEvents()
        {
            AuthenticationService.Instance.SignedIn += OnSignedIn;
            AuthenticationService.Instance.SignInFailed += OnSignInFailed;
            AuthenticationService.Instance.SignedOut += OnSignedOut;
            AuthenticationService.Instance.Expired += OnExpired;
        }

        private static void OnSignedIn()
        {
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

            // Shows how to get an access token
            Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");

            SceneManager.LoadScene("PhotonLoadingScene");
        }

        private static void OnSignInFailed(RequestFailedException err)
        {
            Debug.LogError(err);
        }
        
        private static void OnSignedOut()
        {
            Debug.Log("Player signed out.");
        }
        
        private static void OnExpired()
        {
            Debug.Log("Player session could not be refreshed and expired.");
        }
    }
}
