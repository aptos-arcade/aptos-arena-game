using System;
using System.Threading.Tasks;
using Player.PlayerStats;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.RemoteConfig;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UGS
{
    public class UnityAuthentication : MonoBehaviour
    {
        
        public struct userAttributes {}

        public struct appAttributes {}
        
        [SerializeField] private PlayerStats playerStats;

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
            }
            catch (AuthenticationException ex)
            {
                
                Debug.LogException(ex);
            }
            catch (RequestFailedException ex)
            {
                Debug.LogException(ex);
            }
        }

        private void ApplyRemoteSettings (ConfigResponse configResponse) {
            // Conditionally update settings, depending on the response's origin:
            switch (configResponse.requestOrigin) {
                case ConfigOrigin.Default:
                    Debug.Log ("No settings loaded this session; using default values.");
                    break;
                case ConfigOrigin.Cached:
                    Debug.Log ("No settings loaded this session; using cached values from a previous session.");
                    break;
                case ConfigOrigin.Remote:
                    Debug.Log ("New settings loaded this session; update values accordingly.");
                    var jsonCubeString = RemoteConfigService.Instance.appConfig.GetJson("PlayerStats");
                    JsonUtility.FromJsonOverwrite(jsonCubeString, playerStats);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            SceneManager.LoadScene("PhotonLoadingScene");
        }


        private void SetupEvents()
        {
            AuthenticationService.Instance.SignedIn += OnSignedIn;
            AuthenticationService.Instance.SignInFailed += OnSignInFailed;
            AuthenticationService.Instance.SignedOut += OnSignedOut;
            AuthenticationService.Instance.Expired += OnExpired;
            
            RemoteConfigService.Instance.FetchCompleted += ApplyRemoteSettings;
        }

        private static void OnSignedIn()
        {
            RemoteConfigService.Instance.FetchConfigs(new userAttributes(), new appAttributes());
        }

        private static void OnSignInFailed(RequestFailedException err)
        {
        }
        
        private static void OnSignedOut()
        {
        }
        
        private static void OnExpired()
        {
        }
    }
}
