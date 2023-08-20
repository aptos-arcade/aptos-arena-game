using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ApiServices.Models.RankedMatch;

namespace ApiServices
{
    public static class RankedMatchServices
    {

        public static IEnumerator CreateMatch(List<CreateRankedMatchTeam> teams, Action<bool, string> callback)
        {
            var allPlayers = teams.SelectMany(team => team.players).ToList();
            if (allPlayers.Distinct().Count() != allPlayers.Count)
            {
                callback(false, "Cannot create match with duplicate players.");
                yield break;
            }
            
            yield return ApiClient.PostRequest<CreateRankedMatchPayload, CreateRankedMatchResponse>(response =>
            {
                callback(response != null, response == null ? "Error creating match" : response.message);
            }, "match/ranked/createMatch", new CreateRankedMatchPayload(teams));
        }
        
        public static IEnumerator SetMatchResult(string matchAddress, int winnerIndex, 
            List<RankedMatchTeam> teams, Action<bool, string> callback)
        {
            yield return ApiClient.PostRequest<SetRankedMatchResultPayload, string>(response =>
            {
                callback(response != null, response == null ? "Error creating match" : "Match result set successfully");
            }, "match/ranked/setMatchResult", new SetRankedMatchResultPayload(matchAddress, winnerIndex, teams));
        
        }
    }
}