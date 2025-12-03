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

        public PotionValue GetValue() => this.potionValue;

        private void OnCollisionEnter(Collision other)
        {
            Vector3 collisionForce = other.impulse / Time.fixedDeltaTime;
            float magnitude = collisionForce.magnitude;
            
            if (magnitude < this.forceForShatter)
                return;

            AudioClip sound = this.potionValue.GetShatterSound();

            if (sound)
            {
                GameObject soundObject = new GameObject();
                soundObject.AddComponent<AudioSource>().PlayOneShot(sound);
                Destroy(soundObject, sound.length + 0.01f);
            }

            List<PotionEffectTarget> hits = new List<PotionEffectTarget>();

            foreach (PotionEffectTarget potionEffectTarget in FindObjectsByType<PotionEffectTarget>(FindObjectsSortMode
                         .None))
            {
                if (potionEffectTarget.GetComponent<SphereCollider>() is not { } c)
                    continue;

                
                float distance = Vector3.Distance(this.transform.position,
                    potionEffectTarget.transform.position + c.center);

                if (this.potionValue.GetMaxRadius() + c.radius < distance)
                    continue;

                hits.Add(potionEffectTarget);
            }
            
            // if (hits.Count == 0)
            // {
            //     bool isThornPotion = false;
            //     foreach (PotionEffectBase potionEffectBase in this.potionValue.GetEffects())
            //         if (potionEffectBase is ThornEffect)
            //             isThornPotion = true;
            //     
            //     if (!isThornPotion) 
            //         return;
            // }

            foreach (PotionEffectBase potionEffectBase in this.potionValue.GetEffects())
            {
                potionEffectBase.TriggerEffect(this, hits);
            }

            Destroy(this.gameObject);
        }

        public Sprite GetSprite()
        {
            return this.potionValue.GetPotionSprite();
        }
    }
}