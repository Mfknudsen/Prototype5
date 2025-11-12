using UnityEngine;

namespace Potions
{
    [CreateAssetMenu(fileName = "IngredientValue", menuName = "Scriptable Objects/Ingredient Value")]
    public sealed class IngredientValue : ScriptableObject
    {
        [SerializeField] private GameObject ingredientPrefab;

        [SerializeField] [TextArea] private string location;

        [SerializeField] [TextArea] private string description;

        [SerializeField] private IngredientTag tag1, tag2;

        [SerializeField] private Sprite sprite;

        [SerializeField] private Vector3 bookShowcaseRotation, bookShowcasePositionOffset;

        public string GetDescription()
        {
            return this.description;
        }

        public string GetLocation()
        {
            return this.location;
        }

        public GameObject GetPrefab()
        {
            return this.ingredientPrefab;
        }

        public Sprite GetSprite()
        {
            return this.sprite;
        }

        public IngredientTag GetTag1()
        {
            return this.tag1;
        }

        public IngredientTag GetTag2()
        {
            return this.tag2;
        }

        public Vector3 GetShowcaseRotation()
        {
            return this.bookShowcaseRotation;
        }

        public Vector3 GetShowcaseOffset()
        {
            return this.bookShowcasePositionOffset;
        }
    }
}