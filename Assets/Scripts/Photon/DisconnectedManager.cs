using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Photon
{
    public class DisconnectedManager : MonoBehaviourPunCallbacks
    {

        private static DisconnectedManager _instance;
        
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        [SerializeField] private GameObject disconnectUI;
        [SerializeField] private Button reconnectButton;
        [SerializeField] private TMP_Text disconnectText;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            reconnectButton.onClick.AddListener(Reconnect);
            disconnectUI.SetActive(false);
        }

        private void Reconnect()
        {
            PhotonNetwork.Reconnect();
            disconnectText.text = "Reconnecting...";
            disconnectText.color = Color.white;
        }
        
        public override void OnConnectedToMaster()
        {
            disconnectText.text = "Joining Lobby...";
            PhotonNetwork.JoinLobby(TypedLobby.Default);
        }
        
        public override void OnJoinedLobby()
        {
            disconnectUI.SetActive(false);
            
        }
        
        public override void OnDisconnected(DisconnectCause cause)
        {
            if(DisconnectCause.DisconnectByClientLogic == cause) return;
            disconnectText.text = "Disconnected from server: " + cause;
            disconnectText.color = Color.red;
            disconnectUI.SetActive(true);
        }
    }
}