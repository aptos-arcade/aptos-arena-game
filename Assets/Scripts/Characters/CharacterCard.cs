using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static Photon.PlayerPropertyKeys;

namespace Characters
{
    public class CharacterCard : MonoBehaviour
    {
        [SerializeField] private CharactersEnum characterEnum;

        private Button selectButton;
        
        public delegate void CharacterSelectAction();
        public static event CharacterSelectAction OnSelect;

        private void Start()
        {
            selectButton = GetComponent<Button>();
            selectButton.onClick.AddListener(SelectCharacter);
        }

        private void SelectCharacter()
        {
            var playerProperties = PhotonNetwork.LocalPlayer.CustomProperties;
            if(!playerProperties.TryAdd(CharacterKey, characterEnum)) playerProperties[CharacterKey] = characterEnum;
            PhotonNetwork.SetPlayerCustomProperties(playerProperties);
            OnSelect?.Invoke();
        }
    }
}