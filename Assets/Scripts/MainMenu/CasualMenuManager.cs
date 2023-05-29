using System;
using Characters;
using Photon.Pun;
using UnityEngine;
using static Photon.PlayerPropertyKeys;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Random = UnityEngine.Random;

namespace MainMenu
{
    public class CasualMenuManager : MonoBehaviour
    {
        // Start is called before the first frame update
        private void Start()
        {
            var playerProperties = new Hashtable() { { CharacterKey, GetRandomCharacter()} };
            PhotonNetwork.SetPlayerCustomProperties(playerProperties);
        }
    
        private static CharactersEnum GetRandomCharacter()
        {
            var values = Enum.GetValues(typeof(CharactersEnum));
            return (CharactersEnum)values.GetValue(Random.Range(0, values.Length));
        }
    }
}
