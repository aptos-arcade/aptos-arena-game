using Characters;
using UnityEngine;

namespace Player
{
    public class PlayerState
    {
        public Vector2 Direction { get; set; }
        
        public bool CanDoubleJump { get; set; } = true;
        
        public bool CanMove { get; set; }

        public bool IsDead { get; set; } = true;

        public bool IsStunned { get; set; }
        
        public Global.Weapons Weapon { get; set; }
        
        public float DamageMultiplier { get; set; } = 1;

        public string PlayerName { get; set; }

        public int Lives { get; set; } = 3;
        
        public CharactersEnum Character { get; set; }
        
        public float RangedEnergy { get; set; } = 1;
        
        public float MeleeEnergy { get; set; } = 1;
    }
}