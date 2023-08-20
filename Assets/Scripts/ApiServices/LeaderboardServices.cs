using System;
using System.Collections;
using ApiServices.Models.Leaderboard;
using Global;

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
                $"leaderboard/{Modes[(int)gameMode]}/{Endpoints[(int)leaderboardEndpoint]}?numDays={numDays}&limit={limit}";
            if (collectionIdHash != null)
            {
                endpoint += $"&collectionIdHash={collectionIdHash}";
            }
            return endpoint;
        }

        private static IEnumerator GetCasualPlayers(Action<LeaderboardRowData[]> callback, int numDays, int limit, 
            string collectionIdHash)
        {
            return ApiClient.GetRequest<CasualPlayerResponse>((response) =>
            {
                callback(Array.ConvertAll(response.rows,
                    row => new LeaderboardRowData(row.playerName, row.wins, row.losses, row.eliminations)));
            }, GetEndpoint(GameModes.Casual, LeaderboardEndpoints.Players, numDays, limit, collectionIdHash));
        }
        
        private static IEnumerator GetCasualCollections(Action<LeaderboardRowData[]> callback, int numDays, int limit)
        {
            return ApiClient.GetRequest<CasualCollectionResponse>((response) =>
            {
                callback(Array.ConvertAll(response.rows,
                    row => new LeaderboardRowData(Characters.Characters.GetCollectionName(row.collectionIdHash),
                        row.wins, row.losses, row.eliminations)));
            }, GetEndpoint(GameModes.Casual, LeaderboardEndpoints.Collections, numDays, limit, null));
        }   

        private static IEnumerator GetRankedPlayers(Action<LeaderboardRowData[]> callback, int numDays, int limit, 
            string collectionIdHash)
        {
            return ApiClient.GetRequest<RankedPlayerResponse>((response) =>
            {
                
                callback(Array.ConvertAll(response.rows,
                    row => new LeaderboardRowData(row.playerAddress, row.wins, row.losses, row.eliminations)));
            }, GetEndpoint(GameModes.Ranked, LeaderboardEndpoints.Players, numDays, limit, collectionIdHash));
        }
        
        private static IEnumerator GetRankedCollections(Action<LeaderboardRowData[]> callback, int numDays, int limit)
        {
            return ApiClient.GetRequest<RankedCollectionResponse>((response) =>
            {
                callback(Array.ConvertAll(response.rows,
                    row => new LeaderboardRowData(Characters.Characters.GetCollectionName(row.collectionIdHash),
                        row.wins, row.losses, row.eliminations)));
            }, GetEndpoint(GameModes.Ranked, LeaderboardEndpoints.Collections, numDays, limit, null));
        }
        
        public static IEnumerator GetLeaderboardData(Action<LeaderboardRowData[]> callback, GameModes mode, 
            LeaderboardEndpoints leaderboardEndpoint, int numDays, int limit, string collectionIdHash)
        {
            switch (mode)
            {
                case GameModes.Casual:
                    yield return leaderboardEndpoint switch
                    {
                        LeaderboardEndpoints.Players => GetCasualPlayers(callback, numDays, limit, collectionIdHash),
                        LeaderboardEndpoints.Collections => GetCasualCollections(callback, numDays, limit),
                        _ => throw new ArgumentOutOfRangeException(nameof(leaderboardEndpoint), leaderboardEndpoint,
                            null)
                    };
                    break;
                case GameModes.Ranked:
                    yield return leaderboardEndpoint switch
                    {
                        LeaderboardEndpoints.Players => GetRankedPlayers(callback, numDays, limit, collectionIdHash),
                        LeaderboardEndpoints.Collections => GetRankedCollections(callback, numDays, limit),
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
    }
}