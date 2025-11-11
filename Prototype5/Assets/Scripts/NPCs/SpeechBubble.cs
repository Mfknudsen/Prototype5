using ScriptableVariables.Objects;
using TMPro;
using UnityEngine;

namespace NPCs
{
    public sealed class SpeechBubble : MonoBehaviour
    {
        [SerializeField] private TextMeshPro text;
        [SerializeField] private TransformVariable playerCameraTransformVariable;

        private void Update()
        {
            if (!this.playerCameraTransformVariable.Value)
                return;
            Vector3 p = this.playerCameraTransformVariable.Value.position;
            
            this.transform.LookAt(this.transform.position + (this.transform.position - p), Vector3.up);
        }

        public void SetText(string set)
        {
            this.text.text = set;
        }
    }
}