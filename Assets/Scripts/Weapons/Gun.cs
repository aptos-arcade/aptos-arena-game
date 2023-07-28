using Photon.Pun;
using UnityEngine;

namespace Weapons
{
    public class Gun : MonoBehaviour
    {
        [SerializeField] private Projectile projectilePrefab;
        public Projectile ProjectilePrefab => projectilePrefab;
        
        [SerializeField] private Transform barrelTransform;
        
        public void Shoot()
        {
            PhotonNetwork.Instantiate(projectilePrefab.name, barrelTransform.position, Quaternion.identity);
        }
    }
}