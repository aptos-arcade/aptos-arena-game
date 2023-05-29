using System.Collections.Generic;
using Animations;
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


        [SerializeField] private PolygonCollider2D bodyCollider;
        public PolygonCollider2D BodyCollider { get => bodyCollider; set => bodyCollider = value; }


        [SerializeField] private AnyStateAnimator animator;
        public AnyStateAnimator Animator => animator;


        [SerializeField] private LayerMask ground;
        public LayerMask Ground => ground;


        [SerializeField] private LayerMask platform;
        public LayerMask Platform => platform;

        [SerializeField] private PhotonView photonView;
        public PhotonView PhotonView { get => photonView; set => photonView = value; }


        [SerializeField] private PlayerCamera playerCamera;
        public PlayerCamera PlayerCamera => playerCamera;
        
        [SerializeField] private AudioSource jumpAudioSource;
        public AudioSource JumpAudioSource => jumpAudioSource;
        
        [SerializeField] private AudioSource hitAudioSource;
        public AudioSource HitAudioSource => hitAudioSource;
        
        [SerializeField] private AudioSource runAudioSource;
        public AudioSource RunAudioSource => runAudioSource;

        public List<SpriteRenderer> PlayerSprites { get; set; } = new();
    }
}
