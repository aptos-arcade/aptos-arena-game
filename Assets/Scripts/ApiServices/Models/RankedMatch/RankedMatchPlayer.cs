using Characters;
using Newtonsoft.Json;

namespace ApiServices.Models.RankedMatch
{
    [JsonObject]
    public class RankedMatchPlayer
    {
        [JsonConstructor]
        public RankedMatchPlayer(string playerAddress, CharactersEnum charactersEnum)
        {
            PlayerAddress = playerAddress;
            CollectionIdHash = Characters.Characters.GetCharacter(charactersEnum).CollectionIdHash;
        }

        [JsonProperty("playerAddress", Required = Required.Always)]
        public string PlayerAddress { get; private set; }
        
        [JsonProperty("collectionIdHash", Required = Required.Always)]
        public string CollectionIdHash { get; private set; }
    }
}