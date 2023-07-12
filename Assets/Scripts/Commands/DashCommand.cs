using System;
using Player;
using UnityEngine;

namespace Commands
{
    public class DashCommand : Command
    {

        private readonly PlayerScript player;

        private float lastPressTime;

        private float direction;

        public DashCommand(PlayerScript player, KeyCode key, float direction) : base(key)
        {
            this.player = player;
            this.direction = direction;
        }

        public override void GetKeyDown()
        {
            if (player.PlayerUtilities.IsDodging) return;
            if (player.PlayerUtilities.IsDashing && Math.Abs(player.transform.localScale.x - direction) > 0.01)
            {
                player.PlayerComponents.Animator.OnAnimationDone("Body_Dash");
                player.PlayerComponents.Animator.OnAnimationDone("Legs_Dash");
            }
            var elapsedTime = Time.time - lastPressTime;
            lastPressTime = Time.time;
            if (elapsedTime > 0.2f) return;
            player.PlayerActions.TryDash();
        }
    }
}
