using System;
using System.Collections;
using ApiServices.Models.Player;

namespace ApiServices
{
    public static class PlayerServices
    {
        public static IEnumerator SetPlayerName(string playerId, string newName, Action<bool> callback)
        {
            yield return ApiClient.PostRequest<SetNamePayload, string>(response =>
            {
                callback(response != null);
            },"player/setName", new SetNamePayload(newName, playerId));
        }
    }
}