using Interactions;
using UnityEngine;

namespace Potions
{
    public sealed class PotionObject : MonoBehaviour, IInteractable
    {
        [SerializeField] private PotionValue potionValue;

        public float OnTrigger()
        {
            return 0.0f;
        }
    }
}