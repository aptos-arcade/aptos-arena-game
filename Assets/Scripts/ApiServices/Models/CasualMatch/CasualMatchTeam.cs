using System;
using System.Collections.Generic;

namespace ApiServices.Models.CasualMatch
{
    [Serializable]
    public class CasualMatchTeam
    {
        public CasualMatchTeam(List<CasualMatchPlayer> players)
        {
            this.players = players;
        }

        public List<CasualMatchPlayer> players;
    }
}