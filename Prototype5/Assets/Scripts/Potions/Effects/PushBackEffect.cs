using System;
using UnityEngine;

namespace Potions.Effects
{
    [Serializable]
    public sealed class PushBackEffect : PotionEffectBase
    {
        [SerializeField] [Min(0)] private float forceStrength;

        private readonly EffectTargetTag[] includes = {
            EffectTargetTag.RigidBody
        };
        
        protected override EffectTargetTag[] include()
        {
            return this.includes;
        }

        protected override void Effect(PotionObject potionObject, PotionEffectTarget target)
        {
            Debug.Log("Push back");
            // target.GetComponent<Rigidbody>()
            //     .AddExplosionForce(this.forceStrength, potionObject.transform.position, this.effectRadius);
        }
    }
}