using UnityEngine;

namespace Potions
{
    [CreateAssetMenu(fileName = "IngredientValue", menuName = "Scriptable Objects/Ingredient Value")]
    public sealed class IngredientValue : ScriptableObject
    {
        [SerializeField] private GameObject ingredientPrefab;

        [SerializeField] [TextArea] private string description;

        public string GetDescription()
        {
            return this.description;
        }

        public GameObject GetPrefab()
        {
            return this.ingredientPrefab;
        }
    }
}