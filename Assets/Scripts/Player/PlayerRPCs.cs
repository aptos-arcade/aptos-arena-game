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
            player.PlayerComponents.Animator.OnDeath();
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
        public void OnStrike(Vector2 direction, float knockBack, float damage, int strikerActorNumber)
        {
            player.StartCoroutine(player.PlayerComponents.PlayerCamera.Shake(0.2f, 0.1f));
            player.PlayerState.StrikerActorNumber = strikerActorNumber;
            player.PlayerState.DamageMultiplier += damage;
            player.PlayerReferences.DamageDisplay.text = ((player.PlayerState.DamageMultiplier - 1) * 100).ToString("F0") + "%";
            player.PlayerUtilities.PlayOneShotAudio(player.PlayerReferences.DamageAudioClip);
            player.PlayerComponents.RigidBody.velocity =
                direction.normalized * knockBack * player.PlayerState.DamageMultiplier;
        }
        
        [PunRPC]
        public void OnShieldStrike(float damage)
        {
            player.PlayerState.ShieldEnergy -= damage;
        }
        
        [PunRPC]
        public void ShieldStunEffect(bool stunned)
        {
            player.PlayerUtilities.StunEffect(stunned, new Color(240, 0, 255));
        }

        [PunRPC]
        public void HurtEffect(bool hurt)
        {
            player.PlayerUtilities.StunEffect(hurt, Color.red);
        }

        [PunRPC]
        public void DodgeEffect(bool dodging)
        {
            player.PlayerUtilities.DodgeEffect(dodging);
        }

        [PunRPC]
        public void TriggerInvincibility(bool invincible)
        {
            player.PlayerUtilities.TriggerInvincibility(invincible);
        }
    }
}
