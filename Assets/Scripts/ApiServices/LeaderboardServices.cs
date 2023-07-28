using System;
using System.Collections;
using ApiServices.Models.Leaderboard;
using Global;
using Leaderboard;
using Newtonsoft.Json;
using UnityEngine.Networking;
using Utilities;

namespace ApiServices
{
    public static class LeaderboardServices
    {
        public enum LeaderboardEndpoints {
            Players,
            Collections,
        }
        private static readonly string[] Endpoints = {"players", "collections"};

        private static readonly string[] Modes = {"casual", "ranked", "training"};

        private static string GetEndpoint(GameModes gameMode, LeaderboardEndpoints leaderboardEndpoint, int numDays, 
            int limit, string collectionIdHash)
        {
            var endpoint =
                $"{ApiClient.BaseUrl()}/leaderboard/{Modes[(int)gameMode]}/{Endpoints[(int)leaderboardEndpoint]}?numDays={numDays}&limit={limit}";
            if (collectionIdHash != null)
            {
                endpoint += $"&collectionIdHash={collectionIdHash}";
            }
            return endpoint;
        }
        
        private static IEnumerator GetLeaderboardData(Action<bool, string> callback, GameModes mode,
            LeaderboardEndpoints leaderboardEndpoint, int numDays, int limit, string collectionIdHash)
        {
            var endpoint = GetEndpoint(mode, leaderboardEndpoint, numDays, limit, collectionIdHash);
            var request = UnityWebRequest.Get(endpoint);
            yield return request.SendWebRequest();
            callback(request.result == UnityWebRequest.Result.Success, request.downloadHandler.text);
        }
        
        private static IEnumerator GetCasualPlayers(Action<CasualPlayerRow[]> callback, int numDays, int limit, string collectionIdHash)
        {
            return GetLeaderboardData((success, response) =>
            {
                callback(success ? JsonConvert.DeserializeObject<CasualPlayerRow[]>(response) : Array.Empty<CasualPlayerRow>());
            }, GameModes.Casual, LeaderboardEndpoints.Players, numDays, limit, collectionIdHash);
        }
        
        private static IEnumerator GetCasualCollections(Action<CasualCollectionRow[]> callback, int numDays, int limit)
        {
            return GetLeaderboardData((success, response) =>
            {
                callback(success ? JsonConvert.DeserializeObject<CasualCollectionRow[]>(response) : Array.Empty<CasualCollectionRow>());
            }, GameModes.Casual, LeaderboardEndpoints.Collections, numDays, limit, null);
        }   

        private static IEnumerator GetRankedPlayers(Action<RankedPlayerRow[]> callback, int numDays, int limit, string collectionIdHash)
        {
            return GetLeaderboardData((success, response) =>
            {
                callback(success ? JsonConvert.DeserializeObject<RankedPlayerRow[]>(response) : Array.Empty<RankedPlayerRow>());
            }, GameModes.Ranked, LeaderboardEndpoints.Players, numDays, limit, collectionIdHash);
        }
        
        private static IEnumerator GetRankedCollections(Action<RankedCollectionRow[]> callback, int numDays, int limit)
        {
            return GetLeaderboardData((success, response) =>
            {
                callback(success ? JsonConvert.DeserializeObject<RankedCollectionRow[]>(response) : Array.Empty<RankedCollectionRow>());
            }, GameModes.Ranked, LeaderboardEndpoints.Collections, numDays, limit, null);
        }
        
        public static IEnumerator GetLeaderboardData(Action<LeaderboardRowData[]> callback, GameModes mode, LeaderboardEndpoints leaderboardEndpoint, int numDays, int limit, string collectionIdHash)
        {
            switch (mode)
            {
                case GameModes.Casual:
                    yield return leaderboardEndpoint switch
                    {
                        LeaderboardEndpoints.Players => GetCasualPlayers(
                            rows => { callback(Array.ConvertAll(rows, CasualPlayerRowToLeaderboardRowData)); }, numDays,
                            limit, collectionIdHash),
                        LeaderboardEndpoints.Collections => GetCasualCollections(
                            rows => { callback(Array.ConvertAll(rows, CasualCollectionRowToLeaderboardRowData)); },
                            numDays, limit),
                        _ => throw new ArgumentOutOfRangeException(nameof(leaderboardEndpoint), leaderboardEndpoint,
                            null)
                    };
                    break;
                case GameModes.Ranked:
                    yield return leaderboardEndpoint switch
                    {
                        LeaderboardEndpoints.Players => GetRankedPlayers(
                            rows => { callback(Array.ConvertAll(rows, RankedPlayerRowToLeaderboardRowData)); }, numDays,
                            limit, collectionIdHash),
                        LeaderboardEndpoints.Collections => GetRankedCollections(
                            rows => { callback(Array.ConvertAll(rows, RankedCollectionRowToLeaderboardRowData)); },
                            numDays, limit),
                        _ => throw new ArgumentOutOfRangeException(nameof(leaderboardEndpoint), leaderboardEndpoint,
                            null)
                    };
                    break;
                case GameModes.Training:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }
        
        private static LeaderboardRowData CasualPlayerRowToLeaderboardRowData(CasualPlayerRow row)
        {
            return new LeaderboardRowData(row.PlayerName, row.Wins, row.Losses);
        }
        
        private static LeaderboardRowData CasualCollectionRowToLeaderboardRowData(CasualCollectionRow row)
        {
            return new LeaderboardRowData(Characters.Characters.GetCollectionName(row.CollectionIdHash), row.Wins,
                row.Losses);
        }
        
        private static LeaderboardRowData RankedPlayerRowToLeaderboardRowData(RankedPlayerRow row)
        {
            return new LeaderboardRowData(StringUtils.Ellipsize(row.PlayerAddress), row.Wins, row.Losses);
        }
        
        private static LeaderboardRowData RankedCollectionRowToLeaderboardRowData(RankedCollectionRow row)
        {
            return new LeaderboardRowData(Characters.Characters.GetCollectionName(row.CollectionIdHash), row.Wins,
                row.Losses);
        }
    }
}