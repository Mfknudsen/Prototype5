using System;
using Potions.Effects.PersistentEffects;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Potions.Effects
{
    [Serializable]
    public sealed class FireEffect : IncludeSelfEffectBase
    {
        [SerializeField] private GameObject persistentGameObject;
        [SerializeField] private float effectDuration;

        private readonly EffectTargetTag[] excludes =
        {
            EffectTargetTag.All
        };

        protected override EffectTargetTag[] exclude()
        {
            return this.excludes;
        }

        protected override void Effect(PotionObject potionObject, PotionEffectTarget target)
        {
            //Fire will be handled by spawned gameobject
        }

        public override void TriggerSelf(PotionObject potionObject)
        {
            PersistentFireEffect persistentFireEffect = Object.Instantiate(this.persistentGameObject)
                .GetComponent<PersistentFireEffect>();

            persistentFireEffect.transform.position = potionObject.transform.position;
            persistentFireEffect.Trigger(this.effectRadius, this.effectDuration);
        }
    }
}