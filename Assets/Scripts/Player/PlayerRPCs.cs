using System.Collections;
using Photon.Pun;
using UnityEngine;

namespace Player
{
    public class PlayerRPCs : MonoBehaviourPun
    {
        private PlayerScript player;
    
        // Start is called before the first frame update
        private void Start()
        {
            player = GetComponent<PlayerScript>();
        }

        [PunRPC]
        public void OnDeath()
        {
            player.PlayerUtilities.DeathRevive(false);
            player.PlayerComponents.RigidBody.velocity = Vector2.zero;
            player.PlayerState.Direction = Vector2.zero;
            player.PlayerState.Lives--;
            player.PlayerReferences.PlayerLives.GetChild(player.PlayerState.Lives).gameObject.SetActive(false);
        }

        [PunRPC]
        public void OnRevive()
        {
            player.PlayerUtilities.DeathRevive(true);
            player.PlayerState.DamageMultiplier = 1;
            player.PlayerReferences.DamageDisplay.text = "0%";
        }

        [PunRPC]
        public void OnStrike(Vector2 direction, float knockBack, float damage)
        {
            player.PlayerUtilities.HandleStrike(direction, knockBack, damage);
            StartCoroutine(HurtCoroutine());
        }

        private IEnumerator HurtCoroutine()
        {
            player.PlayerComponents.PhotonView.RPC("HurtEffect", RpcTarget.AllBuffered, true);
            yield return new WaitForSeconds(0.25f);
            player.PlayerComponents.PhotonView.RPC("HurtEffect", RpcTarget.AllBuffered, false);
        }

        [PunRPC]
        public void HurtEffect(bool hurt)
        {
            player.PlayerUtilities.HurtEffect(hurt);
        }
    }
}
