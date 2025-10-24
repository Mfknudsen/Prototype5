using Interactions;
using Managers;
using ScriptableVariables.SystemSpecific;
using UnityEngine;

namespace Inventory
{
    public sealed class InventoryItem : MonoBehaviour, IInteractable
    {
        [SerializeField] private Collider itemCollider;

        [SerializeField] private Rigidbody rb;

        [SerializeField] private InventoryItemListVariable backpack;

        [SerializeField] private string ItemName;

        [SerializeField] private float throwForce;

        [SerializeField] private bool throwable;

        private bool inHand;

        public void OnTrigger()
        {
            Debug.Log($"Trigger: {this.gameObject.name}");
            this.gameObject.SetActive(false);
            this.backpack.Add(this);
        }

        public bool CheckAgainstPrefab(string toCheck)
        {
            return this.ItemName.Equals(toCheck);
        }

        public string GetPrefabPath()
        {
            return this.ItemName;
        }

        public bool IsActive()
        {
            return this.enabled;
        }

        public Vector3 Hover()
        {
            return this.transform.position;
        }

        public void SetInHand(bool set)
        {
            if (set == this.inHand)
                return;

            if (set && !this.inHand)
            {
                this.itemCollider.enabled = false;
                this.rb.isKinematic = true;
                if (this.throwable)
                    InputManager.Instance.AttackEvent.AddListener(this.OnThrowInput);
            }
            else if (!set && this.inHand)
            {
                this.itemCollider.enabled = true;
                this.rb.isKinematic = false;
                if (this.throwable)
                    InputManager.Instance.AttackEvent.RemoveListener(this.OnThrowInput);
            }

            this.inHand = set;
        }

        private void OnThrowInput()
        {
            this.itemCollider.enabled = true;
            this.rb.isKinematic = false;
            this.rb.AddForce(this.transform.forward * this.throwForce, ForceMode.Impulse);
            this.transform.parent = null;
            this.backpack.Remove(this);

            this.inHand = false;
            InputManager.Instance.AttackEvent.RemoveListener(this.OnThrowInput);
        }
    }
}