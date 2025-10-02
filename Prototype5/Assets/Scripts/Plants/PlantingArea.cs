using Interactions;
using Inventory;
using ScriptableVariables.Objects;
using ScriptableVariables.SystemSpecific;
using UnityEngine;

namespace Plants
{
    public sealed class PlantingArea : MonoBehaviour, IInteractable
    {
        [SerializeField] private TransformVariable handTransform;

        [SerializeField] private InventoryItemListVariable itemListVariable;

        private SphereCollider collider;

        private void Start()
        {
            this.collider = this.GetComponent<SphereCollider>();
        }

        public void OnTrigger()
        {
            if (this.handTransform.Value.childCount == 0)
                return;

            Seed seed = null;
            foreach (Transform t in this.handTransform.Value)
            {
                if (t.TryGetComponent(out seed))
                    break;
            }

            if (seed == null)
                return;

            this.collider.enabled = false;

            Transform seedTransform = Instantiate(seed.GetPrefab()).transform;
            seedTransform.parent = this.transform;
            seedTransform.position = this.transform.position;
            seedTransform.rotation = this.transform.rotation;

            this.itemListVariable.Remove(seed.GetComponent<InventoryItem>());
            Destroy(seed.gameObject);
        }
    }
}