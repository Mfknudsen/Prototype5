using Inventory;
using UnityEngine;

namespace ScriptableVariables.SystemSpecific
{
    [CreateAssetMenu(fileName = "InventoryItemListVariable", menuName = "SV/System Specific/Inventory Item List")]
    public sealed class InventoryItemListVariable : ListVariable<InventoryItem>
    {
    }
}