using Potions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

namespace UI.Page
{
    public class PotionPage : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TMP_Text pageTitle;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private Transform potionParent;
        [SerializeField] private Image pageBackground;

        public void SetupFromPotion(PotionValue potion)
        {
            if (potion == null)
            {
                Debug.LogError("PotionPage.SetupFromPotion: potion is null", this);
                return;
            }

            // Title
            if (pageTitle != null)
                pageTitle.text = RemovePotionPrefix(potion.name);

            // Description (add a getter on PotionValue if you can; this tries a few options safely)
            if (descriptionText != null)
                descriptionText.text = potion.GetDescription();

            // Optional: background image — leave as-is if you don’t use one yet
            if (pageBackground != null)
                pageBackground.enabled = pageBackground.sprite != null;

            // Optional visual: instantiate the potion prefab under potionParent
            if (potionParent != null)
            {
                foreach (Transform child in potionParent)
                    Destroy(child.gameObject);

                var prefab = potion.GetPrefab();
                if (prefab != null)
                {
                    var instance = Instantiate(prefab, potionParent);
                    instance.transform.localPosition = Vector3.zero;
                    instance.transform.localRotation = Quaternion.identity;
                    instance.transform.localScale = Vector3.one;
                }
            }
        }

        private string RemovePotionPrefix(string originalName)
        {
            const string PREFIX = "Potion";
            return name.StartsWith(PREFIX) ? name.Substring(PREFIX.Length + 1) : name;
        }

        // Helper: try to read a description without requiring API changes.
        // Prefer adding: public string GetDescription() => description;
        private static string TryGetDescription(PotionValue potion)
        {
            // 1) Preferred: call GetDescription() if it exists
            var method = potion.GetType().GetMethod("GetDescription", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (method != null && method.ReturnType == typeof(string))
                return (string)method.Invoke(potion, null);

            // 2) Try a public field/property named "description"
            var prop = potion.GetType().GetProperty("description", BindingFlags.Instance | BindingFlags.Public);
            if (prop != null && prop.PropertyType == typeof(string))
                return (string)prop.GetValue(potion);

            var field = potion.GetType().GetField("description", BindingFlags.Instance | BindingFlags.Public);
            if (field != null && field.FieldType == typeof(string))
                return (string)field.GetValue(potion);

            // 3) Try private field (your current SO shows a private 'description')
            var privField = potion.GetType().GetField("description", BindingFlags.Instance | BindingFlags.NonPublic);
            if (privField != null && privField.FieldType == typeof(string))
                return (string)privField.GetValue(potion);

            return string.Empty;
        }
    }
}
