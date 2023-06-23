using System;
using Animations;
using Characters;
using Gameplay;
using Photon.Pun;
using UnityEngine;
using Weapons;
using static Photon.PlayerPropertyKeys;

namespace Player
{
    public class PlayerScript : MonoBehaviourPun
    {

        [SerializeField] private PlayerStats playerStats;
        public PlayerStats PlayerStats => playerStats;

        [SerializeField] private PlayerComponent playerComponent;
        public PlayerComponent PlayerComponents => playerComponent;

        [SerializeField] private PlayerReferences playerReferences;
        public PlayerReferences PlayerReferences => playerReferences;

        public PlayerActions PlayerActions { get; private set; }

        public PlayerUtilities PlayerUtilities { get; private set; }
        
        public PlayerState PlayerState { get; } = new();

        // Start is called before the first frame update
        private void Awake()
        {
            if (photonView.IsMine)
            {
                if (!MatchManager.Instance) return;
                MatchManager.Instance.Player = this;

                var playerPosition = transform.position;
                playerReferences.PlayerCamera.transform.position = new Vector3(playerPosition.x, playerPosition.y, playerReferences.PlayerCamera.transform.position.z);
                playerReferences.PlayerCamera.SetActive(true);
                playerReferences.PlayerCamera.transform.SetParent(null, true);
                
                playerReferences.NameTag.text = PhotonNetwork.NickName;
                playerReferences.NameTag.color = new Color(0.6588235f, 0.8078431f, 1f);
            }
            else
            {
                var tagColor =
                    (CharactersEnum)photonView.Owner.CustomProperties[TeamKey] ==
                    (CharactersEnum)PhotonNetwork.LocalPlayer.CustomProperties[TeamKey]
                        ? new Color(0.6588235f, 0.8078431f, 1f)
                        : Color.red;
                playerReferences.NameTag.color = tagColor;
                playerReferences.CollectionTag.color = tagColor;
                playerReferences.NameTag.text = photonView.Owner.NickName;
            }

            playerReferences.CollectionTag.text = Characters.Characters
                .AvailableCharacters[(CharactersEnum)photonView.Owner.CustomProperties[CharacterKey]].DisplayName;
        }


        // Start is called before the first frame update
        private void Start()
        {
            PlayerActions = new PlayerActions(this);
            PlayerUtilities = new PlayerUtilities(this);

            PlayerState.PlayerName = PhotonNetwork.NickName;
            PlayerState.Character = (CharactersEnum)PhotonNetwork.LocalPlayer.CustomProperties[CharacterKey];
            AnyStateAnimation[] animations = {
                new(Rig.Body, "Body_Idle", "Body_Attack", "Body_Jump"),
                new(Rig.Body, "Body_Walk", "Body_Attack", "Body_Jump"),
                new(Rig.Body, "Body_Jump", "Body_Attack"),
                new(Rig.Body, "Body_Double_Jump"),
                new(Rig.Body, "Body_Fall", "Body_Attack", "Body_Double_Jump"),
                new(Rig.Body, "Body_Attack", "Body_Shield"),
                new(Rig.Body, "Body_Shield", "Body_Attack", "Body_Jump", "Body_Double_Jump", "Body_Fall"),

                new(Rig.Legs, "Legs_Idle", "Legs_Attack"),
                new(Rig.Legs, "Legs_Walk", "Legs_Attack"),
                new(Rig.Legs, "Legs_Jump", "Legs_Double_Jump"),
                new(Rig.Legs, "Legs_Double_Jump"),
                new(Rig.Legs, "Legs_Fall", "Legs_Attack", "Legs_Double_Jump"),
                new(Rig.Legs, "Legs_Attack", "Legs_Shield"),
                new(Rig.Legs, "Legs_Shield", "Legs_Attack", "Legs_Jump", "Legs_Double_Jump", "Legs_Fall"),
            };

            playerComponent.Animator.AnimationTriggerEvent += PlayerUtilities.HandleAnimation;
            playerComponent.Animator.AddAnimations(animations);
            
            PlayerUtilities.GetSpriteRenderers();

            playerReferences.DamageDisplay.text = ((PlayerState.DamageMultiplier - 1) * 100) + "%";

            if (photonView.IsMine)
            {
                StartCoroutine(PlayerUtilities.SpawnCoroutine(transform.position));
            }
        }

        // Update is called once per frame
        private void Update()
        {
            if(PlayerState.IsDead) return;
            PlayerUtilities.HandleInput();
            PlayerUtilities.HandleAir();
            PlayerUtilities.HandleDeath();
            PlayerUtilities.HandleEnergy();
        }

        private void FixedUpdate()
        {
            PlayerActions.Move(transform);
        }
    }
}
