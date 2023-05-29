using Player;
using UnityEngine;

namespace Commands
{
    public class MeleeCommand : Command
    {
        private readonly PlayerScript player;

        public MeleeCommand(PlayerScript player, KeyCode key) : base(key)
        {
            this.player = player;
        }

        public override void GetKeyDown()
        {
            if(player.PlayerState.MeleeEnergy <= player.PlayerStats.MeleeAttackEnergyCost) return;
            player.PlayerActions.TrySwapWeapon(Global.Weapons.Sword);
            player.PlayerActions.Attack();
        }
    }
}
