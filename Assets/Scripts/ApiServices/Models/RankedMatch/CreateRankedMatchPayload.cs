using System;
using System.Collections.Generic;

namespace ApiServices.Models.RankedMatch
{
    [Serializable]
    public class CreateRankedMatchPayload
    {
        public CreateRankedMatchPayload(List<CreateRankedMatchTeam> teams)
        {
            this.teams = teams;
        }
        
        public List<CreateRankedMatchTeam> teams;
    }
}