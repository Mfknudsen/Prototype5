using Interactions;
using UnityEngine;

namespace Potions
{
    public sealed class PotionObject : MonoBehaviour
    {
        [SerializeField] private PotionValue potionValue;

        public PotionValue GetValue()
        {
            return this.potionValue;
        }
    }
}