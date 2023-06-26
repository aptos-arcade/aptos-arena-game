using Photon.Pun;
using TMPro;
using UnityEngine;

public class FeedManager : MonoBehaviourPunCallbacks
{
    
    [SerializeField] private TMP_Text feedTextPrefab;
    
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player player)
    {
        WriteMessage(player.NickName + " has joined the game", 3f);
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player player)
    {
        WriteMessage(player.NickName + " has left the game", 3f);
    }
    
    public void WriteMessage(string message, float destroyTime)
    {
        var playerText = Instantiate(feedTextPrefab, transform);
        playerText.text = message;
        Destroy(playerText.gameObject, destroyTime);
    }
}
