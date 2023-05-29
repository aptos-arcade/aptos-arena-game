using Player;
using UnityEngine;

namespace Commands
{
    public class JumpCommand : Command
    {

        private readonly PlayerScript player;

        public JumpCommand(PlayerScript player, KeyCode key) : base(key)
        {
            this.player = player;
        }

        public override void GetKeyDown()
        {
            player.PlayerActions.Jump();
        }
    }
}
