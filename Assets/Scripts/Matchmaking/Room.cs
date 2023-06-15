using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class Room : MonoBehaviour
{
    [SerializeField] private byte numberOfPlayers;
    [SerializeField] private Global.GameModes gameMode;

    private Button joinButton;

    public const string ModePropKey = "m";
    public const string MatchAddressPropKey = "ma";
    
    private void Start()
    {
        joinButton = GetComponent<Button>();
        joinButton.onClick.AddListener(JoinRoom);
    }

    private void JoinRoom()
    {
        var roomOptions = new RoomOptions
        {
            MaxPlayers = numberOfPlayers,
            CustomRoomPropertiesForLobby = new []{ ModePropKey },
            CustomRoomProperties = new Hashtable() { { ModePropKey, gameMode } }
        };
        var expectedCustomRoomProperties = new Hashtable() { { ModePropKey, gameMode } };
        PhotonNetwork.JoinRandomOrCreateRoom(
            roomOptions: roomOptions, 
            expectedMaxPlayers: numberOfPlayers,
            expectedCustomRoomProperties: expectedCustomRoomProperties
        );
    }
}
