using System;
using DG.Tweening;
using NPCs.Enemies;
using UnityEngine;
using UnityEngine.AI;

namespace Potions.Effects
{
    [Serializable]
    public sealed class DancingEffect : PotionEffectBase
    {
        [SerializeField] private int jumpCount;
        [SerializeField] private float jumpHeight;
        [SerializeField] private float danceDuration;

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
            if (target.GetComponent<EnemyStateMachine>() is {} enemyStateMachine)
                enemyStateMachine.OnDance(this.jumpHeight, this.jumpCount, this.danceDuration);
        }
    }
}