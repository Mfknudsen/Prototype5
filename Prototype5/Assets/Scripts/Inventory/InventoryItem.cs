using Interactions;
using ScriptableVariables.SystemSpecific;
using UnityEngine;

namespace Inventory
{
    public sealed class InventoryItem : MonoBehaviour, IInteractable
    {
        [SerializeField] private InventoryItemListVariable backpack;

        [SerializeField] private string ItemName;

        public void OnTrigger()
        {
            Debug.Log($"Trigger: {this.gameObject.name}");
            this.backpack.Add(this);
            this.gameObject.SetActive(false);
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
    }
}