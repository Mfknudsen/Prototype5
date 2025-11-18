using System;
using System.Collections.Generic;
using UnityEngine;

namespace Potions
{
    [CreateAssetMenu(fileName = "PotionRecipe", menuName = "Scriptable Objects/Potion Recipe")]
    public class PotionRecipe : ScriptableObject
    {
        [SerializeField] private List<IngredientAndAmount> ingredients;

        [SerializeField] private List<PotionValue> potionResults;


        public bool CheckCorrect(List<IngredientObject> toCheck)
        {
            foreach (IngredientAndAmount ingredient in this.ingredients)
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

        public int IngredientNeededCount()
        {
            int result = 0;

            foreach (IngredientAndAmount ingredient in this.ingredients)
            {
                result += ingredient.count;
            }

            return result;
        }

        public List<PotionValue> GetResults()
        {
            return this.potionResults;
        }

        public List<IngredientAndAmount> GetIngredientsWithAmounts()
        {
            return this.ingredients;
        }
    }

    [Serializable]
    public struct IngredientAndAmount
    {
        [SerializeField] public IngredientValue ingredientValue;
        [SerializeField] public int count;
    }
}