using Gameplay;
using Global;
using Player;
using UnityEngine;

namespace Commands
{
    public class JabMeleeCommand : Command
    {
        private readonly PlayerScript player;

        public JabMeleeCommand(PlayerScript player, KeyCode key) : base(key)
        {
            this.player = player;
        }

        public override void GetKeyDown()
        {
            if (player.PlayerState.MeleeEnergy >= player.PlayerStats.JabMeleeAttackEnergyCost)
            {
                player.PlayerComponents.Animator.SetAttackDirection(Directions.Neutral);
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
