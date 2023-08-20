using System;

namespace ApiServices.Models.Leaderboard
{
    [Serializable]
    public class CasualCollectionRow
    {
        public string collectionIdHash;
        public int wins;
        public int losses;
        public int eliminations;
    }
    
    [Serializable]
    public class CasualCollectionResponse
    {
        public CasualCollectionRow[] rows;
    }
}