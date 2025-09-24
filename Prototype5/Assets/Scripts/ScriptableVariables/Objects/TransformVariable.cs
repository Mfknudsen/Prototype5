using UnityEngine;

namespace ScriptableVariables.Objects
{
    [CreateAssetMenu(fileName = "TransformVariable", menuName = "SV/Objects/Transform")]
    public sealed class TransformVariable : ScriptableVariable<Transform>
    {
        public Vector3 Position => this.Value.position;
        public Vector3 Forward => this.Value.forward;
        public Vector3 Up => this.Value.up;
        public Vector3 Right => this.Value.right;
        public Quaternion Rotation => this.Value.rotation;
        public Vector3 Euler => this.Value.rotation.eulerAngles;

        public void Move(Vector3 v)
        {
            if (v == Vector3.zero)
                return;

            this.value.position += v;

            this.valueChanged.Invoke();
        }
    }
}