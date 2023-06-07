using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    [System.Serializable]
    public class PlayerStats
    {
        [SerializeField] private float jumpForce;
        public float JumpForce => jumpForce;

        [SerializeField] private float doubleJumpForce;
        public float DoubleJumpForce => doubleJumpForce;

        [SerializeField] private float speed;
        public float Speed => speed;

        [SerializeField] private float acceleration;
        public float Acceleration => acceleration;

        [SerializeField] private float deceleration;
        public float Deceleration => deceleration;

        [SerializeField] private float velPower;
        public float VelPower => velPower;

        [SerializeField] private float knockBackPower;
        public float KnockBackPower => knockBackPower;

        [SerializeField] private float sideMeleeAttackEnergyCost;
        public float SideMeleeAttackEnergyCost => sideMeleeAttackEnergyCost;
        
        [SerializeField] private float jabMeleeAttackEnergyCost;
        public float JabMeleeAttackEnergyCost => jabMeleeAttackEnergyCost;
        
        [SerializeField] private float upMeleeAttackEnergyCost;
        public float UpMeleeAttackEnergyCost => upMeleeAttackEnergyCost;
        
        [SerializeField] private float meleeEnergyRegenTime;
        public float MeleeEnergyRegenTime => meleeEnergyRegenTime;

        [SerializeField] private float rangedAttackEnergyCost;
        public float RangedAttackEnergyCost => rangedAttackEnergyCost;
        
        [SerializeField] private float rangedEnergyRegenTime;
        public float RangedEnergyRegenTime => rangedEnergyRegenTime;
    }
}