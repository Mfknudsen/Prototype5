using System.Collections.Generic;
using System.Linq;
using Interactions;
using Inventory;
using Potions;
using ScriptableVariables.Objects;
using ScriptableVariables.SystemSpecific;
using UnityEngine;
using UnityEngine.VFX;

namespace Mixer
{
    public class MixingStation : MonoBehaviour, IInteractable
    {
        [SerializeField] private List<PotionRecipe> allRecipes;

        [SerializeField] private TransformVariable handTransformVariable, cameraTransformVariable;

        [SerializeField] private InventoryItemListVariable inventoryItemListVariable;

        [SerializeField] private Transform resultSpawnPoint;

        [SerializeField] private Transform showcaseAddedPoint;

        [SerializeField] private int maxIngredientAmount = 2;

        [SerializeField] private InventoryHandler inventoryHandler;

        [SerializeField] private GameObject smokeVFX;

        private List<IngredientObject> currentAddedIngredients;
        private ParticleSystem smokeParticleSystem;

        public bool CheckCauldronFull() => currentAddedIngredients.Count >= maxIngredientAmount;
        
        private void Start()
        {
            this.currentAddedIngredients = new List<IngredientObject>();
            smokeParticleSystem = smokeVFX.GetComponent<ParticleSystem>();
        }

        private void Update()
        {
            if (this.cameraTransformVariable.Value)
                this.showcaseAddedPoint.LookAt(this.cameraTransformVariable.Value);
        }

        public void OnTrigger()
        {
            Debug.Log("Mixer");

            if (this.CheckCauldronFull()) return;

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

            this.OnAddedUpdated();
        }

        public void TriggerMixing()
        {
            if (this.currentAddedIngredients.Count == 0)
                return;

            Debug.Log("Ladle Mix");
            foreach (IngredientObject currentAddedIngredient in this.currentAddedIngredients)
            {
                Debug.Log(currentAddedIngredient.gameObject.name);
            }

            bool isCorrectRecipe = false;
            foreach (PotionRecipe potionRecipe in this.allRecipes.OrderBy(r => r.IngredientNeededCount()))
            {
                if (!potionRecipe.CheckCorrect(this.currentAddedIngredients))
                    continue;

                isCorrectRecipe = true;
                foreach (PotionValue potionValue in potionRecipe.GetResults())
                {
                    Transform t = Instantiate(potionValue.GetPrefab()).transform;
                    t.position = this.resultSpawnPoint.position;

                    if (t.GetComponent<Rigidbody>() is { } rb)
                        rb.useGravity = false;
                }

                break;
            }

            if (!isCorrectRecipe)
                smokeParticleSystem?.Play();

            foreach (IngredientObject currentAddedIngredient in this.currentAddedIngredients)
                Destroy(currentAddedIngredient.gameObject);

            this.currentAddedIngredients.Clear();
        }

        public bool IsActive()
        {
            return this.enabled;
        }

        public Vector3? Hover()
        {
            if (this.CheckCauldronFull() || inventoryHandler.CheckPotionInHand()) 
                return null;
            return this.transform.position;
        }

        private void OnAddedUpdated()
        {
            const float itemSize = 0.5f, spaceBetween = 0.25f;
            float offset = (itemSize - 1) * this.currentAddedIngredients.Count / 2f;
            int i = 0;
            foreach (IngredientObject currentAddedIngredient in this.currentAddedIngredients)
            {
                currentAddedIngredient.transform.parent = this.showcaseAddedPoint;
                currentAddedIngredient.transform.localRotation = Quaternion.identity;
                currentAddedIngredient.transform.localPosition =
                    new Vector3(offset + itemSize * i + spaceBetween, 0, 0);

                currentAddedIngredient.enabled = false;
                currentAddedIngredient.GetComponent<Collider>().enabled = false;
                currentAddedIngredient.GetComponent<Rigidbody>().isKinematic = true;
                currentAddedIngredient.gameObject.SetActive(true);

                i++;
            }
        }
    }
}