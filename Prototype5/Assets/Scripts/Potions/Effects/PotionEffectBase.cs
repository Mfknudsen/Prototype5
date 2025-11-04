using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Potions.Effects
{
    [Serializable]
    public abstract class PotionEffectBase
    {
#if UNITY_EDITOR
        [SerializeField] private bool debugGizmo;
#endif

        [SerializeField] [Min(0)] protected float effectRadius;

        protected virtual EffectTargetTag[] include()
        {
            return Array.Empty<EffectTargetTag>();
        }

        protected virtual EffectTargetTag[] exclude()
        {
            return Array.Empty<EffectTargetTag>();
        }

        protected abstract void Effect(PotionObject potionObject, PotionEffectTarget target);

        public void TriggerEffect(PotionObject potionObject, List<PotionEffectTarget> targets)
        {
            (this as IncludeSelfEffectBase)?.TriggerSelf(potionObject);

            foreach (PotionEffectTarget potionEffectTarget in targets)
            {
                IReadOnlyList<EffectTargetTag> tags = potionEffectTarget.GetTargetTags();

                if (!this.include().Any(effectTargetTag => tags.Contains(effectTargetTag)) ||
                    this.exclude().Any(effectTargetTag => tags.Contains(effectTargetTag)))
                    continue;

                this.Effect(potionObject, potionEffectTarget);
            }
        }

#if UNITY_EDITOR
        public bool GetDebugGizmo()
        {
            return this.debugGizmo;
        }
#endif

        public float GetRadius()
        {
            return this.effectRadius;
        }
    }
}