using System;
using System.Collections;
using ApiServices.Models.Fetch;

namespace ApiServices
{
    public static class FetchServices
    {
        public static IEnumerator FetchBrawlerAddress(Action<BrawlerAddress> callback, string accountAddress)
        {
            yield return ApiClient.GetRequest(callback, $"brawler/{accountAddress}");
        }

        public static IEnumerator FetchBrawlerData(Action<BrawlerData> callback, string accountAddress)
        {
            yield return ApiClient.GetRequest(callback, $"brawler/{accountAddress}/brawlerData");
        }
        
        public static IEnumerator FetchOwnedCharacters(Action<OwnedTokens> callback, string accountAddress)
        {
            yield return ApiClient.GetRequest(callback, $"ownedCharacters/{accountAddress}");
        }

        public static IEnumerator FetchPlayerStats(Action<PlayerStats> callback, string accountAddress)
        {
            yield return ApiClient.GetRequest(callback, $"stats/{accountAddress}");
        }
    }
}