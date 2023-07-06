using Photon;
using Photon.Pun;
using UnityEngine;
using Weapons;

namespace MainMenu
{
    public class CharacterDisplay : MonoBehaviour
    {

        [SerializeField] private GameObject[] characters;
        [SerializeField] private GameObject defaultCharacter;
        
        [SerializeField] private SwordSprite swordSprite;
        [SerializeField] private GunSprite gunSprite;
    
        // Start is called before the first frame update
        private void Start()
        {
            UpdateCharacter();
        }

        public void UpdateCharacter()
        {
            var characterIndex = PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(PlayerPropertyKeys.CharacterKey, 
                    out var characterEnum) && characterEnum != null ? (int)characterEnum : -1;
            for(var i = 0; i < characters.Length; i++)
            {
                characters[i].SetActive(i == characterIndex);
            }
            defaultCharacter.SetActive(characterIndex == -1);
            swordSprite.UpdateSwordSprite();
            gunSprite.UpdateGunSprite();
        }
    }
}
