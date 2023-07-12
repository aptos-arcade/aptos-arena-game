using Animations;
using Characters;
using Gameplay;
using Photon.Pun;
using UnityEngine;
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
                new(Rig.Body, false, "Body_Idle", "Body_Attack", "Body_Jump", "Body_Shield", "Body_Dodge", "Body_Dash"),
                new(Rig.Body,false, "Body_Walk", "Body_Attack", "Body_Jump", "Body_Shield", "Body_Dodge", "Body_Dash"),
                new(Rig.Body,false, "Body_Jump", "Body_Attack", "Body_Dodge", "Body_Dash", "Body_Double_Jump"),
                new(Rig.Body,false, "Body_Double_Jump", "Body_Dodge", "Body_Attack", "Body_FastFall"),
                new(Rig.Body,false, "Body_Fall", "Body_Attack", "Body_Jump", "Body_Double_Jump", "Body_Shield", "Body_Dodge", "Body_Dash", "Body_FastFall"),
                new(Rig.Body, false,"Body_Attack", "Body_Shield", "Body_Dodge", "Body_FastFall"),
                new(Rig.Body,true, "Body_Shield", "Body_Attack", "Body_Jump", "Body_Double_Jump", "Body_Dodge", "Body_Dash", "Body_FastFall"),
                new(Rig.Body, false,"Body_Dodge", "Body_Attack", "Body_Dash", "Body_FastFall"),
                new(Rig.Body, false,"Body_Dash", "Body_Attack", "Body_Shield", "Body_Dodge", "Body_FastFall"),
                new(Rig.Body, true,"Body_FastFall", "Body_Attack"),

                new(Rig.Legs, false,"Legs_Idle", "Legs_Attack", "Legs_Jump", "Legs_Shield", "Legs_Dodge", "Legs_Dash"),
                new(Rig.Legs, false,"Legs_Walk", "Legs_Shield", "Legs_Dodge", "Legs_Jump", "Legs_Dash", "Legs_Attack"),
                new(Rig.Legs, false,"Legs_Jump", "Legs_Double_Jump", "Legs_Dodge", "Legs_Dash"),
                new(Rig.Legs, false,"Legs_Double_Jump", "Legs_Dodge", "Legs_Attack", "Legs_FastFall"),
                new(Rig.Legs, false,"Legs_Fall", "Legs_Attack", "Legs_Double_Jump", "Legs_Shield", "Legs_Dodge", "Legs_Dash", "Legs_FastFall"),
                new(Rig.Legs, false,"Legs_Attack", "Legs_Shield", "Legs_Dodge", "Legs_FastFall"),
                new(Rig.Legs, true,"Legs_Shield", "Legs_Attack", "Legs_Jump", "Legs_Double_Jump", "Legs_Dodge", "Legs_Dash", "Legs_FastFall"),
                new(Rig.Legs, false,"Legs_Dodge", "Legs_Attack", "Legs_Dash", "Legs_FastFall"),
                new(Rig.Legs, false,"Legs_Dash", "Legs_Attack", "Legs_Shield", "Legs_Dodge", "Legs_FastFall"),
                new(Rig.Legs, true,"Legs_FastFall", "Legs_Attack"),
            };

            playerComponent.Animator.AnimationTriggerEvent += PlayerUtilities.HandleAnimation;
            playerComponent.Animator.AddAnimations(animations);
            
            PlayerUtilities.GetSpriteRenderers();

            playerReferences.DamageDisplay.text = ((PlayerState.DamageMultiplier - 1) * 100) + "%";
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
