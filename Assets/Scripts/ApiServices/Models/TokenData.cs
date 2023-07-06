using Newtonsoft.Json;

namespace ApiServices.Models
{
    [JsonObject]
    public class TokenData
    {
        [JsonConstructor]
        public TokenData() { }
        
        [JsonProperty("creator_address", Required = Required.Always)]
        public string Creator { get; private set; }
        
        [JsonProperty("collection_name", Required = Required.Always)]
        public string Collection { get; private set; }
        
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; private set; }
        
        [JsonProperty("property_version", Required = Required.Always)]
        public int PropertyVersion { get; private set; }
    }
}