using UnityEngine;

namespace ScriptableVariables.Objects
{
    [CreateAssetMenu(fileName = "Damage", menuName = "Scriptable Objects/Damage")]
    public class DamageInfo : ScriptableObject
    {
        public string nameType;
        public string description;

        [HideInInspector] public float multiplier = 1.0f;
    }
}
