using System;
using System.Collections.Generic;
using Inventory;
using Potions;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utils;

namespace UI.Book
{
    public sealed class RecipePage : MonoBehaviour
    {
        [Header("Left UI References")] [SerializeField]
        private TMP_Text pageTitleLeft;

        [FormerlySerializedAs("effectDescriptionLeft")] [SerializeField]
        private TMP_Text descriptionLeft;

        [SerializeField] private TMP_Text flavourDescriptionLeft;
        [SerializeField] private Transform potionParentLeft;
        [SerializeField] private Image pageBackgroundLeft;
        [SerializeField] private Transform ingredient1Left;
        [SerializeField] private Transform ingredient2Left;
        [SerializeField] private Transform ingredient3Left;

        [Header("Right UI References")] [SerializeField]
        private TMP_Text pageTitleRight;

        [FormerlySerializedAs("effectDescriptionRight")] [SerializeField]
        private TMP_Text descriptionRight;

        [SerializeField] private TMP_Text flavourDescriptionRight;
        [SerializeField] private Transform potionParentRight;
        [SerializeField] private Image pageBackgroundRight;
        [SerializeField] private Transform ingredient1Right;
        [SerializeField] private Transform ingredient2Right;
        [SerializeField] private Transform ingredient3Right;

        public void SetupFromRecipes(PotionRecipe recipe1, PotionRecipe recipe2)
        {
            int layer = LayerMask.NameToLayer("Book UI");

            if (recipe1 != null)
            {
                // Debug.Log(potion1.name);
                // Title
                if (this.pageTitleLeft != null) this.pageTitleLeft.text = NameUtils.RemovePotionPrefix(recipe1.name);

                PotionValue result = recipe1.GetResults()[0];

                // Effect
                if (this.descriptionLeft != null) this.descriptionLeft.text = result.GetDescription();
                this.descriptionLeft.ForceMeshUpdate(true);

                // Flavour
                if (this.flavourDescriptionLeft != null) this.flavourDescriptionLeft.text = result.GetFlavor();
                this.flavourDescriptionLeft.ForceMeshUpdate(true);

                if (this.pageBackgroundLeft != null)
                    this.pageBackgroundLeft.enabled = this.pageBackgroundLeft.sprite != null;

                if (this.potionParentLeft != null)
                {
                    foreach (Transform child in this.potionParentLeft)
                        Destroy(child.gameObject);

                    GameObject prefab = result.GetPrefab();

                    if (prefab != null)
                    {
                        GameObject instance = Instantiate(prefab, this.potionParentLeft);
                        instance.transform.localPosition = result.GetShowcaseOffset();
                        instance.transform.localRotation = Quaternion.Euler(result.GetShowcaseRotation());
                        instance.transform.localScale = Vector3.one;
                        Destroy(instance.GetComponent<Rigidbody>());
                        Destroy(instance.GetComponent<Collider>());
                        Destroy(instance.GetComponent<IngredientObject>());
                        Destroy(instance.GetComponent<InventoryItem>());
                        this.SetLayer(instance, layer);
                    }
                }

                List<IngredientAndAmount> ingredientsWithAmounts = recipe1.GetIngredientsWithAmounts();
                for (int i = 0; i < 3; i++)
                {
                    Transform parent = i switch
                    {
                        0 => this.ingredient1Left,
                        1 => this.ingredient2Left,
                        2 => this.ingredient3Left,
                        _ => throw new ArgumentOutOfRangeException()
                    };

                    if (i >= ingredientsWithAmounts.Count)
                    {
                        parent.gameObject.SetActive(false);
                        continue;
                    }

                    IngredientAndAmount current = ingredientsWithAmounts[i];

                    Image image = parent.GetComponent<Image>();
                    image.color = Color.white;
                    image.sprite = current.ingredientValue.GetSprite();

                    continue;

                    GameObject instance = Instantiate(current.ingredientValue.GetPrefab(), parent);
                    instance.transform.localPosition = current.ingredientValue.GetShowcaseOffset();
                    instance.transform.localRotation = Quaternion.Euler(current.ingredientValue.GetShowcaseRotation());
                    instance.transform.localScale = Vector3.one;
                    Destroy(instance.GetComponent<Rigidbody>());
                    Destroy(instance.GetComponent<Collider>());
                    Destroy(instance.GetComponent<IngredientObject>());
                    Destroy(instance.GetComponent<InventoryItem>());
                    this.SetLayer(instance, layer);
                }
            }

            if (recipe2 != null)
            {
                // Title
                if (this.pageTitleRight != null) this.pageTitleRight.text = NameUtils.RemovePotionPrefix(recipe2.name);

                PotionValue result = recipe1.GetResults()[0];

                // Effect
                if (this.descriptionRight != null) this.descriptionRight.text = result.GetDescription();
                this.descriptionRight.ForceMeshUpdate(true);

                // Flavour @TODO: potion.GetFlavour?
                if (this.flavourDescriptionRight != null) this.flavourDescriptionRight.text = result.GetFlavor();
                this.flavourDescriptionRight.ForceMeshUpdate(true);

                if (this.pageBackgroundRight != null)
                    this.pageBackgroundRight.enabled = this.pageBackgroundRight.sprite != null;

                if (this.potionParentRight != null)
                {
                    foreach (Transform child in this.potionParentRight)
                        Destroy(child.gameObject);

                    GameObject prefab = result.GetPrefab();

                    if (prefab != null)
                    {
                        GameObject instance = Instantiate(prefab, this.potionParentRight);
                        instance.transform.localPosition = result.GetShowcaseOffset();
                        instance.transform.localRotation = Quaternion.Euler(result.GetShowcaseRotation());
                        instance.transform.localScale = Vector3.one;
                        Destroy(instance.GetComponent<Rigidbody>());
                        Destroy(instance.GetComponent<Collider>());
                        Destroy(instance.GetComponent<IngredientObject>());
                        Destroy(instance.GetComponent<InventoryItem>());
                        this.SetLayer(instance, layer);
                    }
                }

                List<IngredientAndAmount> ingredientsWithAmounts = recipe2.GetIngredientsWithAmounts();

                for (int i = 0; i < Math.Min(ingredientsWithAmounts.Count, 3); i++)
                {
                    Transform parent = i switch
                    {
                        0 => this.ingredient1Right,
                        1 => this.ingredient2Right,
                        2 => this.ingredient3Right,
                        _ => throw new ArgumentOutOfRangeException()
                    };

                    if (i >= ingredientsWithAmounts.Count)
                    {
                        parent.gameObject.SetActive(false);
                        continue;
                    }

                    IngredientAndAmount current = ingredientsWithAmounts[i];

                    Image image = parent.GetComponent<Image>();
                    image.color = Color.white;
                    image.sprite = current.ingredientValue.GetSprite();

                    continue;

                    GameObject instance = Instantiate(current.ingredientValue.GetPrefab(), parent);
                    instance.transform.localPosition = current.ingredientValue.GetShowcaseOffset();
                    instance.transform.localRotation = Quaternion.Euler(current.ingredientValue.GetShowcaseRotation());
                    instance.transform.localScale = Vector3.one;
                    Destroy(instance.GetComponent<Rigidbody>());
                    Destroy(instance.GetComponent<Collider>());
                    Destroy(instance.GetComponent<IngredientObject>());
                    Destroy(instance.GetComponent<InventoryItem>());
                    this.SetLayer(instance, layer);
                }
            }
        }


        private void SetLayer(GameObject obj, LayerMask layer)
        {
            obj.layer = layer;
            foreach (Transform t in obj.transform)
                this.SetLayer(t.gameObject, layer);
        }
    }
}