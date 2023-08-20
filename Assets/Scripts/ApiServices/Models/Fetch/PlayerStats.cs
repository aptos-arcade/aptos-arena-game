using System;

namespace ApiServices.Models.Fetch
{
    [Serializable]
    public class PlayerStats
    {
        public int wins;
        public int losses;
        public int eloRating;
    }
}