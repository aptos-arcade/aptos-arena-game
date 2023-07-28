using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Characters
{
    public static class Characters
    {
        public static readonly Dictionary<CharactersEnum, Character> AvailableCharacters = new()
        {
            {
                CharactersEnum.PontemPirates, 
                new Character(
                    "Pontem Pirates", 
                    "Pontem Pirate",
                    "aece05d29c0b543be608d73c44d8bb46a09e18e06097f7fdec078689e52ed118"
                )
            },
            {
                CharactersEnum.AptosMonkeys, 
                new Character(
                    "Aptos Monkeys", 
                    "Aptos Monkey",
                    "7ac8cecb76edbbd5da40d719bbb9795fc5744e4098ee0ce1be4bb86c90f42301"
                )
            },
            {
                CharactersEnum.Aptomingos, 
                new Character(
                    "Aptomingos", 
                    "Aptomingo",
                    "e6a7399d10406b993e25d8a3bf24842413ba8f1a08444dbfa5f1c31b09f0d16e"
                )
            },
            {
                CharactersEnum.BruhBears, 
                new Character(
                    "Bruh Bears", 
                    "Bruh Bear", 
                    "da59e5f610419f274a20341fb198bf98415712de11a4468cfd45cbe495600c2a"
                )
            },
            {
                CharactersEnum.DarkAges, 
                new Character(
                    "Dark Ages", 
                    "Dark Ages",
                    "a23b49b39acacce0adbcc328f94b910eb4adf7aa3258e7362cfbf2be505e1ec7"
                )
            },
            {
                CharactersEnum.Mavriks, 
                new Character(
                    "MAVRIK", 
                    "Mavrik",
                    "b0c10aba073b4ed474fa9615df596f9e9a689b8b9482bae5ae2832fab970a42d"
                )
            },
            {
                CharactersEnum.Spooks, 
                new Character(
                    "Spooks", 
                    "Spook",
                    "bc79c099fc7d0f853d8b9d69f34138c07bbb0caf3b75ee70d163e524153c8561"
                )
            }
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
        
        private static readonly Dictionary<string, string> CollectionIdHashToCollectionName = new()
        {
            {"aece05d29c0b543be608d73c44d8bb46a09e18e06097f7fdec078689e52ed118", "Pontem Pirates"},
            {"7ac8cecb76edbbd5da40d719bbb9795fc5744e4098ee0ce1be4bb86c90f42301", "Aptos Monkeys"},
            {"e6a7399d10406b993e25d8a3bf24842413ba8f1a08444dbfa5f1c31b09f0d16e", "Aptomingos"},
            {"da59e5f610419f274a20341fb198bf98415712de11a4468cfd45cbe495600c2a", "Bruh Bears"},
            {"a23b49b39acacce0adbcc328f94b910eb4adf7aa3258e7362cfbf2be505e1ec7", "Dark Ages"},
            {"b0c10aba073b4ed474fa9615df596f9e9a689b8b9482bae5ae2832fab970a42d", "MAVRIK"},
            {"bc79c099fc7d0f853d8b9d69f34138c07bbb0caf3b75ee70d163e524153c8561", "Spooks"}
        };

        public static Character GetCharacter(CharactersEnum characterEnum)
        {
            return AvailableCharacters[characterEnum];
        }
        
        public static CharactersEnum GetCharacterEnum(string collectionName)
        {
            return CollectionNameToEnum[collectionName];
        }
        
        public static string GetCollectionName(string collectionIdHash)
        {
            return CollectionIdHashToCollectionName[collectionIdHash];
        }

        public static string GetCollectionIdHash(string collectionName)
        {
            return AvailableCharacters[GetCharacterEnum(collectionName)].CollectionIdHash;
        }

        public static CharactersEnum GetRandomCharacter()
        {
            var values = Enum.GetValues(typeof(CharactersEnum));
            return (CharactersEnum)values.GetValue(Random.Range(0, values.Length));
        }
    }
}