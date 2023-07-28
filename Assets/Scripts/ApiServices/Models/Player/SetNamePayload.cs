using Newtonsoft.Json;

namespace ApiServices.Models.Player
{
    [JsonObject]
    public class SetNamePayload
    {
        [JsonConstructor]
        public SetNamePayload(string name, string playerId)
        {
            Name = name;
            PlayerId = playerId;
        }
        
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; private set; }
        
        [JsonProperty("playerId", Required = Required.Always)]
        public string PlayerId { get; private set; }
    }
}