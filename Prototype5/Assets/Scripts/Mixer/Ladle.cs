using Interactions;
using UnityEngine;

namespace Mixer
{
    public sealed class Ladle : MonoBehaviour, IInteractable
    {
        [SerializeField] private MixingStation mixingStation;

        public void OnTrigger()
        {
            this.mixingStation.TriggerMixing();
        }
    }
}