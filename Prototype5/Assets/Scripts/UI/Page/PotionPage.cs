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
        
        public void SetupFromPotion(PotionValue potion)
        {
            if (potion == null)
            {
                Debug.LogError("PotionPage.SetupFromPotion: potion is null", this);
                return;
            }

            // Title
            if (pageTitle != null)
                pageTitle.text = removePotionPrefix(potion.name);
          
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

        private string removePotionPrefix(string name)
        {
            const string PREFIX = "Potion";
            return name.StartsWith(PREFIX) ? name.Substring(PREFIX.Length + 1) : name;
        }
    }
}
