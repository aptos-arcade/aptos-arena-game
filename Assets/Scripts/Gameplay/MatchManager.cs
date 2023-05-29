using Photon.Pun;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Photon.PlayerPropertyKeys;

namespace Gameplay
{
    public class MatchManager : MonoBehaviourPunCallbacks
    {
        // singleton pattern
        public static MatchManager Instance;
        
        private void Awake()
        {
            Instance = this;
        }
        
        public PlayerScript Player { get; set; }
        
        [SerializeField] private SpawnManager spawnManager;
        [SerializeField] private RespawnManager respawnManager;
        
        [SerializeField] private GameObject energyUI;

        [SerializeField] private GameObject outOfLivesUI;
        
        [SerializeField] private Transform[] spawnPositions;
        
        public Vector3 SpawnPosition { get; private set; }

        private void Start()
        {
            SpawnPosition = spawnPositions[(int)PhotonNetwork.LocalPlayer.CustomProperties[TeamKey]].position;
            var playerLayer = LayerMask.NameToLayer("Player");
            Physics2D.IgnoreLayerCollision(playerLayer, playerLayer);
            spawnManager.ShowSpawnPanel();
        }

        public void OnPlayerDeath()
        {
            respawnManager.StartRespawn();
        }

        public void OnPlayerOutOfLives()
        {
            outOfLivesUI.SetActive(true);
        }
        
        public void SetEnergyUIActive(bool active)
        {
            energyUI.SetActive(active);
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene("ModeSelectScene");
        }
    }
}
