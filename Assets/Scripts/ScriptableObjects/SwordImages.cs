using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu]
    public class SwordImages : ScriptableObject
    {
        [SerializeField] private Sprite[] swordImages;
    
        public Sprite GetSwordImage(int index)
        {
            return swordImages[index];
        }
    }
}
