using System;
using Characters;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using ScriptableObjects;
using UnityEngine;
using static Photon.PlayerPropertyKeys;

namespace Gameplay
{
    public class ConnectedPlayersManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject connectedPlayersPanel;
        [SerializeField] private GameObject connectedPlayersView;
        [SerializeField] private ConnectedPlayer connectedPlayerPrefab;
        [SerializeField] private CharacterImages characterImages;

        private void Start()
        {
            ListAllPlayers();
        }

        private void Update()
        {
            connectedPlayersPanel.SetActive(Input.GetKey(KeyCode.Tab));
        }
    
        private void ListAllPlayers()
        {
            foreach (var roomPlayer in connectedPlayersView.transform)
            {
                if (roomPlayer is Transform player)
                {
                    Destroy(player.gameObject);
                }
            }
        
            foreach (var player in PhotonNetwork.PlayerList)
            {
                OnPlayerEnteredRoom(player);
            }
        }

        public override void OnPlayerEnteredRoom(Photon.Realtime.Player player)
        {
            var roomPlayer = Instantiate(connectedPlayerPrefab, connectedPlayersView.transform);
            var playerCharacter = (CharactersEnum)player.CustomProperties[CharacterKey];
            roomPlayer.SetPlayerInfo(player.NickName, characterImages.GetCharacterSprite((int)playerCharacter));
        }
    
        public override void OnPlayerLeftRoom(Photon.Realtime.Player player)
        {
            ListAllPlayers();
        }
    }
}
