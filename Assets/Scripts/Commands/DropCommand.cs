using Player;
using UnityEngine;

namespace Commands
{
    public class DropCommand : Command
    {

        private readonly PlayerScript player;

        public DropCommand(PlayerScript player, KeyCode key) : base(key)
        {
            this.player = player;
        }

        public override void GetKeyDown()
        {
            if (player.PlayerUtilities.IsOnPlatform && !player.PlayerUtilities.IsOnGround)
            {
                player.PlayerActions.Drop();
            }
        }
    }
}
