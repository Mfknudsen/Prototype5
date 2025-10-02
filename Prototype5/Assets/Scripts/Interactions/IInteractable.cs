using UnityEngine;

namespace Interactions
{
    public interface IInteractable
    {
        public void OnTrigger();

        public bool IsActive();
    }
}