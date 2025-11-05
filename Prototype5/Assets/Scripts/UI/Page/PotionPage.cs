using Potions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Page
{
    public class PotionPage : MonoBehaviour
    {
        [Header("Left UI References")] [SerializeField]
        private TMP_Text pageTitleLeft;

        [SerializeField] private TMP_Text effectDescriptionLeft;
        [SerializeField] private TMP_Text flavourDescriptionLeft;
        [SerializeField] private Transform potionParentLeft;
        [SerializeField] private Image pageBackgroundLeft;

        [Header("Ingredients")] [SerializeField]
        private Image ingredient1Left;

        [SerializeField] private Image ingredient2Left;

        [Header("Right UI References")] [SerializeField]
        private TMP_Text pageTitleRight;

        [SerializeField] private TMP_Text effectDescriptionRight;
        [SerializeField] private TMP_Text flavourDescriptionRight;
        [SerializeField] private Transform potionParentRight;
        [SerializeField] private Image pageBackgroundRight;

        [Header("Ingredients")] [SerializeField]
        private Image ingredient1Right;

        [SerializeField] private Image ingredient2Right;

        public void SetupFromPotion(PotionValue potion1, PotionValue potion2)
        {
            if (potion1 != null)
            {
                Debug.Log(potion1.name);
                // Title
                if (this.pageTitleLeft != null) this.pageTitleLeft.text = RemovePotionPrefix(potion1.name);

                // Effect
                if (this.effectDescriptionLeft != null) this.effectDescriptionLeft.text = potion1.GetDescription();
                this.effectDescriptionLeft.ForceMeshUpdate(true);

                // Flavour @TODO: potion.GetFlavour?
                if (this.flavourDescriptionLeft != null) this.flavourDescriptionLeft.text = potion1.GetDescription();
                this.flavourDescriptionLeft.ForceMeshUpdate(true);

                if (this.pageBackgroundLeft != null)
                    this.pageBackgroundLeft.enabled = this.pageBackgroundLeft.sprite != null;

                this.SetPotionIngredientImagesLeft(potion1);

                if (this.potionParentLeft != null)
                {
                    foreach (Transform child in this.potionParentLeft)
                        Destroy(child.gameObject);

                    GameObject prefab = potion1.GetPrefab();

                    if (prefab != null)
                    {
                        GameObject instance = Instantiate(prefab, this.potionParentLeft);
                        instance.transform.localPosition = Vector3.zero;
                        instance.transform.localRotation = Quaternion.identity;
                        instance.transform.localScale = Vector3.one;
                    }
                }
            }
            else
            {
                Debug.LogError("PotionPage.SetupFromPotion: potion is null", this);
            }

            if (potion2 != null)
            {
                // Title
                if (this.pageTitleRight != null) this.pageTitleRight.text = RemovePotionPrefix(potion2.name);

                // Effect
                if (this.effectDescriptionRight != null) this.effectDescriptionRight.text = potion2.GetDescription();
                this.effectDescriptionRight.ForceMeshUpdate(true);

                // Flavour @TODO: potion.GetFlavour?
                if (this.flavourDescriptionRight != null) this.flavourDescriptionRight.text = potion2.GetDescription();
                this.flavourDescriptionRight.ForceMeshUpdate(true);

                if (this.pageBackgroundRight != null)
                    this.pageBackgroundRight.enabled = this.pageBackgroundRight.sprite != null;

                this.SetPotionIngredientImagesRight(potion2);

                if (this.potionParentRight != null)
                {
                    foreach (Transform child in this.potionParentRight)
                        Destroy(child.gameObject);

                    GameObject prefab = potion2.GetPrefab();

                    if (prefab != null)
                    {
                        GameObject instance = Instantiate(prefab, this.potionParentRight);
                        instance.transform.localPosition = Vector3.zero;
                        instance.transform.localRotation = Quaternion.identity;
                        instance.transform.localScale = Vector3.one;
                    }
                }
            }
            else
            {
                this.pageTitleRight.text = "";
                this.effectDescriptionRight.text = "";
                this.flavourDescriptionRight.text = "";
                //Debug.LogError("PotionPage.SetupFromPotion: potion is null", this);
            }
        }

        private void SetPotionIngredientImagesLeft(PotionValue potion)
        {
            if (potion == null)
            {
                Debug.LogWarning("PotionPage: Expected 2 ingredient sprites.");
                return;
            }

            if (this.ingredient1Left != null && potion.GetSpriteOne() != null)
            {
                this.ingredient1Left.sprite = potion.GetSpriteOne();
                this.ingredient1Left.preserveAspect = true;
                this.ingredient1Left.color = Color.white;
                this.ingredient1Left.gameObject.SetActive(true);
            }

            if (this.ingredient2Left != null && potion.GetSpriteTwo() != null)
            {
                this.ingredient2Left.sprite = potion.GetSpriteTwo();
                this.ingredient2Left.preserveAspect = true;
                this.ingredient2Left.color = Color.white;
                this.ingredient2Left.gameObject.SetActive(true);
            }
        }

        private void SetPotionIngredientImagesRight(PotionValue potion)
        {
            if (potion == null)
            {
                Debug.LogWarning("PotionPage: Expected 2 ingredient sprites.");
                return;
            }

            if (this.ingredient1Right != null && potion.GetSpriteOne() != null)
            {
                this.ingredient1Right.sprite = potion.GetSpriteOne();
                this.ingredient1Right.preserveAspect = true;
                this.ingredient1Right.color = Color.white;
                this.ingredient1Right.gameObject.SetActive(true);
            }

            if (this.ingredient2Right != null && potion.GetSpriteTwo() != null)
            {
                this.ingredient2Right.sprite = potion.GetSpriteTwo();
                this.ingredient2Right.preserveAspect = true;
                this.ingredient2Right.color = Color.white;
                this.ingredient2Right.gameObject.SetActive(true);
            }
        }

        private static string RemovePotionPrefix(string name)
        {
            const string PREFIX = "Potion";
            return name.StartsWith(PREFIX) ? name.Substring(PREFIX.Length + 1) : name;
        }
    }
}