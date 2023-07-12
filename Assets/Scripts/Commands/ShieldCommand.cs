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
            if (!player.PlayerUtilities.IsDodging && Input.GetAxisRaw("Horizontal") != 0 && player.PlayerState.CanDodge)
            {
                player.PlayerState.Direction = Input.GetAxisRaw("Horizontal") * Vector2.right;
                player.PlayerActions.TryDodge();
            }
            else
            {
                player.PlayerActions.TryShield();
            }
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
