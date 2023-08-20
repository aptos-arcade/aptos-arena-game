using System.Collections;
using ApiServices.Models.Fetch;
using AptosIntegration;
using TMPro;
using UnityEngine;

namespace MainMenu.RankedMenu
{
    public class PlayerEloRatingManager : MonoBehaviour
    {
        [SerializeField] private GameObject statsDisplay;
        [SerializeField] private TMP_Text eloText;
        [SerializeField] private TMP_Text winsText;
        [SerializeField] private TMP_Text lossesText;

        private bool loadSuccessful;
        
        public void OnEnable()
        {
            StartCoroutine(LoadPlayerStats());
        }

        private IEnumerator LoadPlayerStats()
        {
            yield return ApiServices.FetchServices.FetchPlayerStats(HandlePlayerStats, WalletManager.Instance.Address);
        }

        private void HandlePlayerStats(PlayerStats stats)
        {
            if (stats == null) return;
            eloText.text = stats.eloRating.ToString();
            winsText.text = stats.wins.ToString();
            lossesText.text = stats.losses.ToString();
        }

        public void ShowEloRating(bool active)
        {
            statsDisplay.SetActive(active);
        }
    }
}