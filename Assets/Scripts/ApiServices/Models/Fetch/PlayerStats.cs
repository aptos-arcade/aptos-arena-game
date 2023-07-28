using Newtonsoft.Json;

namespace ApiServices.Models.Fetch
{
    [JsonObject]
    public class PlayerStats
    {
        [JsonConstructor]
        public PlayerStats() { }

        [JsonProperty("wins", Required = Required.Always)]
        public int Wins { get; private set; }

        [JsonProperty("losses", Required = Required.Always)]
        public int Losses { get; private set; }
        
        [JsonProperty("eloRating", Required = Required.Always)]
        public int EloRating { get; private set; }
    }
}