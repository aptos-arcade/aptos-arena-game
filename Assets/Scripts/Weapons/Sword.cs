using UnityEngine;

namespace Weapons
{
    public class Sword : Striker
    {
        [SerializeField] private Transform ownerTransform;
        private void Update()
        {
            KnockBackSignedDirection = new Vector2(
                ownerTransform.localScale.x * KnockBackDirection.x,
                KnockBackDirection.y
            );
        }
    }
}