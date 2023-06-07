using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using static Photon.PlayerPropertyKeys;
using static Characters.Characters;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace Characters
{
    public class CharacterCard : MonoBehaviour
    {
        [SerializeField] private CharactersEnum characterEnum;
        // [SerializeField] private Image characterImage;

        private Button selectButton;
        
        public delegate void CharacterSelectAction();
        public static event CharacterSelectAction OnSelect;
        
        // private const string Endpoint = "http://localhost:3000/api/hasNFT";
        
        private Character character;
        
        // [System.Serializable]
        // private class HasNftResponse
        // {
        //     public bool hasNFT;
        // }
        
        
        private void Start()
        {
            selectButton = GetComponent<Button>();
            selectButton.onClick.AddListener(SelectCharacter);
            character = GetCharacter(characterEnum);
            // StartCoroutine(IsCharacterOwned());
        }

        private void SelectCharacter()
        {
            var playerProperties = new Hashtable() { { CharacterKey, characterEnum} };
            PhotonNetwork.SetPlayerCustomProperties(playerProperties);
            OnSelect?.Invoke();
        }

        // private IEnumerator IsCharacterOwned()
        // {
        //     Debug.Log("IsCharacterOwned");
        //     Debug.Log($"{Endpoint}/{character.CharacterIdHash}/{WalletManager.Instance.Address}");
        //     var request = UnityWebRequest.Get($"{Endpoint}/{character.CharacterIdHash}/{WalletManager.Instance.Address}");
        //     yield return request.SendWebRequest();
        //     if (request.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
        //     {
        //         Debug.LogError(request.error);
        //     }
        //     else
        //     {
        //         Debug.Log("Success");
        //         var response = JsonUtility.FromJson<HasNftResponse>(request.downloadHandler.text);
        //         if (!response.hasNFT) yield break;
        //         selectButton.interactable = true;
        //         characterImage.color = Color.white;
        //     }
        // }
    }
}