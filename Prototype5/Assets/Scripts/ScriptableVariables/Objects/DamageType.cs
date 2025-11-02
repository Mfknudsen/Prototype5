using UnityEngine;

namespace ScriptableVariables.Objects
{
    [CreateAssetMenu(fileName = "DamageType", menuName = "Scriptable Objects/Damage Type")]
    public class DamageType : ScriptableObject
    {
        [SerializeField] [TextArea] private string description;
    }
}