using ApiServices.Models.Leaderboard;
using TMPro;
using UnityEngine;

namespace Leaderboard
{
    public class LeaderboardPlayerRow : MonoBehaviour
    {
        
        [SerializeField] private TMP_Text playerNameText;
        [SerializeField] private TMP_Text winsText;
        [SerializeField] private TMP_Text lossesText;

        public void Initialize(LeaderboardRowData data)
        {
            playerNameText.text = data.Name;
            winsText.text = data.Wins.ToString();
            lossesText.text = data.Losses.ToString();
        }
    }
}