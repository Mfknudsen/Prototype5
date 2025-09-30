using UnityEngine;

namespace Potions
{
    [CreateAssetMenu(fileName = "PotionValue", menuName = "Scriptable Objects/Potion Value")]
    public sealed class PotionValue : ScriptableObject
    {
        [SerializeField] private GameObject potionPrefab;

        [SerializeField] [TextArea] private string description;

        public GameObject GetPrefab()
        {
            return this.potionPrefab;
        }
    }
}