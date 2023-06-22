using Gameplay;
using Photon.Pun;
using UnityEngine;

namespace Player
{
    public class PlayerShield : MonoBehaviourPun
    {
        [SerializeField] private float shieldEnergyDuration;
        [SerializeField] private ParticleSystem ps;
        
        public PlayerScript Player { get; private set; }

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
            if (Player.PlayerState.ShieldEnergy <= 0.05)
            {
                ps.Stop();
            }
            transform.localScale = new Vector3(Player.PlayerState.ShieldEnergy, Player.PlayerState.ShieldEnergy, 1);
            Player.PlayerState.ShieldEnergy -= Time.deltaTime / shieldEnergyDuration;
        }

        [PunRPC]
        public void TriggerShield(bool trigger)
        {
            gameObject.SetActive(trigger);
            Player.PlayerState.CanMove = !trigger;
            if (trigger) ps.Play();
            else ps.Stop();
        }
    }
}
