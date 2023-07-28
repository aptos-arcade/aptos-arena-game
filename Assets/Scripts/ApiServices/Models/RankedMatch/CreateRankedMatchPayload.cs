using System.Collections.Generic;
using Newtonsoft.Json;

namespace ApiServices.Models.RankedMatch
{
    [JsonObject]
    public class CreateRankedMatchPayload
    {
        [JsonProperty("teams", Required = Required.Always)]
        public List<List<RankedMatchPlayer>> Teams { get; set; }
    }
}