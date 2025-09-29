using System.Collections.Generic;
using Managers;
using ScriptableVariables.Enums;
using ScriptableVariables.SystemSpecific;
using UnityEngine;

namespace Inventory
{
    public sealed class InventoryHandler : MonoBehaviour
    {
        [SerializeField] private PlayerStateVariable playerStateVariable;

        [SerializeField] private Transform handTransform;

        [SerializeField] private GameObject hotbar, backpack;

        [SerializeField] private InventoryItemListVariable backpackItems;

        [SerializeField] private MouseFollower mouseFollower;

        private List<ItemCounter> itemCounters;

        private bool clicked, isClickedHotbar;

        private int clickedIndex;

        private OnUIButtonClick buttonClicked;

        private class ItemCounter
        {
            public int Count;
            public GameObject ItemPrefab;
        }

        private void Start()
        {
            this.backpack.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;

            this.itemCounters = new List<ItemCounter>();

            this.backpackItems.Value?.Clear();

            this.mouseFollower.gameObject.SetActive(false);

            const int hotbarCount = 10, backpackCount = 50;
            for (int i = 0; i < hotbarCount + backpackCount; i++)
                this.itemCounters.Add(null);

            foreach (Transform t in this.hotbar.transform.GetChild(0))
            {
                OnUIButtonClick button = t.GetComponent<OnUIButtonClick>();
                button.SetText("");
            }

            foreach (Transform horizontalGroup in this.backpack.transform.GetChild(0))
            {
                foreach (Transform t in horizontalGroup)
                {
                    OnUIButtonClick button = t.GetComponent<OnUIButtonClick>();
                    button.SetText("");
                }
            }

            this.UpdatePlacements();
        }

        private void OnEnable()
        {
            InputManager.Instance.InventoryEvent.AddListener(this.OnInventoryInputEvent);
            this.backpackItems.AddListener(this.OnBackpackUpdate);
        }

        private void OnDisable()
        {
            InputManager.Instance.InventoryEvent.RemoveListener(this.OnInventoryInputEvent);
            this.backpackItems.RemoveListener(this.OnBackpackUpdate);
        }

        private void OnInventoryInputEvent()
        {
            if (this.backpack.activeSelf)
            {
                this.backpack.SetActive(false);

                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Confined;
                this.playerStateVariable.Value = PlayerStateEnum.Free;
            }
            else
            {
                this.backpack.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                this.playerStateVariable.Value = PlayerStateEnum.InMenu;
            }
        }

        private void OnBackpackUpdate()
        {
            for (int i = 0; i < this.itemCounters.Count; i++)
            {
                ItemCounter itemCounter = this.itemCounters[i];
                itemCounter.Count = 0;
                this.itemCounters[i] = itemCounter;
            }

            foreach (InventoryItem inventoryItem in this.backpackItems.Value)
            {
                if (inventoryItem == null)
                    continue;

                for (int index = 0; index < this.itemCounters.Count; index++)
                {
                    ItemCounter itemCounter = this.itemCounters[index];
                    if (inventoryItem.CheckAgainstPrefab(itemCounter.ItemPrefab))
                        continue;

                    itemCounter.Count++;
                    this.itemCounters[index] = itemCounter;
                }
            }

            for (int i = 0; i < this.itemCounters.Count; i++)
            {
                ItemCounter itemCounter = this.itemCounters[i];
                if (itemCounter.Count > 0)
                    continue;

                this.itemCounters.RemoveAt(i);
                i--;
            }
        }

        private void UpdatePlacements()
        {
        }

        public void Click(bool isHotbar, int index, OnUIButtonClick button)
        {
            if (this.clicked)
            {
                this.mouseFollower.gameObject.SetActive(false);

                this.clicked = false;

                this.buttonClicked.ResetButton();
                button.ResetButton();

                if (button == this.buttonClicked)
                    return;

                this.Swap(isHotbar, index, button);

                return;
            }

            this.clicked = true;

            this.isClickedHotbar = isHotbar;
            this.clickedIndex = index;

            this.mouseFollower.SetMouseFollowerValues(button);
            this.mouseFollower.gameObject.SetActive(true);

            this.buttonClicked = button;
        }

        private void Swap(bool isHotbar, int index, OnUIButtonClick button)
        {
            const int hotbarCount = 10;

            int indexA = index + (isHotbar ? 0 : hotbarCount),
                indexB = this.clickedIndex + (this.isClickedHotbar ? 0 : hotbarCount);

            (this.itemCounters[indexA], this.itemCounters[indexB]) =
                (this.itemCounters[indexB], this.itemCounters[indexA]);

            ItemCounter a = this.itemCounters[indexA],
                b = this.itemCounters[indexB];

            button.SetText(a != null ? a.ItemPrefab.name : "");

            this.buttonClicked.SetText(b != null ? b.ItemPrefab.name : "");
        }
    }
}