using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Potions.Effects
{
    public sealed class PotionEffectTarget : MonoBehaviour
    {
        [SerializeField] private List<EffectTargetTag> tags;

        private void OnValidate()
        {
            this.tags ??= new List<EffectTargetTag>();

            for (int i = 0; i < this.tags.Count; i++)
            {
                for (int j = i + 1; j < this.tags.Count; j++)
                {
                    if (this.tags[i] == this.tags[j])
                        Debug.LogError($"There are multiply of the same tag: {this.tags[i]}");
                }

                if (this.tags[i] == EffectTargetTag.Character && this.GetComponent<NavMeshAgent>() == null)
                    Debug.LogError("No NavMeshAgent component for target with tag Character", this);
                else if (this.tags[i] == EffectTargetTag.RigidBody && this.GetComponent<Rigidbody>() == null)
                    Debug.LogError("No Rigidbody component for target with tag RigidBody", this);
            }
        }

        public IReadOnlyList<EffectTargetTag> GetTargetTags()
        {
            return this.tags;
        }
    }
}