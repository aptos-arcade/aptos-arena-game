using UnityEngine;
using Weapons;

namespace Player
{
    [System.Serializable]
    public class PlayerStats
    {
        [Header("Movement")]
        
        [SerializeField] private float jumpForce;
        public float JumpForce => jumpForce;

        [SerializeField] private float doubleJumpForce;
        public float DoubleJumpForce => doubleJumpForce;

        [SerializeField] private float dodgeVelocity;
        public float DodgeVelocity => dodgeVelocity;

        [SerializeField] private float dashVelocity;
        public float DashVelocity => dashVelocity;

        [SerializeField] private float speed;
        public float Speed => speed;

        [SerializeField] private float acceleration;
        public float Acceleration => acceleration;

        [SerializeField] private float deceleration;
        public float Deceleration => deceleration;

        [SerializeField] private float velPower;
        public float VelPower => velPower;
        
        [SerializeField] private float fastFallForce;
        public float FastFallForce => fastFallForce;

        [Header("Attacks")]
        
        [SerializeField] private StrikerData sideMeleeAttack;
        public StrikerData SideMeleeAttack => sideMeleeAttack;
        
        [SerializeField] private StrikerData jabMeleeAttack;
        public StrikerData JabMeleeAttack => jabMeleeAttack;
        
        [SerializeField] private StrikerData upMeleeAttack;
        public StrikerData UpMeleeAttack => upMeleeAttack;
        
        [SerializeField] private StrikerData downMeleeAttack;
        public StrikerData DownMeleeAttack => downMeleeAttack;


        [Header("Energy Regen")]
        
        [SerializeField] private float meleeEnergyRegenTime;
        public float MeleeEnergyRegenTime => meleeEnergyRegenTime;

        [SerializeField] private float rangedEnergyRegenTime;
        public float RangedEnergyRegenTime => rangedEnergyRegenTime;
        
        [SerializeField] private float shieldEnergyRegenTime;
        public float ShieldEnergyRegenTime => shieldEnergyRegenTime;
    }
}