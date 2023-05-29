using UnityEngine;

namespace Commands
{
    public abstract class Command
    {
        public KeyCode Key { get; }

        protected Command(KeyCode key)
        {
            Key = key;
        }

        public virtual void GetKeyDown() {}

        public virtual void GetKeyUp() {}

        public virtual void GetKey() {}
    }
}
