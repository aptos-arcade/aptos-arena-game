using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using ApiServices.Models.RankedMatch;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace ApiServices
{
    public static class RankedMatchServices
    {
        
        private static string GetEndpoint(string function)
        {
            return $"{ApiClient.BaseUrl()}/match/ranked/{function}";
        }

        public static IEnumerator CreateMatch(List<List<RankedMatchPlayer>> teams, Action<bool, string> callback)
        {
            var payload = new CreateRankedMatchPayload { Teams = teams };
            var payloadBytes = new UTF8Encoding().GetBytes(JsonConvert.SerializeObject(payload));
            var request = new UnityWebRequest(GetEndpoint("createMatch"), "POST");
            request.uploadHandler = new UploadHandlerRaw(payloadBytes);
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
                var response = JsonConvert.DeserializeObject<CreateRankedMatchResponse>(request.downloadHandler.text);
                callback(true, response.Message);
            }
        }
        
        public static IEnumerator SetMatchResult(string matchAddress, int winnerIndex, Action<bool, string> callback)
        {
            var payload = new SetRankedMatchResultPayload()
            {
                MatchAddress = matchAddress,
                WinnerIndex = winnerIndex
            };
            var payloadBytes = new UTF8Encoding().GetBytes(JsonConvert.SerializeObject(payload));
            var request = new UnityWebRequest(GetEndpoint("setMatchResult"), "POST");
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
                callback(true, request.downloadHandler.text);
            }
        }
    }
}