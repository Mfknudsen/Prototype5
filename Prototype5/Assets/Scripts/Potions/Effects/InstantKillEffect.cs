using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Potions.Effects
{
    [Serializable]
    public sealed class InstantKillEffect : PotionEffectBase
    {
        [SerializeField] private GameObject onHitVFX;

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

            potionObject.GetComponent<CharacterHealth.Health>().ApplyDamageType(Mathf.Infinity, null);
        }
    }
}