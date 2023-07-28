using System;
using ApiServices;
using TMPro;
using UnityEngine;
using Characters;
using Global;

namespace Leaderboard
{
    public class LeaderboardManager : MonoBehaviour
    {
        [SerializeField] private Leaderboard leaderboard;

        [Header("Dropdowns")] 
        [SerializeField] private TMP_Dropdown gameModeDropdown;
        [SerializeField] private TMP_Dropdown leaderboardTypeDropdown;
        [SerializeField] private TMP_Dropdown collectionDropdown;
        [SerializeField] private TMP_Dropdown numDaysDropdown;

        private GameModes gameMode = GameModes.Casual;
        private LeaderboardServices.LeaderboardEndpoints leaderboardEndpoint = LeaderboardServices.LeaderboardEndpoints.Players;
        private string collection;
        private int numDays = 1;
        
        private readonly int[] numDaysOptions = {1, 7, 30, 90, 365};
        
        private void Start()
        {
            gameModeDropdown.onValueChanged.AddListener(OnGameModeChanged);
            leaderboardTypeDropdown.onValueChanged.AddListener(OnLeaderboardTypeChanged);
            collectionDropdown.onValueChanged.AddListener(OnCollectionChanged);
            numDaysDropdown.onValueChanged.AddListener(OnTimeframeChanged);
            
            foreach (CharactersEnum character in Enum.GetValues(typeof(CharactersEnum)))
            {
                collectionDropdown.options.Add(
                    new TMP_Dropdown.OptionData(Characters.Characters.GetCharacter(character).DisplayName));
            }

            FetchLeaderboardRows();
        }
        
        private void FetchLeaderboardRows()
        {
            leaderboard.FetchLeaderboardRows(gameMode, leaderboardEndpoint, numDays: numDays, collection: collection);
        }
        
        private void OnGameModeChanged(int value)
        {
            gameMode = (GameModes)value;
            FetchLeaderboardRows();
        }

        private void OnLeaderboardTypeChanged(int value)
        {
            leaderboardEndpoint = (LeaderboardServices.LeaderboardEndpoints)value;
            collectionDropdown.gameObject.SetActive(leaderboardEndpoint ==
                                                    LeaderboardServices.LeaderboardEndpoints.Players);
            FetchLeaderboardRows();
        }

        private void OnCollectionChanged(int value)
        {
            collection = value == 0 ? null : Characters.Characters.GetCharacter((CharactersEnum)(value - 1)).CollectionIdHash;
            FetchLeaderboardRows();
        }
        
        private void OnTimeframeChanged(int value)
        {
            numDays = numDaysOptions[value];
            FetchLeaderboardRows();
        }
    }
}