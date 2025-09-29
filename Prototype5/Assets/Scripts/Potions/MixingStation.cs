using Interactions;
using UnityEngine;

namespace Potions
{
    public class MixingStation : MonoBehaviour, IInteractable
    {
        public float OnTrigger()
        {
            return 1.5f;
        }
    }
}