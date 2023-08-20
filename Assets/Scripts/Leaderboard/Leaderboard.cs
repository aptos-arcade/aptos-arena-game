using ApiServices;
using ApiServices.Models.Leaderboard;
using Global;
using TMPro;
using UnityEngine;

namespace Leaderboard
{
    public class Leaderboard : MonoBehaviour
    {
        [SerializeField] private GameObject leaderboardContent;
        [SerializeField] private GameObject loadingIndicator;
        [SerializeField] private LeaderboardPlayerRow rowPrefab;
        [SerializeField] private Transform rowContainer;
        [SerializeField] private GameObject noResultsText;
        [SerializeField] private TMP_Text playerRowHeader;

        private Coroutine fetchLeaderboardRowsCoroutine;

        private void OnEnable()
        {
            ShowLoadingIndicator();
        }
        
        private void ShowLoadingIndicator()
        {
            loadingIndicator.SetActive(true);
            leaderboardContent.SetActive(false);
            noResultsText.SetActive(false);
        }

        private void UpdateLeaderboard(LeaderboardRowData[] rows)
        {
            foreach (Transform child in rowContainer)
            {
                Destroy(child.gameObject);
            }
            
            foreach (var row in rows)
            {
                var playerRow = Instantiate(rowPrefab, rowContainer);
                playerRow.Initialize(row);
            }
            
            loadingIndicator.SetActive(false);
            if(rows.Length == 0)
            {
                noResultsText.SetActive(true);
            }
            else
            {
                leaderboardContent.SetActive(true);
            }
        }
        
        public void FetchLeaderboardRows(GameModes gameMode, 
            LeaderboardServices.LeaderboardEndpoints leaderboardEndpoint, int numDays = 10, int limit = 10, 
            string collection = null)
        {
            if(fetchLeaderboardRowsCoroutine != null) StopCoroutine(fetchLeaderboardRowsCoroutine);
            ShowLoadingIndicator();
            playerRowHeader.text = leaderboardEndpoint == LeaderboardServices.LeaderboardEndpoints.Players
                ? "Player"
                : "Collection";
            fetchLeaderboardRowsCoroutine =
                StartCoroutine(LeaderboardServices.GetLeaderboardData(UpdateLeaderboard, gameMode, 
                    leaderboardEndpoint, numDays, limit, collection));
        }
    }
}