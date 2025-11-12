using Inventory;
using Potions;
using TMPro;
using UnityEngine;

namespace UI.Page
{
    public class IngredientPage : MonoBehaviour
    {
        [Header("Left UI References")] [SerializeField]
        private TMP_Text pageTitleLeft;

        [SerializeField] private TMP_Text descriptionLeft;
        [SerializeField] private TMP_Text locationLeft;
        [SerializeField] private TMP_Text tag1Left;
        [SerializeField] private TMP_Text tag2Left;

        [SerializeField] private Transform prefabParentLeft;

        [Header("Left UI References")] [SerializeField]
        private TMP_Text pageTitleRight;

        [SerializeField] private TMP_Text descriptionRight;
        [SerializeField] private TMP_Text locationRight;
        [SerializeField] private TMP_Text tag1Right;
        [SerializeField] private TMP_Text tag2Right;

        [SerializeField] private Transform prefabParentRight;

        public void SetupFromIngredient(IngredientValue ingredient1, IngredientValue ingredient2)
        {
            int layer = LayerMask.NameToLayer("Book UI");
            this.pageTitleLeft.text = RemoveIngredientPrefix(ingredient1.name);

            this.descriptionLeft.text = ingredient1.GetDescription();
            this.locationLeft.text = ingredient1.GetLocation();

            this.tag1Left.text = ingredient1.GetTag1().ToString();
            this.tag2Left.text = ingredient1.GetTag2().ToString();

            GameObject prefab1 = Instantiate(ingredient1.GetPrefab(), this.prefabParentLeft);
            prefab1.transform.localPosition = ingredient1.GetShowcaseOffset();
            prefab1.transform.localRotation = Quaternion.Euler(ingredient1.GetShowcaseRotation());
            Destroy(prefab1.GetComponent<Rigidbody>());
            Destroy(prefab1.GetComponent<Collider>());
            Destroy(prefab1.GetComponent<IngredientObject>());
            Destroy(prefab1.GetComponent<InventoryItem>());
            this.SetLayer(prefab1, layer);

            if (ingredient2 == null)
                return;

            this.pageTitleRight.text = RemoveIngredientPrefix(ingredient2.name);

            this.descriptionRight.text = ingredient2.GetDescription();
            this.locationRight.text = ingredient2.GetLocation();

            this.tag1Right.text = ingredient2.GetTag1().ToString();
            this.tag2Right.text = ingredient2.GetTag2().ToString();

            GameObject prefab2 = Instantiate(ingredient2.GetPrefab(), this.prefabParentRight);
            prefab2.transform.localPosition = ingredient2.GetShowcaseOffset();
            prefab2.transform.localRotation = Quaternion.Euler(ingredient2.GetShowcaseRotation());
            Destroy(prefab2.GetComponent<Rigidbody>());
            Destroy(prefab2.GetComponent<Collider>());
            Destroy(prefab2.GetComponent<IngredientObject>());
            Destroy(prefab2.GetComponent<InventoryItem>());
            this.SetLayer(prefab2, layer);
        }

        private static string RemoveIngredientPrefix(string name)
        {
            const string prefix = "Ingredient";
            return name.StartsWith(prefix) ? name.Substring(prefix.Length + 1) : name;
        }

        private void SetLayer(GameObject obj, LayerMask layer)
        {
            obj.layer = layer;
            foreach (Transform t in obj.transform)
                this.SetLayer(t.gameObject, layer);
        }
    }
}