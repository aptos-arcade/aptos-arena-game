using System.Collections.Generic;

namespace Characters
{
    public static class Characters
    {
        public static readonly Dictionary<CharactersEnum, Character> AvailableCharacters = new(){
            {CharactersEnum.PontemPirates, new Character(
                "Pontem Pirates", 
                "Pontem Pirate",
                "0xc46dd298b89d38314b486b2182a6163c4c955dce3509bf30751c307f5ecc2f36", 
                "Pontem Space Pirates", 
                "Space Pirate #993"
                )
            },
            {CharactersEnum.AptosMonkeys, new Character(
                    "Aptos Monkeys", 
                    "Aptos Monkey",
                    "0xf932dcb9835e681b21d2f411ef99f4f5e577e6ac299eebee2272a39fb348f702", 
                    "Aptos Monkeys", 
                    "AptosMonkeys #4037"
                )
            },
            {CharactersEnum.Aptomingos, new Character(
                    "Aptomingos", 
                    "Aptomingo",
                    "0xf932dcb9835e681b21d2f411ef99f4f5e577e6ac299eebee2272a39fb348f702", 
                    "Aptomingos",
                    "AptosMonkeys #4037"
                )
            },
            {CharactersEnum.BruhBears, new Character(
                    "Bruh Bears", 
                    "Bruh Bear",
                    "0xf932dcb9835e681b21d2f411ef99f4f5e577e6ac299eebee2272a39fb348f702", 
                    "Bruh Bears",
                    "AptosMonkeys #4037"
                )
            },
            {CharactersEnum.DarkAges, new Character(
                    "Dark Ages", 
                    "Dark Ages",
                    "0xf932dcb9835e681b21d2f411ef99f4f5e577e6ac299eebee2272a39fb348f702", 
                    "Aptomingos",
                    "AptosMonkeys #4037"
                )
            },
            {CharactersEnum.Mavriks, new Character(
                    "MAVRIK", 
                    "Mavrik",
                    "0xf932dcb9835e681b21d2f411ef99f4f5e577e6ac299eebee2272a39fb348f702", 
                    "Mavriks",
                    "AptosMonkeys #4037"
                )
            },
            {CharactersEnum.Spooks, new Character(
                    "Spooks", 
                    "Spook",
                    "0xf932dcb9835e681b21d2f411ef99f4f5e577e6ac299eebee2272a39fb348f702", 
                    "Spook",
                    "AptosMonkeys #4037"
                )
            }
        };
    }
}