using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace ApiServices
{
    public static class ApiClient
    {
        private const string ProdURL = "https://www.brawl3r.com";
        private const string DevURL = "http://localhost:3000";
        
        private static string BaseUrl()
        {
            #if UNITY_EDITOR
                return $"{DevURL}/api/";
            #else
                return $"{ProdURL}/api/";
            #endif
        }
        
        public static IEnumerator GetRequest<T>(Action<T> callback, string endpoint)
        {
            var req = UnityWebRequest.Get($"{BaseUrl()}{endpoint}");
            yield return req.SendWebRequest();
            var success = req.result == UnityWebRequest.Result.Success;
            if(!success)
            {
                Debug.LogError(req.error);
                Debug.Log(req.downloadHandler.text);
            }
            callback(success
                ? JsonUtility.FromJson<T>(req.downloadHandler.text)
                : default);
        }
        
        public static IEnumerator PostRequest<TRequest, TResponse>(Action<TResponse> callback, string endpoint, TRequest payload)
        {
            var request = new UnityWebRequest($"{BaseUrl()}{endpoint}", "POST");
            request.uploadHandler = new UploadHandlerRaw(new UTF8Encoding().GetBytes(JsonUtility.ToJson(payload)));
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
            var success = request.result == UnityWebRequest.Result.Success;
            if(!success)
            {
                Debug.LogError(request.error);
                Debug.Log(request.downloadHandler.text);
            }
            callback(success
                ? JsonUtility.FromJson<TResponse>(request.downloadHandler.text) 
                : default);
        }
    }
}