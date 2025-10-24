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

        [SerializeField] [TextArea] private string description;

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
    }
}