using System.Collections.Generic;
using System.Linq;
using Potions;
using UI.Book;
using UnityEngine;

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

        private void Start()
        {
            if (uiBook == null)
            {
                uiBook = FindAnyObjectByType<UIBook>();
                if (uiBook == null)
                {
                    Debug.LogError("PotionBookLoader: Could not find a UIBook in the scene.", this);
                    return;
                }
            }

            LoadPotionPages();
        }

        private void LoadPotionPages()
        {
            PotionValue[] potions = LoadAllPotionValues();

            if (potions == null || potions.Length == 0)
            {
                Debug.LogWarning("PotionBookLoader: No PotionValue assets found.");
                return;
            }

            createdPages.Clear();

            foreach (var potion in potions)
            {
                if (potion == null) continue;

                GameObject page = Instantiate(potionPagePrefab, pageParent);
                page.name = potion.name;

                PotionPage potionPage = page.GetComponentInChildren<PotionPage>(true);
                if (potionPage == null)
                {
                    Debug.LogError($"PotionPage component not found on prefab: {potionPagePrefab.name}", page);
                    continue;
                }

                potionPage.SetupFromPotion(potion);
                createdPages.Add(page);
            }

            // Assign generated pages to UIBook
            uiBook.SetPages(createdPages);

            Debug.Log($"PotionBookLoader: Added {createdPages.Count} pages to UIBook.");
        }

        private static PotionValue[] LoadAllPotionValues()
        {
            string folder = "Assets/ScriptableObjects/Potions";
            string[] guids = AssetDatabase.FindAssets("t:PotionValue", new[] { folder });

            var potions = guids
                .Select(guid =>
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
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
    }
}
