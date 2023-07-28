using Newtonsoft.Json;

namespace ApiServices.Models.Fetch
{
    [JsonObject]
    public class BrawlerData
    {
        [JsonConstructor]
        public BrawlerData() { }

        [JsonProperty("character", Required = Required.Always)]
        public CharacterData Character { get; private set; }

        [JsonProperty("meleeWeapon", Required = Required.Always)]
        public MeleeWeaponData MeleeWeapon { get; private set; }
        
        [JsonProperty("rangedWeapon", Required = Required.Always)]
        public RangedWeaponData RangedWeapon { get; private set; }
    }
}