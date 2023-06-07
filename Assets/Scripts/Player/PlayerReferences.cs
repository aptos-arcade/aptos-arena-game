using TMPro;
using UnityEngine;

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
        
        [SerializeField] private GameObject projectilePrefab;
        public GameObject ProjectilePrefab => projectilePrefab;
        
        [SerializeField] private GameObject portal;
        public GameObject Portal => portal;
        
        [Header("Game Objects")]
        
        [SerializeField] private GameObject playerMesh;
        public GameObject PlayerMesh => playerMesh;
        
        [SerializeField] private GameObject playerCanvas;
        public GameObject PlayerCanvas { get => playerCanvas; set => playerCanvas = value; }

        [SerializeField] private GameObject playerCamera;
        public GameObject PlayerCamera { get => playerCamera; set => playerCamera = value; }
        
        [SerializeField] private GameObject[] weaponObjects;
        public GameObject[] WeaponObjects => weaponObjects;
        
        [Header("Transforms")]

        [SerializeField] private Transform playerLives;
        public Transform PlayerLives { get => playerLives; set => playerLives = value; }
        
        [SerializeField] private Transform gunBarrel;
        public Transform GunBarrel => gunBarrel;
        
        [Header("Audio Clips")]
        
        [SerializeField] private AudioClip shootAudioClip;
        public AudioClip ShootAudioClip => shootAudioClip;
        
        [SerializeField] private AudioClip sideMeleeAudioClip;
        public AudioClip SideMeleeAudioClip => sideMeleeAudioClip;
        
        [SerializeField] private AudioClip jabMeleeAudioClip;
        public AudioClip JabMeleeAudioClip => jabMeleeAudioClip;
        
        [SerializeField] private AudioClip upMeleeAudioClip;
        public AudioClip UpMeleeAudioClip => upMeleeAudioClip;
    }
}
