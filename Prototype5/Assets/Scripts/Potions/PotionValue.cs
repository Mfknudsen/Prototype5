using System.Collections.Generic;
using Potions.Effects;

#if UNITY_EDITOR
#endif
using UnityEngine;

namespace Potions
{
    [CreateAssetMenu(fileName = "PotionValue", menuName = "Scriptable Objects/Potion Value")]
    public sealed class PotionValue : ScriptableObject
    {
        [SerializeField] private GameObject potionPrefab;

        [SerializeField] private GameObject onShatterVFX;

        [SerializeField] private Sprite potionSprite;
        
        [SerializeField] [TextArea] private string description, flavor;
        [SerializeField] private Sprite spriteOne, spriteTwo;

        [SerializeField] private Vector3 bookShowcaseRotation, bookShowcasePositionOffset;

        [SerializeField] private AudioClip onShatterSound;

        [SerializeReference]
#if UNITY_EDITOR
        [SerializeReferenceDrawer]
#endif
        private List<PotionEffectBase> effects;

        private float maxEffectDistance;

        private void OnValidate()
        {
            this.effects ??= new List<PotionEffectBase>();

            foreach (PotionEffectBase potionEffectBase in this.effects)
            {
                if (potionEffectBase == null)
                {
                    Debug.LogError("Potion Effect Is Null", this);
                    continue;
                }

                this.maxEffectDistance = Mathf.Max(this.maxEffectDistance, potionEffectBase.GetRadius());
            }
        }

        public AudioClip GetShatterSound()
        {
            return this.onShatterSound;
        }

        public GameObject GetPrefab()
        {
            return this.potionPrefab;
        }

        public IEnumerable<PotionEffectBase> GetEffects()
        {
            return this.effects;
        }

        public float GetMaxRadius()
        {
            return this.maxEffectDistance;
        }

        public string GetDescription() => this.description;

        public string GetFlavor() => this.flavor;

        public Sprite GetSpriteOne()
        {
            return this.spriteOne;
        }

        public Sprite GetSpriteTwo()
        {
            return this.spriteTwo;
        }

        public Vector3 GetShowcaseRotation()
        {
            return this.bookShowcaseRotation;
        }

        public Vector3 GetShowcaseOffset()
        {
            return this.bookShowcasePositionOffset;
        }

        public Sprite GetPotionSprite()
        {
            return this.potionSprite;
        }
    }
}