using Interactions;
using ScriptableVariables.SystemSpecific;
using UnityEngine;

namespace Inventory
{
    public sealed class InventoryItem : MonoBehaviour, IInteractable
    {
        [SerializeField] private GameObject selfPrefab;

        [SerializeField] private InventoryItemListVariable backpack;

        public float OnTrigger()
        {
            this.backpack.Add(this);
            this.gameObject.SetActive(false);

            return 0.0f;
        }

        public bool CheckAgainstPrefab(GameObject toCheck)
        {
            return this.selfPrefab.Equals(toCheck);
        }

        public GameObject GetSelfPrefab()
        {
            return this.selfPrefab;
        }
    }
}