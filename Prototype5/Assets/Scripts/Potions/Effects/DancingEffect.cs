using System;
using DG.Tweening;
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
            NavMeshAgent agent = target.GetComponent<NavMeshAgent>();
            agent.enabled = false;
            Rigidbody rb = target.GetComponent<Rigidbody>();
            bool kinematicState = rb.isKinematic;
            rb.isKinematic = true;

            Transform transform = target.transform;
            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform
                .DOLocalJump(transform.localPosition, this.jumpHeight, this.jumpCount, this.danceDuration));
            sequence.Append(transform
                .DORotate(new Vector3(0, 360, 0), this.danceDuration, RotateMode.FastBeyond360));

            sequence.OnComplete(() =>
            {
                rb.isKinematic = kinematicState;
                agent.enabled = true;
            });
        }
    }
}