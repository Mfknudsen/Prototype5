using ScriptableVariables.Objects;
using TMPro;
using UnityEngine;

namespace NPCs
{
    public sealed class SpeechBubble : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private TransformVariable playerCameraTransformVariable;

        private void Update()
        {
            if (this.playerCameraTransformVariable.Value)
                this.transform.LookAt(this.playerCameraTransformVariable.Value.position, Vector3.up);
        }

        public void SetText(string set)
        {
            this.text.text = set;
        }
    }
}