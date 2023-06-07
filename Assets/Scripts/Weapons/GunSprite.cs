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
        
        private SpriteRenderer spriteRenderer;

        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (photonView.Owner == null)
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