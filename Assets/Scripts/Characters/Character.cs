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
        AptoRobos
    }
    
    public class Character
    {
        public string DisplayName { get; }
        
        public string PrefabName { get; }
        
        public string CollectionIdHash { get; }

        public Character(string displayName, string prefabName, string collectionIdHash) {
            DisplayName = displayName;
            PrefabName = prefabName;
            CollectionIdHash = collectionIdHash;
        }
    }
}