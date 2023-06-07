using Gameplay;
using Global;
using Player;
using UnityEngine;

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
            if (player.PlayerState.MeleeEnergy >= player.PlayerStats.UpMeleeAttackEnergyCost)
            {
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
