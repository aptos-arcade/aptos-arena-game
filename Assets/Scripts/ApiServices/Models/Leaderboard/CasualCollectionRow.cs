using Newtonsoft.Json;

namespace ApiServices.Models.Leaderboard
{
    [JsonObject]
    public class CasualCollectionRow
    {
        [JsonConstructor]
        public CasualCollectionRow() {}

        [JsonProperty("collectionIdHash", Required = Required.Always)]
        public string CollectionIdHash { get; private set; }
        
        [JsonProperty("wins", Required = Required.Always)]
        public int Wins { get; private set; }
        
        [JsonProperty("losses", Required = Required.Always)]
        public int Losses { get; private set; }
        
        [JsonProperty("eliminations", Required = Required.Always)]
        public int Eliminations { get; private set; }
    }
}