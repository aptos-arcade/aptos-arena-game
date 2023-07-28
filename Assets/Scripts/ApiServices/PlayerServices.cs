using System;
using System.Collections;
using System.Text;
using ApiServices.Models.Player;
using Newtonsoft.Json;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.Networking;

namespace ApiServices
{
    public static class PlayerServices
    {
        private static string GetEndpoint(string function)
        {
            return $"{ApiClient.BaseUrl()}/player/{function}";
        }
        
        public static IEnumerator SetPlayerName(string newName, Action<bool> callback)
        {
            if(AuthenticationService.Instance == null) 
                throw new Exception("AuthenticationService.Instance is null");
            var payload = new SetNamePayload(newName, AuthenticationService.Instance.PlayerId);
            var payloadBytes = new UTF8Encoding().GetBytes(JsonConvert.SerializeObject(payload));
            var request = new UnityWebRequest(GetEndpoint("setName"), "POST");
            request.uploadHandler = new UploadHandlerRaw(payloadBytes);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
            Debug.Log(request.downloadHandler.text);
            callback(request.result == UnityWebRequest.Result.Success);
        }
    }
}