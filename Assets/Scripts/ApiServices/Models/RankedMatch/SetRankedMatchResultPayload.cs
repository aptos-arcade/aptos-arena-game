using Newtonsoft.Json;

namespace ApiServices.Models.RankedMatch
{
    [JsonObject]
    public class SetRankedMatchResultPayload
    {
        [JsonProperty("matchAddress", Required = Required.Always)]
        public string MatchAddress { get; set; }
        
        [JsonProperty("winnerIndex", Required = Required.Always)]
        public int WinnerIndex { get; set; }
    }
}