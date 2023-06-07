using Gameplay;
using Player;
using UnityEngine;

namespace Commands
{
    public class ShootCommand : Command
    {

        private readonly PlayerScript player;

        public ShootCommand(PlayerScript player, KeyCode key) : base(key)
        {
            this.player = player;
        }

        public override void GetKeyDown()
        {
            if (player.PlayerState.RangedEnergy >= player.PlayerStats.RangedAttackEnergyCost)
            {
                player.PlayerActions.TrySwapWeapon(Global.Weapons.Gun);
                player.PlayerActions.Attack();
            }
            else
            {
                MatchManager.Instance.NoEnergy(Global.Weapons.Gun);
            }
            
        }
    }
}
