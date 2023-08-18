using Photon.Pun;
using UnityEngine;

namespace Player
{
    public class PlayerShield : MonoBehaviourPun
    {
        public PlayerScript Player { get; private set; }
        
        public float ShieldStunDuration => Player.PlayerStats.ShieldStunDuration * transform.localScale.x;

        
        private void Awake()
        {
            Player = GetComponentInParent<PlayerScript>();
        }
        
        private void Update()
        {
            if (!photonView.IsMine) return;
            if(Player.PlayerState.ShieldEnergy <= 0 || Player.PlayerComponents.Animator.CurrentAnimationBody != "Body_Shield")
            {
                Player.PlayerActions.TriggerShield(false);
                Player.PlayerComponents.Animator.OnAnimationDone("Body_Shield");
                Player.PlayerComponents.Animator.OnAnimationDone("Legs_Shield");
            }
            var scale = 0.1f + 0.9f * Player.PlayerState.ShieldEnergy;
            transform.localScale = new Vector3(scale, scale, 1);
            Player.PlayerState.ShieldEnergy -= Time.deltaTime / Player.PlayerStats.ShieldDuration;
        }

        [PunRPC]
        public void TriggerShield(bool trigger)
        {
            gameObject.SetActive(trigger);
            Player.PlayerState.IsInvincible = trigger;
        }
    }
}
