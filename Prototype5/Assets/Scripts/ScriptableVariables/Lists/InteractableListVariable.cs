using Interactions;
using UnityEngine;

namespace ScriptableVariables.Lists
{
    [CreateAssetMenu(fileName = "InteractListVariable", menuName = "SV/List/Interactable")]
    public sealed class InteractableListVariable : ListVariable<IInteractable>
    {
    }
}