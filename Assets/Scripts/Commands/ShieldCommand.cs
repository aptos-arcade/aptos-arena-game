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

        public override void GetKey()
        {
            if (Input.GetAxisRaw("Horizontal") == 0)
            {
                if (player.PlayerState.ShieldEnergy < 0.1 || !player.PlayerUtilities.IsGrounded) return;
                player.PlayerActions.TryShield();
            }
            else
            {
                if (player.PlayerUtilities.IsDodging || !player.PlayerState.CanDodge) return;
                player.PlayerState.Direction = Input.GetAxisRaw("Horizontal") * Vector2.right;
                player.PlayerActions.TryDodge();
            }
        }

        public override void GetKeyUp()
        {
            player.PlayerComponents.Animator.OnAnimationDone("Body_Shield");
            player.PlayerComponents.Animator.OnAnimationDone("Legs_Shield");
        }
    }
}
