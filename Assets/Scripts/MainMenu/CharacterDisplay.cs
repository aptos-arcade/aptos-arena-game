using Photon;
using Photon.Pun;
using UnityEngine;

namespace MainMenu
{
    public class CharacterDisplay : MonoBehaviour
    {

        [SerializeField] private GameObject[] characters;
    
        // Start is called before the first frame update
        private void Start()
        {
            var characterIndex = PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(PlayerPropertyKeys.CharacterKey,
                out var characterEnum) ? (int)characterEnum : 0;
            for(var i = 0; i < characters.Length; i++)
            {
                characters[i].SetActive(i == characterIndex);
            }
        }
    }
}
