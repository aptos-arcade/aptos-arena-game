using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace AptosIntegration
{
    public static class MatchEntryFunctions
    {
        private const string LocalEndpoint = "http://localhost:3000";
        private const string ProdEndpoint = "https://www.aptosarcade.com";

        private static string GetEndpoint(bool isLocal, string function)
        {
            var endpoint = isLocal ? LocalEndpoint : ProdEndpoint;
            return $"{endpoint}/api/arena/{function}";
        }
        
        public static IEnumerator CreateMatch(List<List<string>> teams, Action<bool, string> callback)
        {
            var payload = new CreateMatchPayload { Teams = teams };
            var payloadBytes = new UTF8Encoding().GetBytes(JsonConvert.SerializeObject(payload));
            var request = new UnityWebRequest(GetEndpoint(false, "createMatch"), "POST");
            request.uploadHandler = new UploadHandlerRaw(payloadBytes);
            Debug.Log(request.uploadHandler.data.Length);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.downloadHandler.text);
                callback(false, request.error);
            }
            else
            {
                var response = JsonConvert.DeserializeObject<CreateMatchResponse>(request.downloadHandler.text);
                callback(true, response.Message);
            }
        }
        
        public static IEnumerator SetMatchResult(string matchAddress, int winnerIndex, Action<bool, string> callback)
        {
            var payload = new SetMatchResultPayload()
            {
                MatchAddress = matchAddress,
                WinnerIndex = winnerIndex
            };
            var payloadBytes = new UTF8Encoding().GetBytes(JsonConvert.SerializeObject(payload));
            var request = new UnityWebRequest(GetEndpoint(false, "setMatchResult"), "POST");
            request.SetRequestHeader("Content-Type", "application/json");
            request.uploadHandler = new UploadHandlerRaw(payloadBytes);
            request.downloadHandler = new DownloadHandlerBuffer();
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.downloadHandler.text);
                callback(false, request.error);
            }
            else
            {
                callback(true, "Match result set successfully.");
            }
        }
    }
    
    [JsonObject]
    public class CreateMatchPayload
    {
        [JsonProperty("teams", Required = Required.Always)]
        public List<List<string>> Teams { get; set; }
    }
    
    [JsonObject]
    public class SetMatchResultPayload
    {
        [JsonProperty("matchAddress", Required = Required.Always)]
        public string MatchAddress { get; set; }
        
        [JsonProperty("winnerIndex", Required = Required.Always)]
        public int WinnerIndex { get; set; }
    }
    
    [JsonObject]
    public class CreateMatchResponse
    {
        [JsonProperty("message", Required = Required.Always)]
        public string Message { get; set; }
    }
}