using System;
using System.Collections.Generic;

namespace ApiServices.Models.CasualMatch
{
    [Serializable]
    public class CreateCasualMatchPayload
    {
        public CreateCasualMatchPayload(List<CasualMatchTeam> teams)
        {
            this.teams = teams;
        }

        public List<CasualMatchTeam> teams;
    }
}