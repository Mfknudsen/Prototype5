using Potions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Page
{
    public class PotionPage : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TMP_Text pageTitle;
        [SerializeField] private TMP_Text effectDescription;
        [SerializeField] private TMP_Text flavourDescription;
        [SerializeField] private Transform potionParent;
        [SerializeField] private Image pageBackground;
        
        [Header("Ingredients")]
        [SerializeField] private Image ingredient1;
        [SerializeField] private Image ingredient2;
        
        public void SetupFromPotion(PotionValue potion, Sprite[] images)
        {
            if (potion == null)
            {
                Debug.LogError("PotionPage.SetupFromPotion: potion is null", this);
                return;
            }

            // Title
            if (pageTitle != null)
                pageTitle.text = RemovePotionPrefix(potion.name);
          
            // Effect
            if (effectDescription != null)
                effectDescription.text = potion.GetDescription();
            effectDescription.ForceMeshUpdate(true);
            
            // Flavour @TODO: potion.GetFlavour?
            if (flavourDescription != null)
                flavourDescription.text = potion.GetDescription();
            flavourDescription.ForceMeshUpdate(true);
            
            if (pageBackground != null)
                pageBackground.enabled = pageBackground.sprite != null;

            SetPotionIngredientImages(images);
            
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

        private void SetPotionIngredientImages(Sprite[] sprites)
        {
            if (sprites == null || sprites.Length < 2)
            {
                Debug.LogWarning("PotionPage: Expected 2 ingredient sprites.");
                return;
            }

            if (ingredient1 != null && sprites[0] != null)
            {
                ingredient1.sprite = sprites[0];
                ingredient1.preserveAspect = true;
                ingredient1.color = Color.white; 
                ingredient1.gameObject.SetActive(true);
            }

            if (ingredient2 != null && sprites[1] != null)
            {
                ingredient2.sprite = sprites[1];
                ingredient2.preserveAspect = true;
                ingredient2.color = Color.white; 
                ingredient2.gameObject.SetActive(true);
            }
        }

        private string RemovePotionPrefix(string name)
        {
            const string PREFIX = "Potion";
            return name.StartsWith(PREFIX) ? name.Substring(PREFIX.Length + 1) : name;
        }
    }
}
