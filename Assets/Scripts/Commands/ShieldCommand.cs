using Player;
using UnityEngine;

namespace Commands
{
    public class ShieldCommand : Command
    {
        private readonly PlayerScript player;
        
        public ShieldCommand(PlayerScript player, KeyCode key) : base(key)
        {
            this.player = player;
        }

        public override void GetKeyDown()
        {
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                if(player.PlayerState.CanDodge) player.PlayerActions.TryDodge();
            }
            else
            {
                player.PlayerActions.TryShield();
            }
        }
        
        public override void GetKeyUp()
        {
            player.PlayerComponents.Animator.OnAnimationDone("Body_Shield");
            player.PlayerComponents.Animator.OnAnimationDone("Legs_Shield");
        }
    }
}
