using Photon.Pun;
using TMPro;
using UnityEngine;

namespace UIComponents
{
    public class RegionalServerText : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<TMP_Text>().text = "Server Region: " + PhotonNetwork.CloudRegion;
        }
    }
}
