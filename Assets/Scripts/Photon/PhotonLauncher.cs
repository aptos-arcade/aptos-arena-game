using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Photon.PlayerPropertyKeys;

namespace Photon
{
    public class PhotonLauncher : MonoBehaviourPunCallbacks
    {

        [SerializeField] private TMP_Text loadingText;
        [SerializeField] private GameObject regionList;

        [SerializeField] private RegionButton regionButtonPrefab;
        
        [SerializeField] private Transform topRegions;
        [SerializeField] private Transform regionListContent;
        
        [SerializeField] private Button seeRegionListButton;
        [SerializeField] private GameObject regionScrollRect;


        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            ServerSettings.ResetBestRegionCodeInPreferences();
        }
        
        private void Start()
        {
            if (PhotonNetwork.IsConnected)
            {
                SceneManager.LoadScene("UsernameScene");
            }
            else
            {
                PhotonNetwork.NetworkingClient.AppId = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime;
                PhotonNetwork.NetworkingClient.AppVersion = PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion;
                PhotonNetwork.NetworkingClient.ConnectToNameServer();
                
                seeRegionListButton.onClick.AddListener(() =>
                {
                    topRegions.gameObject.SetActive(false);
                    regionScrollRect.SetActive(true);
                    seeRegionListButton.gameObject.SetActive(false);
                });
            }
        }

        public override void OnRegionListReceived(RegionHandler regionHandler)
        {
            regionHandler.PingMinimumOfRegions((handler) =>
            {
                loadingText.gameObject.SetActive(false);
                
                regionHandler.EnabledRegions.Sort((a, b) => a.Ping.CompareTo(b.Ping));
                
                foreach (var region in handler.EnabledRegions.GetRange(0, 3))
                {
                    var regionButton = Instantiate(regionButtonPrefab, topRegions);
                    regionButton.Initialize(region);
                }
                topRegions.gameObject.SetActive(true);
                regionList.SetActive(true);
                
                foreach (var region in handler.EnabledRegions)
                {
                    var regionButton = Instantiate(regionButtonPrefab, regionListContent);
                    regionButton.Initialize(region);
                }
            }, regionHandler.SummaryToCache);
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected to Master");
            regionList.SetActive(false);
            loadingText.gameObject.SetActive(true);
            loadingText.text = "Joining Lobby...";
            PhotonNetwork.JoinLobby(TypedLobby.Default);
        }
        
        public override void OnJoinedLobby()
        {
            Debug.Log("Joined Lobby");
            // add player id to Photon Custom Player Properties
            PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable
            {
                {PlayerIdKey, AuthenticationService.Instance.PlayerId}
            });
            SceneManager.LoadScene("UsernameScene");
        }
        
        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarningFormat("Disconnected with reason {0}", cause);
            loadingText.text = "Disconnected from server";
            loadingText.color = Color.red;
        }
    }
}
