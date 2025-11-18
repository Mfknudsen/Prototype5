using System.Collections.Generic;
using Potions;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI.Book
{
    public sealed class RecipePageLoader : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private List<PotionRecipe> recipes;

        [SerializeField] private UIBook uiBook; // The main book controller

        [FormerlySerializedAs("PagePrefab")] [FormerlySerializedAs("potionPagePrefab")] [SerializeField]
        private GameObject pagePrefab; // Prefab with PotionPage.cs

        [SerializeField] private Transform pageParent; // Usually inside the Book Canvas

        private readonly List<GameObject> createdPages = new List<GameObject>();

        private void Start()
        {
            if (this.uiBook == null)
            {
                this.uiBook = FindFirstObjectByType<UIBook>();
                if (this.uiBook == null)
                {
                    Debug.LogError("RecipePageLoader: Could not find a UIBook in the scene", this);
                    return;
                }
            }

            this.LoadPotionPages();
        }

        private void LoadPotionPages()
        {
            if (this.recipes == null || this.recipes.Count == 0)
            {
                Debug.LogWarning("PotionBookLoader: No PotionValue assets found.");
                return;
            }

            this.createdPages.Clear();

            for (int i = 0; i < this.recipes.Count; i += 2)
            {
                PotionRecipe recipe1 = this.recipes[i],
                    recipe2 = i + 1 < this.recipes.Count ? this.recipes[i + 1] : null;

                GameObject page = Instantiate(this.pagePrefab, this.pageParent);
                page.name = recipe1?.name + " | " + recipe2?.name;

                RecipePage recipePage = page.GetComponentInChildren<RecipePage>(true);
                if (recipePage == null)
                {
                    Debug.LogError($"RecipePage component not found on prefab: {this.pagePrefab}", page);
                    continue;
                }

                recipePage.SetupFromRecipes(recipe1, recipe2);

                this.createdPages.Add(page);
            }

            this.uiBook.SetPages(this.createdPages);

            Debug.Log($"RecipePageLoader: Added {this.createdPages.Count} pages to UIBook.");
        }
    }
}