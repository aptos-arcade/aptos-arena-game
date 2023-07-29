using System.Collections.Generic;
using Animations;
using Com.LuisPedroFonseca.ProCamera2D;
using Gameplay;
using Photon.Pun;
using UnityEngine;

namespace Player
{
    [System.Serializable]
    public class PlayerComponent
    {
        [SerializeField] private Rigidbody2D rigidBody;
        public Rigidbody2D RigidBody => rigidBody;

        [SerializeField] private BoxCollider2D footCollider;
        public Collider2D FootCollider => footCollider;

        [SerializeField] private CapsuleCollider2D bodyCollider;
        public CapsuleCollider2D BodyCollider { get => bodyCollider; set => bodyCollider = value; }

        [SerializeField] private AnyStateAnimator animator;
        public AnyStateAnimator Animator => animator;

        [SerializeField] private LayerMask ground;
        public LayerMask Ground => ground;

        [SerializeField] private LayerMask platform;
        public LayerMask Platform => platform;

        [SerializeField] private PhotonView photonView;
        public PhotonView PhotonView { get => photonView; set => photonView = value; }

        [SerializeField] private AudioSource runAudioSource;
        public AudioSource RunAudioSource => runAudioSource;
        
        [SerializeField] private AudioSource oneShotAudioSource;
        public AudioSource OneShotAudioSource => oneShotAudioSource;
        
        public ProCamera2DShake ProCamera2DShake => MatchManager.Instance.SceneCamera.GetComponent<ProCamera2DShake>();

        public List<SpriteRenderer> PlayerSprites { get; set; } = new();
        
        public List<Color> PlayerSpriteColors { get; set; } = new();
    }
}
