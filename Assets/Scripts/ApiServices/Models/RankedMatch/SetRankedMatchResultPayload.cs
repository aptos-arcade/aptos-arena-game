using System.Collections.Generic;
using Newtonsoft.Json;

namespace ApiServices.Models.RankedMatch
{
    [JsonObject]
    public class SetRankedMatchResultPayload
    {
        [JsonConstructor]
        public SetRankedMatchResultPayload(string matchAddress, int winnerIndex, List<List<RankedMatchPlayer>> teams)
        {
            MatchAddress = matchAddress;
            WinnerIndex = winnerIndex;
            Teams = teams;
        }

        [JsonProperty("matchAddress", Required = Required.Always)]
        public string MatchAddress { get; set; }
        
        [JsonProperty("winnerIndex", Required = Required.Always)]
        public int WinnerIndex { get; set; }
        
        [JsonProperty("teams", Required = Required.Always)]
        public List<List<RankedMatchPlayer>> Teams { get; set; }
    }
}