using System.Collections;
using Photon.Pun;
using Player;
using UnityEngine;

namespace Weapons
{
    public class Projectile: Striker
    {
        [SerializeField] private float speed;
        
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
        public void Initialize(Vector2 dir)
        {
            direction = dir;
            KnockBackSignedDirection = new Vector2(
                direction.x > 0 ? strikerData.KnockBackDirection.x : -strikerData.KnockBackDirection.x,
                strikerData.KnockBackDirection.y
            );
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

        protected override void OnPlayerStrike(Vector2 position, PlayerScript player)
        {
            base.OnPlayerStrike(position, player);
            Destroy();
        }
    }
}
