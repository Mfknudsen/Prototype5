using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public sealed class MouseFollower : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;

        [SerializeField] private Image image;

        [SerializeField] private TextMeshProUGUI text;

        [SerializeField] private RectTransform rectTransform;

        private void Update()
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(this.canvas.transform as RectTransform,
                Input.mousePosition, this.canvas.worldCamera,
                out Vector2 movePos);

            this.transform.position = this.canvas.transform.TransformPoint(movePos);
        }

        public void SetMouseFollowerValues(OnUIButtonClick buttonClick)
        {
            this.text.text = buttonClick.GetText();
        }
    }
}