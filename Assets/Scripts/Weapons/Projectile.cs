using System.Collections;
using Photon.Pun;
using Player;
using UnityEngine;

namespace Weapons
{
    public class Projectile: Striker
    {
        [SerializeReference] private float speed;
        
        [SerializeField] private float destroyTime;

        private Vector2 direction;
        
        private void Start()
        {
            if (!photonView.IsMine) return;
            StartCoroutine(DestroyBullet());
        }


        // Update is called once per frame
        private void Update()
        {
            transform.Translate(speed * Time.deltaTime * direction);
        }

        [PunRPC]
        public void Initialize(Vector2 direction, Vector2 knockBackDirection, float damage, float knockBack)
        {
            this.direction = direction;
            KnockBackSignedDirection = new Vector2(
                direction.x > 0 ? knockBackDirection.x : -knockBackDirection.x,
                knockBackDirection.y
            );
            Damage = damage;
            KnockBackForce = knockBack;
        }

        private void Destroy()
        {
            PhotonNetwork.Destroy(gameObject);
        }

        private IEnumerator DestroyBullet()
        {
            yield return new WaitForSeconds(destroyTime);
            Destroy();
        }

        protected override void OnStrike(Vector2 position)
        {
            base.OnStrike(position);
            Destroy();
        }
    }
}
