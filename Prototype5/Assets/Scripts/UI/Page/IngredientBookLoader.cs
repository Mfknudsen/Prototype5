using System.Collections.Generic;
using Potions;
using UI.Book;
using UnityEngine;

namespace UI.Page
{
    public sealed class IngredientBookLoader : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private List<IngredientValue> ingredients;

        [SerializeField] private UIBook uiBook;
        [SerializeField] private GameObject ingredientPagePrefab;
        [SerializeField] private Transform pageParent;

        private readonly List<GameObject> createdPages = new List<GameObject>();

        private void Start()
        {
            this.LoadIngredientPages();
        }

        private void LoadIngredientPages()
        {
            if (this.ingredients == null || this.ingredients.Count == 0)
            {
                Debug.LogWarning("IngredientBookLoader: No IngredientValue assets found.");
                return;
            }

            this.createdPages.Clear();

            for (int i = 0; i < this.ingredients.Count; i += 2)
            {
                IngredientValue ingredient1 = this.ingredients[i],
                    ingredient2 = i + 1 < this.ingredients.Count ? this.ingredients[i + 1] : null;

                if (ingredient1 == null)
                    continue;

                GameObject page = Instantiate(this.ingredientPagePrefab, this.pageParent);

                IngredientPage ingredientPage = page.GetComponent<IngredientPage>();

                ingredientPage.SetupFromIngredient(ingredient1, ingredient2);

                this.createdPages.Add(page);
            }

            this.uiBook.SetPages(this.createdPages);

            Debug.Log($"IngredientBookLoader: Added {this.createdPages.Count} pages to UIBook.");
        }
    }
}