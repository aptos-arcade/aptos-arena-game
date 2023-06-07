using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu]
    public class GunImages : ScriptableObject
    {
        [SerializeField] private Sprite[] gunImages;
    
        public Sprite GetGunImage(int index)
        {
            return gunImages[index];
        }
    }
}
