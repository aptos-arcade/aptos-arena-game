using System;

namespace ApiServices.Models.Leaderboard
{
    [Serializable]
    public class CasualPlayerRow
    {
        public string playerId;
        public string playerName;
        public int wins;
        public int losses;
        public int eliminations;
    }
    
    [Serializable]
    public class CasualPlayerResponse
    {
        public CasualPlayerRow[] rows;
    }
}