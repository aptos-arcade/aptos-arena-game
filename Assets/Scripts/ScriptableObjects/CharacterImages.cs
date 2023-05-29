using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu]
    public class CharacterImages : ScriptableObject
    {
        [SerializeField] private Sprite[] characterSprites;
    
        public Sprite GetCharacterSprite(int index)
        {
            return characterSprites[index];
        }
    }
}
