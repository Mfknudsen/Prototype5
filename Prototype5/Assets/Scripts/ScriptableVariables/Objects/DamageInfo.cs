using UnityEngine;

namespace ScriptableVariables.Objects
{
    [CreateAssetMenu(fileName = "Damage", menuName = "Scriptable Objects/Damage")]
    public class DamageInfo : ScriptableObject
    {
        public string nameType;
        public string description;

        [Range(-2.0f, 2.0f)] public float multiplier = 1.0f;
    }
}
