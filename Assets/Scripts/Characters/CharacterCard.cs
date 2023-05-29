using System;
using ExitGames.Client.Photon;
using Global;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using static Photon.PlayerPropertyKeys;

namespace Characters
{
    public class CharacterCard : MonoBehaviour
    {

        [SerializeField] private Button selectButton;
        [SerializeField] private CharactersEnum characterEnum;
        
        public delegate void CharacterSelectAction();
        public static event CharacterSelectAction OnSelect;
        
        
        private void Start()
        {
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