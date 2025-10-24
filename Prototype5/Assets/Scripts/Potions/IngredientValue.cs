using UnityEngine;

namespace Potions
{
    [CreateAssetMenu(fileName = "IngredientValue", menuName = "Scriptable Objects/Ingredient Value")]
    public sealed class IngredientValue : ScriptableObject
    {
        [SerializeField] private GameObject ingredientPrefab;

        [SerializeField] [TextArea] private string description;

        [SerializeField] private IngredientTag tag1, tag2;

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