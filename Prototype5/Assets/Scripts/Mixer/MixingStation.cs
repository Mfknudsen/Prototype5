using System.Collections.Generic;
using Interactions;
using Inventory;
using Potions;
using ScriptableVariables.Objects;
using ScriptableVariables.SystemSpecific;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Mixer
{
    public class MixingStation : MonoBehaviour, IInteractable
    {
        [SerializeField] private List<PotionRecipe> allRecipes;

        [SerializeField] private TransformVariable handTransformVariable;

        [SerializeField] private InventoryItemListVariable inventoryItemListVariable;

        [SerializeField] private Transform resultSpawnPoint;

        private List<IngredientObject> currentAddedIngredients;

        private void Start()
        {
            this.currentAddedIngredients = new List<IngredientObject>();
        }

        public void OnTrigger()
        {
            Debug.Log("Mixer");
            if (this.handTransformVariable == null || this.handTransformVariable.Value == null)
                return;

            IngredientObject ingredientObject = null;
            foreach (Transform t in this.handTransformVariable.Value)
            {
                if (t.gameObject.TryGetComponent(out ingredientObject))
                    break;
            }


            if (ingredientObject == null)
                return;

            Debug.Log($"Ingredient : {ingredientObject.name}");
            this.currentAddedIngredients.Add(ingredientObject);

            this.inventoryItemListVariable.Remove(ingredientObject.GetComponent<InventoryItem>());
            ingredientObject.gameObject.SetActive(false);
            ingredientObject.transform.parent = null;
        }

        public void TriggerMixing()
        {
            if (this.currentAddedIngredients.Count == 0)
                return;

            foreach (PotionRecipe potionRecipe in this.allRecipes)
            {
                if (!potionRecipe.CheckCorrect(this.currentAddedIngredients))
                    continue;

                const float offset = 0.5f;

                foreach (PotionValue potionValue in potionRecipe.GetResults())
                {
                    Transform t = Instantiate(potionValue.GetPrefab()).transform;
                    t.position = this.resultSpawnPoint.position + new Vector3(
                        Random.Range(-offset, offset),
                        0,
                        Random.Range(-offset, offset));
                }

                break;
            }

            foreach (IngredientObject currentAddedIngredient in this.currentAddedIngredients)
                Destroy(currentAddedIngredient);

            this.currentAddedIngredients.Clear();
        }
    }
}