using Characters;
using Newtonsoft.Json;

namespace ApiServices.Models.CasualMatch
{
    [JsonObject]
    public class CasualMatchPlayer
    {
        [JsonConstructor]
        public CasualMatchPlayer(string playerId, CharactersEnum charactersEnum)
        {
            PlayerId = playerId;
            CollectionIdHash = Characters.Characters.GetCharacter(charactersEnum).CollectionIdHash;
        }

        [JsonProperty("playerId", Required = Required.Always)]
        public string PlayerId { get; private set; }
        
        [JsonProperty("collectionIdHash", Required = Required.Always)]
        public string CollectionIdHash { get; private set; }
    }
}