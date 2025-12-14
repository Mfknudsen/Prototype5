using ScriptableVariables.Objects;
using UnityEngine;

namespace DayNightCycle
{
    public class DirectionalLight : MonoBehaviour
    {
        [SerializeField] private TransformVariable lightTransformVariable;
        
        private void Awake()
        {
            this.lightTransformVariable.Value = this.transform;
        }
    }
}
