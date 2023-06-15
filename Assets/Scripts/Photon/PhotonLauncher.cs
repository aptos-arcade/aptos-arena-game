using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Photon
{
    public class PhotonLauncher : MonoBehaviourPunCallbacks
    {

        [SerializeField] private TMP_Text loadingText;
        
        private const string GameVersion = "1";
        
        private void Awake()
        {
            // #Critical
            // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
            PhotonNetwork.AutomaticallySyncScene = true;
            ServerSettings.ResetBestRegionCodeInPreferences();
        }
        
        private void Start()
        {
            if (!PhotonNetwork.IsConnected)
            {
                loadingText.text = "Connecting to server...";
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = GameVersion;
            }
            else
            {
                SceneManager.LoadScene("UsernameScene");
            }
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected to Master");
            loadingText.text = "Joining Lobby...";
            PhotonNetwork.JoinLobby(TypedLobby.Default);
        }
        
        public override void OnJoinedLobby()
        {
            Debug.Log("Joined Lobby");
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
