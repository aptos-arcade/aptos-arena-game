using System;

namespace ApiServices.Models.Leaderboard
{
    [Serializable]
    public class RankedPlayerRow
    {
        public string playerAddress;
        public int wins;
        public int losses;
        public int eliminations;
    }
    
    [Serializable]
    public class RankedPlayerResponse
    {
        public RankedPlayerRow[] rows;
    }
}