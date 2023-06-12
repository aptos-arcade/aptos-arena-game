using Photon.Pun;
using Player;
using UnityEngine;

namespace Weapons
{
    public class Gun : MonoBehaviour
    {
        
        [SerializeField] private Projectile projectilePrefab;
        [SerializeField] private Transform barrelTransform;
        
        public void Shoot(PlayerScript player)
        {
            var projectile = PhotonNetwork.Instantiate(projectilePrefab.name,
                barrelTransform.position, Quaternion.identity);
            var direction = new Vector2(player.transform.localScale.x, 0);
            projectile.GetPhotonView().RPC("Initialize", RpcTarget.All, direction,
                player.PlayerStats.RangedAttack.KnockBackDirection, player.PlayerStats.RangedAttack.Damage,
                player.PlayerStats.RangedAttack.KnockBack);
        }
    }
}