using System;
using System.Collections.Generic;
using UnityEngine;

namespace Potions
{
    [CreateAssetMenu(fileName = "PotionRecipe", menuName = "Scriptable Objects/Potion Recipe")]
    public class PotionRecipe : ScriptableObject
    {
        [SerializeField] private List<Ingredient> ingredients;

        [SerializeField] private List<PotionValue> potionResults;

        [Serializable]
        private struct Ingredient
        {
            [SerializeField] public IngredientValue ingredientValue;
            [SerializeField] public int count;
        }

        public bool CheckCorrect(List<IngredientObject> toCheck)
        {
            foreach (Ingredient ingredient in this.ingredients)
            {
                int count = 0;

                foreach (IngredientObject ingredientObject in toCheck)
                {
                    if (ingredientObject.CheckValue(ingredient.ingredientValue))
                        count++;
                }

                if (count != ingredient.count)
                    return false;
            }

            return true;
        }

        public List<PotionValue> GetResults()
        {
            return this.potionResults;
        }
    }
}