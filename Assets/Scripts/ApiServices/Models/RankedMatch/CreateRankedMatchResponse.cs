using Newtonsoft.Json;

namespace ApiServices.Models.RankedMatch
{
    [JsonObject]
    public class CreateRankedMatchResponse
    {
        [JsonProperty("message", Required = Required.Always)]
        public string Message { get; set; }
    }
}