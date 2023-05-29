using Photon.Pun;
using TMPro;
using UnityEngine;

public class PingTextController : MonoBehaviour
{
    
    private TMP_Text pingText;
    
    // Start is called before the first frame update
    private void Start()
    {
        pingText = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    private void Update()
    {
        var ping = PhotonNetwork.GetPing();
        pingText.text = "Ping: " + ping;
        pingText.color = ping > 100 ? Color.red : new Color(0.6588235f, 0.8078431f, 1f) ;
    }
}
