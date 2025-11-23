using Interactions;
using Managers;
using ScriptableVariables.SystemSpecific;
using UnityEngine;
using UnityEngine.Events;

namespace Inventory
{
    [RequireComponent(typeof(AudioSource))]
    public sealed class InventoryItem : MonoBehaviour, IInteractable
    {
        [SerializeField] private Collider itemCollider;

        [SerializeField] private Rigidbody rb;

        [SerializeField] private InventoryItemListVariable backpack;

        [SerializeField] private string itemName;

        [SerializeField] private float throwForce;

        [SerializeField] private bool throwable;

        [SerializeField] private AudioClip audioClip;

        private bool inHand;

        private UnityEvent<InventoryItem> onTrigger;

        [HideInInspector] public bool skipAttack;

        public void OnTrigger()
        {
            Debug.Log($"Trigger: {this.gameObject.name}");
            this.onTrigger?.Invoke(this);
            this.gameObject.SetActive(false);
            this.backpack.Add(this);

            AudioSource source = this.GetComponent<AudioSource>();
            source.PlayOneShot(this.audioClip);
        }

        public bool CheckAgainstPrefab(string toCheck)
        {
            return this.itemName.Equals(toCheck);
        }

        public string GetPrefabPath()
        {
            return this.itemName;
        }

        public bool IsActive()
        {
            return this.enabled;
        }

        public Vector3? Hover()
        {
            return this.transform.position;
        }

        public void SetInHand(bool set)
        {
            if (set == this.inHand)
                return;

            switch (set)
            {
                case true when !this.inHand:
                {
                    this.itemCollider.enabled = false;
                    this.rb.isKinematic = true;
                    if (this.throwable)
                        InputManager.Instance.ClickEvent.AddListener(this.OnThrowInput);
                    break;
                }
                case false when this.inHand:
                {
                    this.itemCollider.enabled = true;
                    this.rb.isKinematic = false;
                    if (this.throwable)
                        InputManager.Instance.ClickEvent.RemoveListener(this.OnThrowInput);
                    break;
                }
            }

            this.inHand = set;
        }

        private void OnThrowInput()
        {
            if (this.skipAttack)
            {
                this.skipAttack = false;
                return;
            }

            this.rb.useGravity = true;
            this.itemCollider.enabled = true;
            this.rb.isKinematic = false;
            this.rb.AddForce(this.transform.forward * this.throwForce, ForceMode.Impulse);
            this.transform.parent = null;
            this.backpack.Remove(this);

            this.inHand = false;
            InputManager.Instance.ClickEvent.RemoveListener(this.OnThrowInput);
        }

        public void AddEventListener(UnityAction<InventoryItem> action)
        {
            this.onTrigger ??= new UnityEvent<InventoryItem>();

            this.onTrigger.AddListener(action);
        }

        public void RemoveEventListener(UnityAction<InventoryItem> action)
        {
            this.onTrigger?.RemoveListener(action);
        }
    }
}