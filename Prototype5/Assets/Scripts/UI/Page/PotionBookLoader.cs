using System;
using System.Collections.Generic;
using System.Linq;
using Potions;
using UI.Book;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UI.Page
{
    public class PotionBookLoader : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private UIBook uiBook;              // The main book controller
        [SerializeField] private GameObject potionPagePrefab; // Prefab with PotionPage.cs
        [SerializeField] private Transform pageParent;        // Usually inside the Book Canvas

        private readonly List<GameObject> createdPages = new();
        private Dictionary<string, Sprite> ingredientImages = new();
        private const string POTION_FOLDER = "Assets/ScriptableObjects/Potions";
        private const string INGREDIENT_IMAGE_FOLDER = "Assets/RenderTextures/Ingredients";
        
        private void Start()
        {
            if (uiBook == null)
            {
                uiBook = FindFirstObjectByType<UIBook>();
                if (uiBook == null)
                {
                    Debug.LogError("PotionBookLoader: Could not find a UIBook in the scene", this);
                    return;
                }
            }

            LoadAllIngredientImages();
            
            LoadPotionPages();
        }

        private void LoadPotionPages()
        {
            var potions = LoadAllPotionValues();

            if (potions == null || potions.Length == 0)
            {
                Debug.LogWarning("PotionBookLoader: No PotionValue assets found.");
                return;
            }

            createdPages.Clear();

            foreach (var potion in potions)
            {
                if (potion == null) continue;

                var page = Instantiate(potionPagePrefab, pageParent);
                page.name = potion.name;

                var potionPage = page.GetComponentInChildren<PotionPage>(true);
                if (potionPage == null)
                {
                    Debug.LogError($"PotionPage component not found on prefab: {potionPagePrefab.name}", page);
                    continue;
                }

                potionPage.SetupFromPotion(potion, GetTwoRandomIngredientImages());
                createdPages.Add(page);
            }

            // Assign generated pages to UIBook
            uiBook.SetPages(createdPages);

            Debug.Log($"PotionBookLoader: Added {createdPages.Count} pages to UIBook.");
        }
        
        private static PotionValue[] LoadAllPotionValues()
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
        }

        private void LoadAllIngredientImages()
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

            ingredientImages = images;
        }

        private Sprite[] GetTwoRandomIngredientImages()
        {
            if (ingredientImages == null || ingredientImages.Count < 2)
            {
                Debug.LogWarning("Not enough ingredient images to pick two unique ones.");
                return Array.Empty<Sprite>();
            }
            
            // Get a random list of unique sprites
            var sprites = ingredientImages.Values.ToList();

            int firstIndex = UnityEngine.Random.Range(0, sprites.Count);
            int secondIndex;

            // ensure sprites are unique
            do
            {
                secondIndex = UnityEngine.Random.Range(0, sprites.Count);
            } 
            while (secondIndex == firstIndex);

            var firstSprite = sprites[firstIndex];
            var secondSprite = sprites[secondIndex];
           
            return new[] { firstSprite, secondSprite };
        }
    }
}
