using Newtonsoft.Json;

namespace ApiServices.Models.Leaderboard
{
    [JsonObject]
    public class RankedPlayerRow
    {
        [JsonConstructor]
        public RankedPlayerRow() {}

        [JsonProperty("playerAddress", Required = Required.Always)]
        public string PlayerAddress { get; private set; }

        [JsonProperty("wins", Required = Required.Always)]
        public int Wins { get; private set; }
        
        [JsonProperty("losses", Required = Required.Always)]
        public int Losses { get; private set; }
    }
}