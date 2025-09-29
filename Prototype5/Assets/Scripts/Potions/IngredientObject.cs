using Interactions;
using ScriptableVariables.Objects;
using UnityEngine;

namespace Potions
{
    public sealed class IngredientObject : MonoBehaviour, IInteractable
    {
        [SerializeField] private IngredientValue value;

        [SerializeField] private GameObjectVariable playerHoldingObject;

        public void OnTrigger()
        {
            if (this.playerHoldingObject.Value != null)
            {
                Transform holdingObject = this.playerHoldingObject.Value.transform;
                holdingObject.parent = null;
                holdingObject.position = this.transform.position;
                holdingObject.rotation = this.transform.rotation;
            }

            this.playerHoldingObject.Value = this.gameObject;
        }
    }
}