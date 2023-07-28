using System.Collections.Generic;
using Newtonsoft.Json;

namespace ApiServices.Models.CasualMatch
{
    [JsonObject]
    public class CreateCasualMatchPayload
    {
        [JsonProperty("teams", Required = Required.Always)]
        public List<List<CasualMatchPlayer>> Teams { get; set; }
    }
}