using Photon.Pun;
using Player;
using UnityEngine;

namespace Weapons
{
    public class Striker: MonoBehaviourPun
    {

        public StrikerData strikerData;

        public Vector2 KnockBackSignedDirection { get; protected set; }
        
        [SerializeField] private GameObject hitEffect;

        private void Start()
        {
            KnockBackSignedDirection = strikerData.KnockBackDirection;
        }

        protected virtual void OnPlayerStrike(Vector2 position, PlayerScript player)
        {
            PhotonNetwork.Instantiate(hitEffect.name, position, Quaternion.identity);
        }
        
        protected virtual void OnShieldStrike(Vector2 position, PlayerShield shield)
        {
            PhotonNetwork.Instantiate(hitEffect.name, position, Quaternion.identity);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if(!photonView.IsMine) return;
            
            var player = col.GetComponent<PlayerScript>();
            if (player != null && !player.photonView.IsMine
                               && !player.PlayerUtilities.IsSameTeam(photonView) && !player.PlayerState.IsInvincible)
            {
                OnPlayerStrike(col.transform.position, player);
                player.PlayerUtilities.StrikerCollision(this);
                return;
            }
            
            var shield = col.GetComponent<PlayerShield>();
            if (shield != null && !shield.photonView.IsMine
                               && !shield.Player.PlayerUtilities.IsSameTeam(photonView))
            {
                OnShieldStrike(col.transform.position, shield);
                shield.Player.PlayerUtilities.ShieldCollision(this);
            }
            
        }
    }
}