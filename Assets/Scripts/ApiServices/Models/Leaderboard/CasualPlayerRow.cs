using Newtonsoft.Json;

namespace ApiServices.Models.Leaderboard
{
    [JsonObject]
    public class CasualPlayerRow
    {
        [JsonConstructor]
        public CasualPlayerRow() {}

        [JsonProperty("playerId", Required = Required.Always)]
        public string PlayerId { get; private set; }
        
        [JsonProperty("playerName", Required = Required.Always)]
        public string PlayerName { get; private set; }
        
        [JsonProperty("wins", Required = Required.Always)]
        public int Wins { get; private set; }
        
        [JsonProperty("losses", Required = Required.Always)]
        public int Losses { get; private set; }
        
        [JsonProperty("eliminations", Required = Required.Always)]
        public int Eliminations { get; private set; }
    }
}