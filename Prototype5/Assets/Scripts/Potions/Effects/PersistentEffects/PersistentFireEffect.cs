using System.Linq;
using Health.Conditions;
using ScriptableVariables.Objects;
using Unity.VisualScripting;
using UnityEngine;

namespace Potions.Effects.PersistentEffects
{
    [RequireComponent(typeof(SphereCollider))]
    public sealed class PersistentFireEffect : MonoBehaviour
    {
        [SerializeField] private DamageType damageType;

        [SerializeField] private SphereCollider sphereCollider;

        [SerializeField] private float damagePerTick;

        private void OnValidate()
        {
            this.sphereCollider = this.GetComponent<SphereCollider>();
        }

        public void Trigger(float radius, float duration)
        {
            Destroy(this.gameObject, duration);

            this.sphereCollider.radius = radius;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PotionEffectTarget potionEffectTarget))
                return;

            if (!potionEffectTarget.GetTargetTags().Contains(EffectTargetTag.Character))
                return;

            other.GetOrAddComponent<FireCondition>().InFire(true);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out PotionEffectTarget potionEffectTarget))
                return;

            if (!potionEffectTarget.GetTargetTags().Contains(EffectTargetTag.Character))
                return;

            FireCondition condition = other.GetOrAddComponent<FireCondition>();
            condition.InFire(false);
            condition.SetDamageType(this.damageType);
        }
    }
}