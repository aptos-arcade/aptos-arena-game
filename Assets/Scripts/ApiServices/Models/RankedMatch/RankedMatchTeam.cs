using System;
using System.Collections.Generic;

namespace ApiServices.Models.RankedMatch
{
    [Serializable]
    public class RankedMatchTeam
    {
        public RankedMatchTeam(List<RankedMatchPlayer> players)
        {
            this.players = players;
        }
        
        public List<RankedMatchPlayer> players;
    }
}