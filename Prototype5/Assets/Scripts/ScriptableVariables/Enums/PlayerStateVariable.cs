using UnityEngine;

namespace ScriptableVariables.Enums
{
    [CreateAssetMenu(fileName = "PlayerStateVariable", menuName = "SV/Enums/Player State")]
    public sealed class PlayerStateVariable : ScriptableVariable<PlayerStateEnum>
    {
    }
}