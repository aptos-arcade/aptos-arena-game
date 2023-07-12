using System;
using System.Collections.Generic;
using System.Linq;
using Global;
using Photon.Pun;
using UnityEngine;

namespace Animations
{
    public delegate void AnimationTriggerEvent(string animation);

    public class AnyStateAnimator : MonoBehaviourPun
    {
        private Animator animator;

        private readonly Dictionary<string, AnyStateAnimation> animations = new();

        public AnimationTriggerEvent AnimationTriggerEvent { get; set; }

        private string currentAnimationLegs = string.Empty;

        private string currentAnimationBody = string.Empty;
        public string CurrentAnimationBody => currentAnimationBody;
        public string CurrentAnimationLegs => currentAnimationLegs;
    
        private static readonly int Weapon = Animator.StringToHash("Weapon");
        private static readonly int AttackDirection = Animator.StringToHash("AttackDirection");

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (!photonView.IsMine) return;
            Animate();
        }

        public void OnDeath()
        {
            foreach (var key in animations.Keys)
            {
                animator.SetBool(key, false);
            }
        }

        public void AddAnimations(params AnyStateAnimation[] newAnimations)
        {
            foreach (var t in newAnimations)
            {
                animations.Add(t.Name, t);
            }
        }

        public void TryPlayAnimation(string newAnimation)
        {
            var rig = animations[newAnimation].AnimationRig;
            switch (rig)
            {
                case Rig.Body:
                    PlayAnimation(ref currentAnimationBody);
                    break;
                case Rig.Legs:
                    PlayAnimation(ref currentAnimationLegs);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            void PlayAnimation(ref string currentAnimation)
            {
                if(currentAnimation == "")
                {
                    animations[newAnimation].Active = true;
                    currentAnimation = newAnimation;
                }
                else if(
                    (currentAnimation != newAnimation 
                     && !animations[newAnimation].HigherPriority.Contains(currentAnimation))
                    || !animations[currentAnimation].Active
                    || (!animations[currentAnimation].HoldOnEnd
                        && animator.GetCurrentAnimatorStateInfo((int)rig).IsName(currentAnimation)
                        && animator.GetCurrentAnimatorStateInfo((int)rig).normalizedTime >= 1)
                )
                {
                    animations[currentAnimation].Active = false;
                    animations[newAnimation].Active = true;
                    currentAnimation = newAnimation;
                }
            }
        }

        public void SetWeapon(float weapon)
        {
            animator.SetFloat(Weapon, weapon);
        }

        public void SetAttackDirection(Directions direction)
        {
            animator.SetFloat(AttackDirection, (float)direction);
        }

        private void Animate()
        {
            foreach (var key in animations.Keys)
            {
                animator.SetBool(key, animations[key].Active);
            }
        }

        public void OnAnimationDone(string doneAnimation)
        {
            if (!photonView.IsMine) return;
            animations[doneAnimation].Active = false;
        }

        public void OnAnimationTrigger(string startAnimation)
        {
            if (!photonView.IsMine) return;
            AnimationTriggerEvent?.Invoke(startAnimation);
        }
    }
}