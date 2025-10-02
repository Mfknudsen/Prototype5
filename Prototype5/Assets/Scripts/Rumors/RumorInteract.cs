using Interactions;
using Potions;
using ScriptableVariables.Objects;
using TMPro;
using UnityEngine;

namespace Rumors
{
    public sealed class RumorInteract : MonoBehaviour, IInteractable
    {
        [SerializeField] private Rumor rumor;

        [SerializeField] private TextMeshPro floatingText;

        [SerializeField] private TransformVariable playerHandTransform;

        private bool hasGivingRumor, completed;

        private void Start()
        {
            this.floatingText.gameObject.SetActive(false);
        }

        public void OnTrigger()
        {
            if (this.completed)
                return;

            if (this.hasGivingRumor)
            {
                if (this.playerHandTransform == null || this.playerHandTransform.Value == null)
                    return;

                if (this.playerHandTransform.Value.childCount == 0)
                    return;

                PotionObject potionObject = null;
                foreach (Transform t in this.playerHandTransform.Value)
                {
                    if (t.TryGetComponent(out potionObject))
                        break;
                }

                if (potionObject == null)
                    return;

                if (!this.rumor.CheckCorrect(potionObject.GetValue()))
                    return;

                Destroy(potionObject);

                this.hasGivingRumor = false;
                this.floatingText.gameObject.SetActive(false);

                return;
            }

            this.hasGivingRumor = true;
            this.floatingText.text = this.rumor.GetDisplayText();
            this.floatingText.gameObject.SetActive(true);
        }

        public bool IsActive()
        {
            return this.enabled;
        }
    }
}