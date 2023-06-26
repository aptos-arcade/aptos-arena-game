using Player;
using UnityEngine;

namespace Commands
{
    public class DashCommand : Command
    {

        private readonly PlayerScript player;

        private float lastPressTime;

        public DashCommand(PlayerScript player, KeyCode key) : base(key)
        {
            this.player = player;
        }

        public override void GetKeyDown()
        {
            if (player.PlayerState.IsStunned || player.PlayerState.IsDodging || player.PlayerState.IsDashing ||
                !player.PlayerUtilities.IsGrounded) return;
            var elapsedTime = Time.time - lastPressTime;
            lastPressTime = Time.time;
            if (elapsedTime > 0.2f) return;
            player.PlayerActions.TryDash();
        }
    }
}
