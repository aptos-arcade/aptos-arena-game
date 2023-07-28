using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Photon
{
    public class RegionButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private TMP_Text text;

        public void Initialize(Region region)
        {
            text.text = $"{region.Code} (Ping: {region.Ping})";
            button.onClick.AddListener(() => PhotonNetwork.ConnectToRegion(region.Code));
        }
    }
}