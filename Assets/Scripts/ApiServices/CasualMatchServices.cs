using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ApiServices.Models.CasualMatch;

namespace ApiServices
{
    public static class CasualMatchServices
    {
        public static IEnumerator CreateMatch(IEnumerable<List<string>> teams, Action<bool, string> callback)
        {
            var allPlayers = teams.SelectMany(team => team).ToList();
            if (allPlayers.Distinct().Count() != allPlayers.Count)
            {
                callback(false, "Cannot create match with duplicate players.");
                yield break;
            }

            yield return ApiClient.PostRequest<string, CreateCasualMatchResponse>(response =>
            {
                callback(response != null, response == null ? "Error creating match" : response.message);
            }, $"match/casual/createMatch", "");
        }
        
        public static IEnumerator SetMatchResult(string matchId, int winnerIndex, List<CasualMatchTeam> teams, 
            Action<bool, string> callback)
        {
            yield return ApiClient.PostRequest<SetCasualMatchResultPayload, string>(response =>
            {
                callback(response != null, response == null ? "Error creating match" : "Match result set successfully");
            }, $"match/casual/setMatchResult", new SetCasualMatchResultPayload(matchId, winnerIndex, teams));
        }
    }
}