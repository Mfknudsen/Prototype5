using UnityEngine;

namespace Potions
{
    [CreateAssetMenu(fileName = "PotionValue", menuName = "Scriptable Objects/PotionValue")]
    public sealed class PotionValue : ScriptableObject
    {
        [SerializeField] private GameObject potionPrefab;
    }
}