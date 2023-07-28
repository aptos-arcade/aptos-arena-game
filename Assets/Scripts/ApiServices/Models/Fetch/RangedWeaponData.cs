using Newtonsoft.Json;

namespace ApiServices.Models.Fetch
{
    [JsonObject]
    public class RangedWeaponData
    {
        [JsonConstructor]
        public RangedWeaponData() { }

        [JsonProperty("power", Required = Required.Always)]
        public int Power { get; private set; }

        [JsonProperty("type", Required = Required.Always)]
        public int Type { get; private set; }
    }
}