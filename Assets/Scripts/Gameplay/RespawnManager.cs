using Photon.Pun;
using TMPro;
using UnityEngine;
using static Photon.PlayerPropertyKeys;

namespace Gameplay
{
    public class RespawnManager : MonoBehaviour
    {
        // references
        [SerializeField] private GameObject respawnPanel;
        [SerializeField] private TMP_Text respawnTimer;
        [SerializeField] private float respawnDuration = 5;
    
        // state
        private bool isRespawning;
        private float respawnTime;

        // Update is called once per frame
        private void Update()
        {
            if (isRespawning) UpdateRespawn();
        }

        public void StartRespawn()
        {
            respawnPanel.gameObject.SetActive(true);
            respawnTime = respawnDuration;
            isRespawning = true;
        }
    
        private void UpdateRespawn()
        {
            respawnTime -= Time.deltaTime;
            respawnTimer.text = "Respawn in: " + respawnTime.ToString("F0");
            if (respawnTime > 0) return;
            Respawn();
        }

        private void Respawn()
        {
            respawnPanel.gameObject.SetActive(false);
            isRespawning = false;
            StartCoroutine(
                MatchManager.Instance.Player.PlayerUtilities.SpawnCoroutine(MatchManager.Instance.SpawnPosition));
        }
    }
}
