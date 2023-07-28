using Newtonsoft.Json;

namespace ApiServices.Models.CasualMatch
{
    [JsonObject]
    public class CreateCasualMatchResponse
    {
        [JsonProperty("message", Required = Required.Always)]
        public string Message { get; set; }
    }
}