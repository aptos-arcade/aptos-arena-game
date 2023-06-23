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
            player.PlayerActions.TryShield();
        }
    }
}
