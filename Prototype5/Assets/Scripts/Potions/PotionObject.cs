using System.Collections.Generic;
using System.Linq;
using Potions.Effects;
using UnityEngine;

namespace Potions
{
    public sealed class PotionObject : MonoBehaviour
    {
        [SerializeField] private PotionValue potionValue;

        [SerializeField] private float forceForShatter;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            foreach (PotionEffectBase potionEffectBase in this.potionValue.GetEffects())
            {
                if (potionEffectBase == null || !potionEffectBase.GetDebugGizmo())
                    continue;

                Gizmos.DrawWireSphere(this.transform.position, potionEffectBase.GetRadius());
            }
        }
#endif

        public PotionValue GetValue()
        {
            return this.potionValue;
        }

        private void OnCollisionEnter(Collision other)
        {
            Vector3 collisionForce = other.impulse / Time.fixedDeltaTime;
            float magnitude = collisionForce.magnitude;

            if (magnitude < this.forceForShatter)
                return;

            List<PotionEffectTarget> hits = Physics
                .OverlapSphere(this.transform.position, this.potionValue.GetMaxRadius())
                .Select(col => col.GetComponent<PotionEffectTarget>()).Where(component => component != null).ToList();

            foreach (PotionEffectBase potionEffectBase in this.potionValue.GetEffects())
            {
                potionEffectBase.TriggerEffect(this, hits);
            }

            Destroy(this.gameObject);
        }
    }
}