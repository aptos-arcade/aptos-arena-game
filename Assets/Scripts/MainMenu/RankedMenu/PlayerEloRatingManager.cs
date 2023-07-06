using System.Collections;
using ApiServices;
using ApiServices.Models;
using AptosIntegration;
using AptosIntegration.WalletManager;
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
            yield return ApiClient.FetchPlayerStats(HandlePlayerStats, WalletManager.Instance.Address);
        }

        private void HandlePlayerStats(PlayerStats stats)
        {
            if (stats == null) return;
            eloText.text = stats.EloRating.ToString();
            winsText.text = stats.Wins.ToString();
            lossesText.text = stats.Losses.ToString();
        }

        public void ShowEloRating(bool active)
        {
            statsDisplay.SetActive(active);
        }
    }
}