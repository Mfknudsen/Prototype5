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
        [SerializeField] private TMP_Text effectTitle;
        [SerializeField] private TMP_Text effectDescription;
        [SerializeField] private TMP_Text flavorTitle;
        [SerializeField] private TMP_Text flavorDescription;
        [SerializeField] private Image pageBackground;
        [SerializeField] private Transform potionParent;
        
        
        public void SetupFromPotion(PotionValue potion)
        {
            if (potion == null)
            {
                Debug.LogError("Potion is null!");
                return;
            }

            // pageTitle.text = potion.name;
            // effectTitle.text = "Effect";
            // effectDescription.text = potion.GetEffectDescription(); // Example getter
            // flavorTitle.text = "Flavor";
            // flavorDescription.text = potion.GetFlavorDescription();
            //
            // if (potion.GetBackgroundSprite() != null)
            //     pageBackground.sprite = potion.GetBackgroundSprite();
            //
            // // If PotionValue provides a prefab:
            // if (potion.GetPrefab() != null)
            // {
            //     foreach (Transform child in potionParent)
            //         Destroy(child.gameObject);
            //
            //     GameObject potionObj = Instantiate(potion.GetPrefab(), potionParent);
            //     potionObj.transform.localPosition = Vector3.zero;
            //     potionObj.transform.localScale = Vector3.one;
            // }
        }
        
    }
}