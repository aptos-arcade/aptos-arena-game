using System;

namespace ApiServices.Models.Leaderboard
{
    [Serializable]
    public class RankedCollectionRow
    {
        public string collectionIdHash;        
        public int wins;
        public int losses;
        public int eliminations;
    }
    
    [Serializable]
    public class RankedCollectionResponse
    {
        public RankedCollectionRow[] rows;
    }
}