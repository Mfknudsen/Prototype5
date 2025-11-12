using System;
using NPCs.Enemies;
using Potions.Effects.PersistentEffects;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Potions.Effects
{
    [Serializable]
    public sealed class FireEffect : PotionEffectBase
    {
        [SerializeField] private GameObject persistentGameObject;
        [SerializeField] private float effectDuration;
        [SerializeField] private int numberOfHits;

        private readonly EffectTargetTag[] includes =
        {
            EffectTargetTag.Character,
            EffectTargetTag.RigidBody
        };

        protected override EffectTargetTag[] include()
        {
            return this.includes;
        }

        protected override void Effect(PotionObject potionObject, PotionEffectTarget target)
        {
            PersistentFireEffect persistentFireEffect = Object.Instantiate(this.persistentGameObject)
                .GetComponent<PersistentFireEffect>();
            
            if (target.GetComponent<EnemyStateMachine>() is { } enemyStateMachine)
            {
                enemyStateMachine.OnFire(persistentFireEffect.GetDamagePerTick(),
                    persistentFireEffect.GetDamageType(), effectDuration, numberOfHits);
            }
        }
    }
}