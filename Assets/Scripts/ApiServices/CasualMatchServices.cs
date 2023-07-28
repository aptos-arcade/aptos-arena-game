using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApiServices.Models.CasualMatch;
using ApiServices.Models.RankedMatch;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace ApiServices
{
    public static class CasualMatchServices
    {
        private static string GetEndpoint(string function)
        {
            return $"{ApiClient.BaseUrl()}/match/casual/{function}";
        }

        public static IEnumerator CreateMatch(List<List<CasualMatchPlayer>> teams, Action<bool, string> callback)
        {
            var payload = new CreateCasualMatchPayload { Teams = teams };
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
        
        public static IEnumerator SetMatchResult(string matchId, int winnerIndex, Action<bool, string> callback)
        {
            var payload = new SetCasualMatchResultPayload()
            {
                MatchId = matchId,
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
                callback(true, "Match result set successfully.");
            }
        }
    }
}