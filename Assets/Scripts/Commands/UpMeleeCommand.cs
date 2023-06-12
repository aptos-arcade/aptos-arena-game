using Gameplay;
using Global;
using Player;
using UnityEngine;
using Weapons;

namespace Commands
{
    public class UpMeleeCommand : Command
    {
        private readonly PlayerScript player;
        

        public UpMeleeCommand(PlayerScript player, KeyCode key) : base(key)
        {
            this.player = player;
        }

        public override void GetKeyDown()
        {
            if (player.PlayerState.MeleeEnergy >= player.PlayerStats.UpMeleeAttack.Energy)
            {
                player.PlayerReferences.Sword.KnockBackDirection = player.PlayerStats.UpMeleeAttack.KnockBackDirection;
                player.PlayerReferences.Sword.KnockBackForce = player.PlayerStats.UpMeleeAttack.KnockBack;
                player.PlayerReferences.Sword.Damage = player.PlayerStats.UpMeleeAttack.Damage;
                
                player.PlayerComponents.Animator.SetAttackDirection(Directions.Up);
                
                player.PlayerActions.TrySwapWeapon(Global.Weapons.Sword);
                player.PlayerActions.Attack();
            }
            else
            {
                MatchManager.Instance.NoEnergy(Global.Weapons.Sword);
            }
        }
    }
}
