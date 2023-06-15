using System.Collections.Generic;

namespace Characters
{
    public static class Characters
    {
        public static readonly Dictionary<CharactersEnum, Character> AvailableCharacters = new(){
            {CharactersEnum.PontemPirates, new Character("Pontem Pirates", "Pontem Pirate") },
            {CharactersEnum.AptosMonkeys, new Character("Aptos Monkeys", "Aptos Monkey") },
            {CharactersEnum.Aptomingos, new Character("Aptomingos", "Aptomingo")},
            {CharactersEnum.BruhBears, new Character("Bruh Bears", "Bruh Bear") },
            {CharactersEnum.DarkAges, new Character("Dark Ages", "Dark Ages") },
            {CharactersEnum.Mavriks, new Character("MAVRIK", "Mavrik")},
            {CharactersEnum.Spooks, new Character("Spooks", "Spook")}
        };

        private static readonly Dictionary<string, CharactersEnum> CollectionNameToEnum = new()
        {
            {"Aptos Monkeys", CharactersEnum.AptosMonkeys},
            {"Bruh Bears", CharactersEnum.BruhBears},
            {"Pontem Dark Ages", CharactersEnum.DarkAges},
            {"MAVRIK", CharactersEnum.Mavriks},
            {"Pontem Space Pirates", CharactersEnum.PontemPirates},
            {"Spooks", CharactersEnum.Spooks},
            {"Aptomingos", CharactersEnum.Aptomingos}
        };

        public static Character GetCharacter(CharactersEnum characterEnum)
        {
            return AvailableCharacters[characterEnum];
        }
        
        public static CharactersEnum GetCharacterEnum(string collectionName)
        {
            return CollectionNameToEnum[collectionName];
        }
    }
}