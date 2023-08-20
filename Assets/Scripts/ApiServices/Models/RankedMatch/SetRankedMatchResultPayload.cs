using System;
using System.Collections.Generic;

namespace ApiServices.Models.RankedMatch
{
    [Serializable]
    public class SetRankedMatchResultPayload
    {
        public SetRankedMatchResultPayload(string matchAddress, int winnerIndex, List<RankedMatchTeam> teams)
        {
            this.matchAddress = matchAddress;
            this.winnerIndex = winnerIndex;
            this.teams = teams;
        }

        public string matchAddress;
        public int winnerIndex;
        public List<RankedMatchTeam> teams;
    }
}