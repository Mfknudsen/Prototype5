using System.Collections.Generic;
using Potions;
using UI.Book;
using UnityEngine;

namespace UI.Page
{
    public class PotionBookLoader : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private List<PotionValue> potions;

        [SerializeField] private UIBook uiBook; // The main book controller
        [SerializeField] private GameObject potionPagePrefab; // Prefab with PotionPage.cs
        [SerializeField] private Transform pageParent; // Usually inside the Book Canvas

        private readonly List<GameObject> createdPages = new List<GameObject>();

        //private Dictionary<string, Sprite> ingredientImages = new Dictionary<string, Sprite>();
        //private const string POTION_FOLDER = "Assets/ScriptableObjects/Potions";
        //private const string INGREDIENT_IMAGE_FOLDER = "Assets/RenderTextures/Ingredients";

        private void Start()
        {
            if (this.uiBook == null)
            {
                this.uiBook = FindFirstObjectByType<UIBook>();
                if (this.uiBook == null)
                {
                    Debug.LogError("PotionBookLoader: Could not find a UIBook in the scene", this);
                    return;
                }
            }

            //this.LoadAllIngredientImages();
            this.LoadPotionPages();
        }

        private void LoadPotionPages()
        {
            //var potions = LoadAllPotionValues();

            if (this.potions == null || this.potions.Count == 0)
            {
                Debug.LogWarning("PotionBookLoader: No PotionValue assets found.");
                return;
            }

            this.createdPages.Clear();

            //foreach (PotionValue potion in this.potions)
            for (int i = 0; i < this.potions.Count; i += 2)
            {
                PotionValue potion1 = this.potions[i],
                    potion2 = i + 1 < this.potions.Count ? this.potions[i + 1] : null;
                
                if (potion1 == null) continue;

                GameObject page = Instantiate(this.potionPagePrefab, this.pageParent);
                page.name = potion1.name + (potion2 != null ? $" & {potion2.name}" : "");

                PotionPage potionPage = page.GetComponentInChildren<PotionPage>(true);
                if (potionPage == null)
                {
                    Debug.LogError($"PotionPage component not found on prefab: {this.potionPagePrefab.name}", page);
                    continue;
                }

                potionPage.SetupFromPotion(potion1, potion2);

                this.createdPages.Add(page);
            }

            // Assign generated pages to UIBook
            this.uiBook.SetPages(this.createdPages);

            Debug.Log($"PotionBookLoader: Added {this.createdPages.Count} pages to UIBook.");
        }

        /*private static PotionValue[] LoadAllPotionValues()
        {
            var guids = AssetDatabase.FindAssets("t:PotionValue", new[] { POTION_FOLDER });

            var potions = guids
                .Select(guid =>
                {
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    return AssetDatabase.LoadAssetAtPath<PotionValue>(path);
                })
                .Where(asset => asset != null)
                .ToArray();

            foreach (var p in potions)
            {
                Debug.Log(p.name);
            }

            return potions;
        }*/

        /*private void LoadAllIngredientImages()
        {
            var images = new Dictionary<string, Sprite>();

            // Find all Sprite assets in the folder
            var guids = AssetDatabase.FindAssets("t:Sprite", new[] { INGREDIENT_IMAGE_FOLDER });
            if (guids.Length == 0)
            {
                Debug.LogWarning($"No Sprites found in {INGREDIENT_IMAGE_FOLDER}");
            }

            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);

                if (sprite == null)
                    continue;

                // Strip folder path, get clean name
                var name = System.IO.Path.GetFileNameWithoutExtension(path);
                images[name] = sprite;

                Debug.Log($"Loaded ingredient sprite: {name}");
            }

            this.ingredientImages = images;
        }*/

        /*private Sprite[] GetTwoRandomIngredientImages()
        {
            if (this.ingredientImages == null || this.ingredientImages.Count < 2)
            {
                Debug.LogWarning("Not enough ingredient images to pick two unique ones.");
                return Array.Empty<Sprite>();
            }

            // Get a random list of unique sprites
            var sprites = this.ingredientImages.Values.ToList();

            int firstIndex = UnityEngine.Random.Range(0, sprites.Count);
            int secondIndex;

            // ensure sprites are unique
            do
            {
                secondIndex = UnityEngine.Random.Range(0, sprites.Count);
            } while (secondIndex == firstIndex);

            var firstSprite = sprites[firstIndex];
            var secondSprite = sprites[secondIndex];

            return new[] { firstSprite, secondSprite };
        }*/
    }
}