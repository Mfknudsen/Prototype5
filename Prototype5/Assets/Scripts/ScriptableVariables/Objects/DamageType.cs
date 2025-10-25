using UnityEngine;

namespace ScriptableVariables.Objects
{
    [CreateAssetMenu(fileName = "DamageType", menuName = "Scriptable Objects/Damage Type")]
    public class DamageType : ScriptableObject
    {
        public string nameType;
        public string description;

        [HideInInspector] public float multiplier;
    }
}
