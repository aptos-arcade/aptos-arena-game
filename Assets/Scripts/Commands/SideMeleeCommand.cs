using Gameplay;
using Global;
using Player;
using UnityEngine;
using Weapons;

namespace Commands
{
    public class SideMeleeCommand : Command
    {
        private readonly PlayerScript player;

        private readonly int xScale;
        
        public SideMeleeCommand(PlayerScript player, KeyCode key, int xScale) : base(key)
        {
            this.player = player;
            this.xScale = xScale;
        }

        public override void GetKeyDown()
        {
            if (player.PlayerState.MeleeEnergy >= player.PlayerStats.SideMeleeAttack.Energy)
            {
                if (player.PlayerComponents.Animator.CurrentAnimationBody == "Body_Attack") return;
                player.PlayerReferences.Sword.strikerData = player.PlayerStats.SideMeleeAttack;

                player.gameObject.transform.localScale = new Vector3(xScale, 1, 1);
                player.PlayerReferences.PlayerCanvas.transform.localScale = new Vector3(xScale, 1, 1);
                
                player.PlayerComponents.Animator.SetAttackDirection(Directions.Side);
                
                player.PlayerActions.TrySwapWeapon(Global.Weapons.Sword);
                player.PlayerActions.Attack();
            }
            else
            {
                MatchManager.Instance.NoEnergy(EnergyUIController.EnergyType.Sword);
            }
            
        }
    }
}
