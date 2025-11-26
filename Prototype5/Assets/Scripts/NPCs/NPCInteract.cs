using Interactions;
using Rumors;
using ScriptableVariables.Objects;
using UnityEngine;

namespace NPCs
{
    public sealed class NPCInteract : MonoBehaviour, IInteractable
    {
        [SerializeField] private TransformVariable playerTransformVariable;

        [SerializeField] private Vector3 interactHighlightOffset;

        [SerializeField] private SpeechBubble speechBubble;

        [SerializeField] private Dialog defaultDialog;

        [SerializeField] private float speechShowDistance = 5f;

        private Dialog currentDialog;

        private NPCInteractBase currentInteractBase;

        private void Start()
        {
            if (this.GetComponent<Collider>() == null)
                Debug.LogError("A collider is needed for the interact handling", this);

            if (this.currentDialog != null)
                return;

            this.currentDialog = this.defaultDialog;
            this.speechBubble.SetText(this.defaultDialog != null ? this.defaultDialog.text : "");
        }

        private void Update()
        {
            if (!this.playerTransformVariable.Value)
            {
                this.speechBubble.gameObject.SetActive(false);
                return;
            }

            //this.speechBubble.gameObject.SetActive(true);

            bool inRange = Vector2.Distance(
                new Vector2(this.transform.position.x, this.transform.position.z),
                this.playerTransformVariable.XZ) <= this.speechShowDistance;

            if (this.speechBubble.gameObject.activeSelf != inRange)
                this.speechBubble.gameObject.SetActive(inRange);
        }

        public void OnTrigger()
        {
            this.currentInteractBase?.Trigger(this);
        }

        public bool IsActive()
        {
            return this.gameObject.activeSelf && this.currentInteractBase != null;
        }

        public Vector3? Hover()
        {
            return this.transform.position + this.interactHighlightOffset;
        }
        
        public string GetInteractName()
        {
            return this.gameObject.name;
        }

        public void SetDialog(Dialog set)
        {
            this.currentDialog = !set ? this.defaultDialog : set;

            this.speechBubble.SetText(this.currentDialog.text);
        }

        public void SetCurrentTrigger(NPCInteractBase set)
        {
            this.currentInteractBase = set;
            set.DefaultSet(this);
        }
    }

    public abstract class NPCInteractBase
    {
        public abstract void Trigger(NPCInteract npc);

        public abstract void DefaultSet(NPCInteract npc);
    }
}