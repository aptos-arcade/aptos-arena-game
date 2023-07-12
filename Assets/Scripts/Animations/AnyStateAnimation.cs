namespace Animations
{
    public enum Rig { Body, Legs }

    public class AnyStateAnimation
    {
        public Rig AnimationRig { get; }

        public string[] HigherPriority { get; }

        public string Name { get; }
        
        public bool Active { get; set; }
        
        public bool HoldOnEnd { get; }

        public AnyStateAnimation(Rig rig, bool holdOnEnd, string name, params string[] higherPriority)
        {
            AnimationRig = rig;
            Name = name;
            HigherPriority = higherPriority;
            HoldOnEnd = holdOnEnd;
        }


    }
}