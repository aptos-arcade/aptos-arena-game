using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using static Photon.PlayerPropertyKeys;
using Hashtable = ExitGames.Client.Photon.Hashtable;

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
            var playerProperties = new Hashtable() { { CharacterKey, characterEnum} };
            PhotonNetwork.SetPlayerCustomProperties(playerProperties);
            OnSelect?.Invoke();
        }
    }
}