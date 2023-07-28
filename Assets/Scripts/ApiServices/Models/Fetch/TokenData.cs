using Newtonsoft.Json;

namespace ApiServices.Models.Fetch
{
    [JsonObject]
    public class TokenData
    {
        [JsonConstructor]
        public TokenData() { }
        
        [JsonProperty("tokenDataId", Required = Required.Always)]
        public TokenDataId TokenDataId { get; private set; }
        
        [JsonProperty("propertyVersion", Required = Required.Always)]
        public int PropertyVersion { get; private set; }
    }
}