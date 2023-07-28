using System;
using System.Collections;
using ApiServices.Models.Fetch;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace ApiServices
{
    public static class FetchServices
    {
        public static IEnumerator FetchBrawlerAddress(Action<string> callback, string accountAddress)
        {
            var url = $"{ApiClient.BaseUrl()}/brawler/{accountAddress}";
            var req = UnityWebRequest.Get(url);
            yield return req.SendWebRequest();
            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(req.downloadHandler.text);
                callback("");
            }
            else
            {
                var response = JsonConvert.DeserializeObject<string>(req.downloadHandler.text);
                callback(response);
            }
        }

        public static IEnumerator FetchBrawlerData(Action<BrawlerData> callback, string accountAddress)
        {
            var url = $"{ApiClient.BaseUrl()}/brawler/{accountAddress}/brawlerData";
            var req = UnityWebRequest.Get(url);
            yield return req.SendWebRequest();
            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(req.downloadHandler.text);
                callback(null);
            }
            else
            {
                var response = JsonConvert.DeserializeObject<BrawlerData>(req.downloadHandler.text);
                callback(response);
            }
        }
        
        public static IEnumerator FetchOwnedCharacters(Action<TokenData[]> callback, string accountAddress)
        {
            var url = $"{ApiClient.BaseUrl()}/ownedCharacters/{accountAddress}";
            var req = UnityWebRequest.Get(url);
            yield return req.SendWebRequest();
            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(req.downloadHandler.text);
                callback(Array.Empty<TokenData>());
            }
            else
            {
                var response = JsonConvert.DeserializeObject<TokenData[]>(req.downloadHandler.text);
                callback(response);
            }
        }

        public static IEnumerator FetchPlayerStats(Action<PlayerStats> callback, string accountAddress)
        {
            var url = $"{ApiClient.BaseUrl()}/stats/{accountAddress}";
            var req = UnityWebRequest.Get(url);
            yield return req.SendWebRequest();
            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(req.downloadHandler.text);
                callback(null);
            }
            else
            {
                var response = JsonConvert.DeserializeObject<PlayerStats>(req.downloadHandler.text);
                callback(response);
            }
        }
    }
}