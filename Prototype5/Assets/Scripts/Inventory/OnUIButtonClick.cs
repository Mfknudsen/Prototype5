using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public sealed class OnUIButtonClick : MonoBehaviour
    {
        [SerializeField] private InventoryHandler handler;

        [SerializeField] private bool isHotbar;

        [SerializeField] private int index;

        [SerializeField] private TextMeshProUGUI text;

        [SerializeField] private Image image;

        [SerializeField] private GameObject border;

        public void OnClick()
        {
            this.image.color = Color.green;
            this.handler.Click(this.isHotbar, this.index, this);
        }

        public void ResetButton()
        {
            this.image.color = Color.white;
        }

        public string GetText()
        {
            return this.text.text;
        }

        public void SetText(string set)
        {
            this.text.text = set;
        }

        public void SetIndexAndIsHotbar(bool setHotbar, int setIndex)
        {
            this.isHotbar = setHotbar;
            this.index = setIndex;
        }

        public void SetSprite(Sprite set)
        {
            this.image.sprite = set;
        }

        public void SetBorderActive()
        {
            this.border?.SetActive(true);
        }

        public void SetBorderInactive()
        {
            this.border?.SetActive(false);
        }
    }
}