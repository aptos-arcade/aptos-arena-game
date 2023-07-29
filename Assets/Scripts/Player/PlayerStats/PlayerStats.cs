using UnityEngine;
using Weapons;

namespace Player.PlayerStats
{
    [CreateAssetMenu]
    public class PlayerStats: ScriptableObject
    {
        [Header("Movement")]
        
        [SerializeField] private float jumpForce;
        public float JumpForce => jumpForce;

        public float DoubleJumpForce => jumpForce * 0.9f;
        
        [SerializeField] private float acceleration;
        public float Acceleration => acceleration;
        
        [SerializeField] private float deceleration;
        public float Deceleration => deceleration;
        
        [SerializeField] private float velocityPower;
        public float VelocityPower => velocityPower;
        
        [SerializeField] private float gravityScale;
        public float GravityScale => gravityScale;

        [SerializeField] private float speed;
        public float Speed => speed;
        
        public float DodgeVelocity => speed * 1.25f;

        public float DashVelocity => speed * 2.25f;

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

        [SerializeField] private StrikerData rangedAttack;
        public StrikerData RangedAttack => rangedAttack;

        [Header("Shield")] 
        
        [SerializeField] private float shieldDuration;
        public float ShieldDuration => shieldDuration;
        
        [SerializeField] private float shieldStunDuration;
        public float ShieldStunDuration => shieldStunDuration;

        [SerializeField] private float shieldEnergyRegenTime;
        public float ShieldEnergyRegenTime => shieldEnergyRegenTime;


        [Header("Energy Regen")]
        
        [SerializeField] private float meleeEnergyRegenTime;
        public float MeleeEnergyRegenTime => meleeEnergyRegenTime;

        [SerializeField] private float rangedEnergyRegenTime;
        public float RangedEnergyRegenTime => rangedEnergyRegenTime;
        
        
    }
}