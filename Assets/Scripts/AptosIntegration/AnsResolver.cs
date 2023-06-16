using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace AptosIntegration
{
    public static class AnsResolver
    {
        
        [Serializable]
        private class AnsResponse
        {
            public string name;
        }
        
        public static IEnumerator ResolveAns(Action<string> callback, string walletAddress)
        {
            var request = UnityWebRequest.Get($"https://www.aptosnames.com/api/mainnet/v1/primary-name/{walletAddress}");
            yield return request.SendWebRequest();
            if (request.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
                callback("");
            }
            else
            {
                var response = JsonUtility.FromJson<AnsResponse>(request.downloadHandler.text);
                callback(response.name ?? "");
            }
        }
    }
}