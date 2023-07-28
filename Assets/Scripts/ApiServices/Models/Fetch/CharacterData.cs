using Newtonsoft.Json;

namespace ApiServices.Models.Fetch
{
    [JsonObject]
    public class CharacterData
    {
        [JsonConstructor]
        public CharacterData() { }

        [JsonProperty("creator", Required = Required.Always)]
        public string Creator { get; private set; }

        [JsonProperty("collection", Required = Required.Always)]
        public string Collection { get; private set; }
        
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; private set; }
    }
}