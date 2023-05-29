using Photon.Pun;
using TMPro;
using UnityEngine;

public class FeedManager : MonoBehaviourPunCallbacks
{
    
    [SerializeField] private TMP_Text feedTextPrefab;
    
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player player)
    {
        var playerText = Instantiate(feedTextPrefab, transform);
        playerText.text = player.NickName + " has joined the game";
        Destroy(playerText.gameObject, 3f);
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player player)
    {
        var playerText = Instantiate(feedTextPrefab, transform);
        playerText.text = player.NickName + " has left the game";
        Destroy(playerText.gameObject, 3f);
    }
}
