using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Photon.PlayerPropertyKeys;

namespace Gameplay
{
    public class ConnectedPlayer : MonoBehaviour
    {
        [SerializeField] private TMP_Text playerName;
        [SerializeField] private TMP_Text playerEliminations;
        [SerializeField] private TMP_Text playerDeaths;
        [SerializeField] private Image characterImage;

        public void SetPlayerInfo(string name, Sprite image, int eliminations, int deaths, int team)
        {
            playerName.text = name;
            characterImage.sprite = image;
            playerEliminations.text = eliminations.ToString();
            playerDeaths.text = deaths.ToString();
            playerName.color = team == (int)PhotonNetwork.LocalPlayer.CustomProperties[TeamKey]
                ? Color.white
                : Color.red;
        }
    }
}
