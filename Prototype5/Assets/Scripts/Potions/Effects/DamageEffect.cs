using System;
using ScriptableVariables.Objects;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Potions.Effects
{
    [Serializable]
    public sealed class DamageEffect : PotionEffectBase
    {
        [SerializeField] private GameObject onHitVFX;

        [SerializeField, Min(0)] private float damage;

        [SerializeField] private DamageType damageType;

        private readonly EffectTargetTag[] includes =
            {
                EffectTargetTag.Character
            },
            excludes =
            {
                EffectTargetTag.Player
            };

        protected override EffectTargetTag[] include()
        {
            return this.includes;
        }

        protected override EffectTargetTag[] exclude()
        {
            return this.excludes;
        }

        protected override void Effect(PotionObject potionObject, PotionEffectTarget target)
        {
            if (this.onHitVFX != null)
            {
                Transform transform = Object.Instantiate(this.onHitVFX).transform;
                transform.position = target.transform.position;
            }

            potionObject.GetComponent<CharacterHealth.Health>().ApplyDamageType(this.damage, this.damageType);
        }
    }
}