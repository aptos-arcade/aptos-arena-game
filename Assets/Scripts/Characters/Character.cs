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
        
        public string CreatorAddress { get; }
        public string CollectionName { get; }
        public string TokenName { get; }
        
        public Character(
            string displayName, 
            string prefabName,
            string creatorAddress,
            string collectionName,
            string tokenName
        )
        {
            DisplayName = displayName;
            PrefabName = prefabName;
            CreatorAddress = creatorAddress;
            CollectionName = collectionName;
            TokenName = tokenName;
        }
    }
}