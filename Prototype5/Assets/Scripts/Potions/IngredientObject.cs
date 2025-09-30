using UnityEngine;

namespace Potions
{
    public sealed class IngredientObject : MonoBehaviour
    {
        [SerializeField] private IngredientValue value;

        public bool CheckValue(IngredientValue toCheck)
        {
            return this.value.Equals(toCheck);
        }
    }
}