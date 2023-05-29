using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

namespace Animations
{
    public delegate void AnimationTriggerEvent(string animation);

    public class AnyStateAnimator : MonoBehaviourPun
    {
        private Animator _animator;

        private readonly Dictionary<string, AnyStateAnimation> _animations = new();

        public AnimationTriggerEvent AnimationTriggerEvent { get; set; }

        private string _currentAnimationLegs = string.Empty;

        private string _currentAnimationBody = string.Empty;
    
        private static readonly int Weapon1 = Animator.StringToHash("Weapon");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (!photonView.IsMine) return;
            Animate();
        }

        public void AddAnimations(params AnyStateAnimation[] newAnimations)
        {
            foreach (var t in newAnimations)
            {
                _animations.Add(t.Name, t);
            }
        }

        public void TryPlayAnimation(string newAnimation)
        {
            switch (_animations[newAnimation].AnimationRig)
            {
                case Rig.Body:
                    PlayAnimation(ref _currentAnimationBody);
                    break;
                case Rig.Legs:
                    PlayAnimation(ref _currentAnimationLegs);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            void PlayAnimation(ref string currentAnimation)
            {
                if(currentAnimation == "")
                {
                    _animations[newAnimation].Active = true;
                    currentAnimation = newAnimation;
                }
                else if(
                    (currentAnimation != newAnimation
                     && !_animations[newAnimation].HigherPriority.Contains(currentAnimation))
                    || !_animations[currentAnimation].Active
                )
                {
                    _animations[currentAnimation].Active = false;
                    _animations[newAnimation].Active = true;
                    currentAnimation = newAnimation;
                }
            }
        }

        public void SetWeapon(float weapon)
        {
            _animator.SetFloat(Weapon1, weapon);
        }

        private void Animate()
        {
            foreach (var key in _animations.Keys)
            {
                _animator.SetBool(key, _animations[key].Active);
            }
        }

        public void OnAnimationDone(string doneAnimation)
        {
            _animations[doneAnimation].Active = false;
        }

        public void OnAnimationTrigger(string startAnimation)
        {
            AnimationTriggerEvent?.Invoke(startAnimation);
        }
    }
}