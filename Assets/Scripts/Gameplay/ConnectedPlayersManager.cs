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
    public class ConnectedPlayersManager : MonoBehaviour
    {
        [SerializeField] private GameObject connectedPlayersPanel;
        [SerializeField] private GameObject connectedPlayersView;
        [SerializeField] private ConnectedPlayer connectedPlayerPrefab;
        [SerializeField] private CharacterImages characterImages;

        private void Update()
        {
            connectedPlayersPanel.SetActive(Input.GetKey(KeyCode.Tab));
        }
    
        public void ListAllPlayers()
        {
            foreach (var roomPlayer in connectedPlayersView.transform)
            {
                if (roomPlayer is Transform player)
                {
                    Destroy(player.gameObject);
                }
            }

            foreach (var player in MatchManager.Instance.PlayerInfos)
            {
                var roomPlayer = Instantiate(connectedPlayerPrefab, connectedPlayersView.transform);
                var networkPlayer = PhotonNetwork.CurrentRoom.GetPlayer(player.ActorNumber);
                if(networkPlayer == null) continue;
                var playerCharacter = (CharactersEnum)networkPlayer.CustomProperties[CharacterKey];
                var playerTeam = (int)networkPlayer.CustomProperties[TeamKey];
                roomPlayer.SetPlayerInfo(player.Name, characterImages.GetCharacterSprite((int)playerCharacter),
                    player.Kills, 3 - player.Lives, playerTeam);
            }
        }
    }
}
