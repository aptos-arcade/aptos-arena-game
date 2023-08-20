using System;
using Characters;

namespace ApiServices.Models.CasualMatch
{
    [Serializable]
    public class CasualMatchPlayer
    {
        public CasualMatchPlayer(string playerId, CharactersEnum charactersEnum, int eliminations)
        {
            this.playerId = playerId;
            this.collectionIdHash = Characters.Characters.GetCharacter(charactersEnum).CollectionIdHash;
            this.eliminations = eliminations;
        }

        public string playerId;
        public string collectionIdHash;
        public int eliminations;
    }
}