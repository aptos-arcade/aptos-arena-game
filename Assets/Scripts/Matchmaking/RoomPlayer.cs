using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomPlayer : MonoBehaviour
{

    [SerializeField] private TMP_Text playerName;
    [SerializeField] private Image characterImage;

    public void SetPlayerInfo(string name, Sprite image)
    {
        playerName.text = name;
        characterImage.sprite = image;
    }
}
