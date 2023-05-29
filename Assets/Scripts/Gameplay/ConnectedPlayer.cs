using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
    public class ConnectedPlayer : MonoBehaviour
    {
        [SerializeField] private TMP_Text playerName;
        [SerializeField] private Image characterImage;

        public void SetPlayerInfo(string name, Sprite image)
        {
            playerName.text = name;
            characterImage.sprite = image;
        }
    }
}
