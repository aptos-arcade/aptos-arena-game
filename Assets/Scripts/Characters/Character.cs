namespace Characters
{
    public enum CharactersEnum
    {
        PontemPirates, 
        AptosMonkeys, 
        Aptomingos, 
        BruhBears,
        DarkAges,
        Mavriks,
        Spooks,
    }
    
    public class Character
    {
        public string DisplayName { get; }
        
        public string PrefabName { get; }

        public Character(string displayName, string prefabName) {
            DisplayName = displayName;
            PrefabName = prefabName;
        }
    }
}