using Player;
using UnityEngine;
using Weapons;

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
            if(player.PlayerState.RangedEnergy <= player.PlayerStats.RangedAttackEnergyCost) return;
            player.PlayerActions.TrySwapWeapon(Global.Weapons.Gun);
            player.PlayerActions.Attack();
        }
    }
}
