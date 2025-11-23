using UnityEngine;

namespace UI
{
    public sealed class PlayerHealthBar : MonoBehaviour
    {
        [SerializeField] private CharacterHealth.Health health;

        [SerializeField] private RectTransform barHolderTransform;

        private void OnEnable()
        {
            this.health.localHealthChangeAction.AddListener(this.OnHealthChange);
        }

        private void OnDisable()
        {
            this.health.localHealthChangeAction.AddListener(this.OnHealthChange);
        }

        private void OnHealthChange(float i)
        {
            this.barHolderTransform.localScale = new Vector3(i, 1, 1);
        }
    }
}