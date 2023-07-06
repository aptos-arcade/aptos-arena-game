using Photon;
using Photon.Pun;
using ScriptableObjects;
using UnityEngine;

namespace Weapons
{
    public class SwordSprite : MonoBehaviour
    {
        [SerializeField] private PhotonView photonView;
        [SerializeField] private SwordImages swordImages;
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        private void Start()
        {
            UpdateSwordSprite();
        }

        public void UpdateSwordSprite()
        {
            if (photonView.Owner == null)
            {
                spriteRenderer.sprite = PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(PlayerPropertyKeys.SwordKey,
                    out var swordIndex) ? swordImages.GetSwordImage((int)swordIndex) : swordImages.GetSwordImage(0);
            }
            else
            {
                spriteRenderer.sprite = photonView.Owner.CustomProperties.TryGetValue(PlayerPropertyKeys.SwordKey,
                    out var swordIndex) ? swordImages.GetSwordImage((int)swordIndex) : swordImages.GetSwordImage(0);
            }
        }
    }
}