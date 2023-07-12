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
            if (player.PlayerComponents.Animator.CurrentAnimationBody == "Body_Attack") return;
            if (player.PlayerUtilities.IsGrounded)
            {
                if (player.PlayerState.MeleeEnergy >= player.PlayerStats.JabMeleeAttack.Energy)
                {
                    player.PlayerReferences.Sword.strikerData = player.PlayerStats.JabMeleeAttack;
                    player.PlayerComponents.Animator.SetAttackDirection(Directions.Neutral);
                }
                else
                {
                    MatchManager.Instance.NoEnergy(EnergyUIController.EnergyType.Sword);
                    return;
                }
            }
            else
            {
                if (player.PlayerState.MeleeEnergy >= player.PlayerStats.DownMeleeAttack.Energy)
                {
                    player.PlayerReferences.Sword.strikerData = player.PlayerStats.DownMeleeAttack;
                    player.PlayerComponents.Animator.SetAttackDirection(Directions.Down);
                }
                else
                {
                    MatchManager.Instance.NoEnergy(EnergyUIController.EnergyType.Sword);
                    return;
                }
            }
            player.PlayerActions.TrySwapWeapon(Global.Weapons.Sword);
            player.PlayerActions.Attack();
        }
    }
}
