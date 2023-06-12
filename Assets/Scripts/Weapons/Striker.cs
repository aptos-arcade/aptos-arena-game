using Photon.Pun;
using Player;
using UnityEngine;

namespace Weapons
{
    public class Striker: MonoBehaviourPun
    {
        [SerializeField] private float damage;
        public float Damage { get => damage; set => damage = value; }
        
        [SerializeField] private float knockBackForce;
        public float KnockBackForce { get => knockBackForce; set => knockBackForce = value; }
        
        public Vector2 KnockBackDirection { get; set; }

        public Vector2 KnockBackSignedDirection { get; protected set; }
        
        [SerializeField] private GameObject hitEffect;

        private void Start()
        {
            KnockBackSignedDirection = KnockBackDirection;
        }

        protected virtual void OnStrike(Vector2 position)
        {
            PhotonNetwork.Instantiate(hitEffect.name, position, Quaternion.identity);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            var player = col.GetComponent<PlayerScript>();
            if (player == null || !photonView.IsMine || player.photonView.IsMine ||
                player.PlayerUtilities.IsSameTeam(photonView) || col.GetType() != typeof(PolygonCollider2D)) return;
            OnStrike(col.transform.position);
            player.PlayerUtilities.StrikerCollision(this);
        }
    }
}