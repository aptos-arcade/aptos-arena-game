using Photon;
using Photon.Pun;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu.RankedMenu
{
    public class BrawlerItemsManager : MonoBehaviour
    {
        [Header("Game Objects")] [SerializeField]
        private GameObject itemsPanel;

        [Header("Buttons")] [SerializeField] private Button characterButton;

        [Header("Images")] [SerializeField] private Image characterImage;
        [SerializeField] private Image swordImage;
        [SerializeField] private Image gunImage;

        [Header("Scriptable Objects")] [SerializeField]
        private CharacterImages characterImages;

        [SerializeField] private SwordImages swordImages;
        [SerializeField] private GunImages gunImages;

        [Header("Modals")] 
        [SerializeField] private ModalManager modalManager;

        private void Start()
        {
            characterButton.onClick.AddListener(() => modalManager.OpenAvailableCharactersModal());
        }

        public void ShowItemsPanel(bool isShown)
        {
            itemsPanel.SetActive(isShown);
        }

        public void UpdateItemsDisplay()
        {
            SetCharacterImage();
            SetSwordImage();
            SetGunImage();
            ShowItemsPanel(true);
        }

        private void SetCharacterImage()
        {
            var noCharacter = PhotonNetwork.LocalPlayer.CustomProperties[PlayerPropertyKeys.CharacterKey] == null;
            characterImage.sprite = noCharacter
                ? null
                : characterImages.GetCharacterSprite(
                    (int)PhotonNetwork.LocalPlayer.CustomProperties[PlayerPropertyKeys.CharacterKey]);
            var color = characterImage.color;
            color.a = noCharacter ? 0.1f : 1f;
            characterImage.color = color;
        }

        private void SetSwordImage()
        {
            var noSword = (int)PhotonNetwork.LocalPlayer.CustomProperties[PlayerPropertyKeys.SwordKey] == -1;
            swordImage.sprite = noSword
                ? null
                : swordImages.GetSwordImage(
                    (int)PhotonNetwork.LocalPlayer.CustomProperties[PlayerPropertyKeys.SwordKey]);
            var color = swordImage.color;
            color.a = noSword ? 0.1f : 1f;
            swordImage.color = color;
        }
        
        private void SetGunImage()
        {
            var noGun = (int)PhotonNetwork.LocalPlayer.CustomProperties[PlayerPropertyKeys.GunKey] == -1;
            gunImage.sprite = noGun
                ? null
                : gunImages.GetGunImage(
                    (int)PhotonNetwork.LocalPlayer.CustomProperties[PlayerPropertyKeys.GunKey]);
            var color = gunImage.color;
            color.a = noGun ? 0.1f : 1f;
            gunImage.color = color;
        }
    }
}