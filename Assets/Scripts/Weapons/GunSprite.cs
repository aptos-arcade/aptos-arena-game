using Photon;
using Photon.Pun;
using ScriptableObjects;
using UnityEngine;

namespace Weapons
{
    public class GunSprite : MonoBehaviour
    {
        
        [SerializeField] private PhotonView photonView;
        [SerializeField] private GunImages gunImages; 
        [SerializeField] private SpriteRenderer spriteRenderer;

        private void Start()
        {
            UpdateGunSprite();
        }

        public void UpdateGunSprite()
        {
            if (photonView == null || photonView.Owner == null)
            {
                spriteRenderer.sprite = PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(PlayerPropertyKeys.GunKey,
                    out var swordIndex) ? gunImages.GetGunImage((int)swordIndex) : gunImages.GetGunImage(0);
            }
            else
            {
                spriteRenderer.sprite = photonView.Owner.CustomProperties.TryGetValue(PlayerPropertyKeys.GunKey,
                    out var swordIndex) ? gunImages.GetGunImage((int)swordIndex) : gunImages.GetGunImage(0);
            }
        }
    }
}