using System;

namespace ApiServices.Models.Fetch
{
    [Serializable]
    public class BrawlerData
    {
        public CharacterData character;
        public MeleeWeaponData meleeWeapon;
        public RangedWeaponData rangedWeapon;
    }
}