using UnityEngine;

namespace ScriptableVariables.Objects
{
    [CreateAssetMenu(fileName = "GameObjectVariable", menuName = "SV/Objects/GameObject")]
    public sealed class GameObjectVariable : ScriptableVariable<GameObject>
    {
    }
}