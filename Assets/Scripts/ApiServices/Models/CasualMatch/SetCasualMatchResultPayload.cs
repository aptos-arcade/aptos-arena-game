using Newtonsoft.Json;

namespace ApiServices.Models.CasualMatch
{
    [JsonObject]
    public class SetCasualMatchResultPayload
    {
        [JsonProperty("matchId", Required = Required.Always)]
        public string MatchId { get; set; }
        
        [JsonProperty("winnerIndex", Required = Required.Always)]
        public int WinnerIndex { get; set; }
    }
}