using System.Collections;
using Player;
using UnityEngine;

namespace Weapons
{
    public class Sword : Striker
    {
        protected override void OnPlayerStrike(Vector2 position, PlayerScript player)
        {
            base.OnPlayerStrike(position, player);
            StartCoroutine(DisableCoroutine(player));
        }
        
        protected override void OnShieldStrike(Vector2 position, PlayerShield shield)
        {
            base.OnShieldStrike(position, shield);
            Owner.PlayerUtilities.ShieldHit(shield);
        }

        private void Update()
        {
            if (!photonView.IsMine) return;
            KnockBackSignedDirection = new Vector2(
                Owner.transform.localScale.x * strikerData.KnockBackDirection.x,
                strikerData.KnockBackDirection.y
            );
        }

        private static IEnumerator DisableCoroutine(PlayerScript player)
        {
            player.PlayerComponents.BodyCollider.enabled = false;
            yield return new WaitForSeconds(0.25f);
            player.PlayerComponents.BodyCollider.enabled = true;
        }
    }
}