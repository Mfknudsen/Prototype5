using System.Linq;
using ScriptableVariables.Objects;
using UnityEngine;

namespace Potions.Effects.PersistentEffects
{
    [RequireComponent(typeof(MeshCollider))]
    public sealed class PersistentThornEffect : MonoBehaviour
    {
        [SerializeField] private float damageOnTouch;

        [SerializeField] private DamageType damageType;

        public void Trigger(float radius, float duration)
        {
            this.transform.localScale = new Vector3(radius * 2f, this.transform.localScale.y, radius * 2f);

            Destroy(this.gameObject, duration);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out PotionEffectTarget potionEffectTarget))
                return;

            //TODO Error check
            if (!potionEffectTarget.GetTargetTags().Contains(EffectTargetTag.Character))
                return;

            potionEffectTarget.GetComponent<CharacterHealth.Health>().ApplyDamageType(this.damageOnTouch, this.damageType);
        }
    }
}