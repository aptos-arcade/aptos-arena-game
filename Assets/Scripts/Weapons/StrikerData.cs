using System;
using UnityEngine;

namespace Weapons
{
    [Serializable]
    public class StrikerData
    {
        [SerializeField] private float energy;
        public float Energy => energy;
        
        [SerializeField] private float knockBack;
        public float KnockBack => knockBack;

        [SerializeField] private Vector2 knockBackDirection;
        public Vector2 KnockBackDirection => knockBackDirection;
        
        [SerializeField] private float damage;
        public float Damage => damage;

        [SerializeField] private float stunTime;
        public float StunTime => stunTime;
    }
}