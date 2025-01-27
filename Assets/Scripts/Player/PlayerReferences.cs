using TMPro;
using UnityEngine;
using Weapons;

namespace Player
{
    [System.Serializable]
    public class PlayerReferences
    {

        [Header("Text References")]

        [SerializeField] private TMP_Text nameTag;
        public TMP_Text NameTag { get => nameTag; set => nameTag = value; }
        
        [SerializeField] private TMP_Text collectionTag;
        public TMP_Text CollectionTag { get => collectionTag; set => collectionTag = value; }

        [SerializeField] private TMP_Text damageDisplay;
        public TMP_Text DamageDisplay { get => damageDisplay; set => damageDisplay = value; }
        
        [Header("Prefabs")]

        [SerializeField] private GameObject explosionPrefab;
        public GameObject ExplosionPrefab => explosionPrefab;

        [Header("Game Objects")]

        [SerializeField] private GameObject playerCanvas;
        public GameObject PlayerCanvas { get => playerCanvas; set => playerCanvas = value; }

        [SerializeField] private Weapon[] weaponObjects;
        public Weapon[] WeaponObjects => weaponObjects;

        public Sword Sword => weaponObjects[(int)Global.Weapons.Sword].GetComponent<Sword>();
        
        public Gun Gun => weaponObjects[(int)Global.Weapons.Gun].GetComponent<Gun>();

        [Header("Transforms")]

        [SerializeField] private Transform playerLives;
        public Transform PlayerLives { get => playerLives; set => playerLives = value; }
        
        [SerializeField] private PlayerShield playerShield;
        public PlayerShield PlayerShield { get => playerShield; set => playerShield = value; }

        [Header("Audio Clips")]
        
        [SerializeField] private AudioClip shootAudioClip;
        public AudioClip ShootAudioClip => shootAudioClip;
        
        [SerializeField] private AudioClip sideMeleeAudioClip;
        public AudioClip SideMeleeAudioClip => sideMeleeAudioClip;
        
        [SerializeField] private AudioClip jabMeleeAudioClip;
        public AudioClip JabMeleeAudioClip => jabMeleeAudioClip;
        
        [SerializeField] private AudioClip upMeleeAudioClip;
        public AudioClip UpMeleeAudioClip => upMeleeAudioClip;
        
        [SerializeField] private AudioClip jumpAudioClip;
        public AudioClip JumpAudioClip => jumpAudioClip;
        
        [SerializeField] private AudioClip damageAudioClip;
        public AudioClip DamageAudioClip => damageAudioClip;
        
        [SerializeField] private AudioClip dashAudioClip;
        public AudioClip DashAudioClip => dashAudioClip;
        
        [SerializeField] private AudioClip dodgeAudioClip;
        public AudioClip DodgeAudioClip => dodgeAudioClip;
        
        
        [Header("Character Specific")]
        
        [SerializeField] private GameObject portal;
        public GameObject Portal => portal;
        
        [SerializeField] private GameObject playerMesh;
        public GameObject PlayerMesh => playerMesh;
        
        
    }
}
