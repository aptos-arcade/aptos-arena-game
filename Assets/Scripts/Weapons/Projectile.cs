using System.Collections;
using Photon.Pun;
using Player;
using UnityEngine;

namespace Weapons
{
    public class Projectile: Striker
    {
        [SerializeReference]
        private float speed;
        
        [SerializeField]
        private float destroyTime;

        private Vector2 _direction;
        
        private void Start()
        {
            StartCoroutine(DestroyBullet());
        }


        // Update is called once per frame
        private void Update()
        {
            transform.Translate(speed * Time.deltaTime * _direction);
        }

        [PunRPC]
        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
            KnockBackSignedDirection = new Vector2(
                direction.x > 0 ? KnockBackDirection.x : -KnockBackDirection.x,
                KnockBackDirection.y
            );
        }

        [PunRPC]
        public void Destroy()
        {
            PhotonNetwork.Destroy(gameObject);
        }

        private IEnumerator DestroyBullet()
        {
            yield return new WaitForSeconds(destroyTime);
            photonView.RPC("Destroy", RpcTarget.AllBuffered);
        }
        
        protected override void OnStrike()
        {
            photonView.RPC("Destroy", RpcTarget.AllBuffered);
        }
    }
}
