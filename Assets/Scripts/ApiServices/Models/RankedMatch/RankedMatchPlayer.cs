using System;
using Characters;

namespace ApiServices.Models.RankedMatch
{
    [Serializable]
    public class RankedMatchPlayer
    {
        public RankedMatchPlayer(string playerAddress, CharactersEnum charactersEnum, int eliminations)
        {
            this.playerAddress = playerAddress;
            this.collectionIdHash = Characters.Characters.GetCharacter(charactersEnum).CollectionIdHash;
            this.eliminations = eliminations;
        }

        public string playerAddress;
        public string collectionIdHash;
        public int eliminations;
    }
}