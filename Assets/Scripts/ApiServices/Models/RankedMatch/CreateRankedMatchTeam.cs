using System;
using System.Collections.Generic;

namespace ApiServices.Models.RankedMatch
{
    [Serializable]
    public class CreateRankedMatchTeam
    {
        public CreateRankedMatchTeam(List<string> players)
        {
            this.players = players;
        }
        
        public List<string> players;
    }
}