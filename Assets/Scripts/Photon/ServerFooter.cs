using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Photon
{
    public class ServerFooter : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TMP_Text regionText;
        [SerializeField] private TMP_Text serverCountText;
        [SerializeField] private TMP_Text playersInRoomCountText;
        
        [SerializeField] private Button disconnectButton;

        private void Start()
        {
            disconnectButton.onClick.AddListener(PhotonNetwork.Disconnect);
        }

        private void Update()
        {
            regionText.text = $"Region: {PhotonNetwork.CloudRegion}";
            serverCountText.text = $"Players in Region: {PhotonNetwork.CountOfPlayers}";
            playersInRoomCountText.text = $"Players in Games: {PhotonNetwork.CountOfPlayersInRooms}";
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            if(cause != DisconnectCause.DisconnectByClientLogic) return;
            SceneManager.LoadScene("PhotonLoadingScene");
        }
    }
}