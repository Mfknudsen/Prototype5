using System.Collections.Generic;
using Managers;
using ScriptableVariables.Enums;
using ScriptableVariables.Objects;
using ScriptableVariables.SystemSpecific;
using UnityEngine;

namespace Inventory
{
    public sealed class InventoryHandler : MonoBehaviour
    {
        [SerializeField] private PlayerStateVariable playerStateVariable;

        [SerializeField] private TransformVariable handTransform;

        [SerializeField] private GameObject hotbar, backpack;

        [SerializeField] private InventoryItemListVariable backpackItems;

        [SerializeField] private MouseFollower mouseFollower;

        private List<ItemCounter> itemCounters;

        private bool clicked, isClickedHotbar;

        private int clickedIndex, hotbarIndexSelected;

        private OnUIButtonClick buttonClicked;

        private Transform currentItemInHand;

        private class ItemCounter
        {
            public int Count;
            public string ItemName;
        }

        private void Start()
        {
            this.backpack.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            this.itemCounters = new List<ItemCounter>();

            this.backpackItems.Value?.Clear();

            this.mouseFollower.gameObject.SetActive(false);

            const int hotbarCount = 10, backpackCount = 50;
            for (int i = 0; i < hotbarCount + backpackCount; i++)
                this.itemCounters.Add(null);

            int index = 0;

            foreach (Transform t in this.hotbar.transform.GetChild(0))
            {
                ItemCounter itemCounter = this.itemCounters[index];
                OnUIButtonClick button = t.GetComponent<OnUIButtonClick>();
                button.SetText(itemCounter == null ? "" : itemCounter.ItemName);
                button.SetIndexAndIsHotbar(true, index);
                index++;
            }

            foreach (Transform horizontalGroup in this.backpack.transform.GetChild(0))
            {
                foreach (Transform t in horizontalGroup)
                {
                    ItemCounter itemCounter = this.itemCounters[index];
                    OnUIButtonClick button = t.GetComponent<OnUIButtonClick>();
                    button.SetText(itemCounter == null ? "" : itemCounter.ItemName);
                    button.SetIndexAndIsHotbar(false, index - 10);
                    index++;
                }
            }

            this.OnHotbarSelectInput(1);
        }

        private void OnEnable()
        {
            InputManager.Instance.InventoryEvent.AddListener(this.OnInventoryInputEvent);
            InputManager.Instance.HotbarKey.AddListener(this.OnHotbarSelectInput);
            this.backpackItems.AddListener(this.OnBackpackUpdate);
        }

        private void OnDisable()
        {
            InputManager.Instance.InventoryEvent.RemoveListener(this.OnInventoryInputEvent);
            InputManager.Instance.HotbarKey.RemoveListener(this.OnHotbarSelectInput);
            this.backpackItems.RemoveListener(this.OnBackpackUpdate);
        }

        private void OnInventoryInputEvent()
        {
            if (this.backpack.activeSelf)
            {
                this.backpack.SetActive(false);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
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
            Debug.Log("Inventory");

            for (int i = 0; i < this.itemCounters.Count; i++)
            {
                ItemCounter itemCounter = this.itemCounters[i];
                if (itemCounter == null)
                    continue;

                itemCounter.Count = 0;
                this.itemCounters[i] = itemCounter;
            }

            foreach (InventoryItem inventoryItem in this.backpackItems.Value)
            {
                if (inventoryItem == null)
                    continue;

                bool found = false;

                for (int index = 0; index < this.itemCounters.Count; index++)
                {
                    ItemCounter itemCounter = this.itemCounters[index];
                    if (itemCounter == null)
                        continue;

                    if (!inventoryItem.CheckAgainstPrefab(itemCounter.ItemName)) continue;

                    found = true;

                    itemCounter.Count++;
                    this.itemCounters[index] = itemCounter;
                }

                if (found)
                    continue;

                for (int i = 0; i < this.itemCounters.Count; i++)
                {
                    if (this.itemCounters[i] != null)
                        continue;

                    this.itemCounters[i] = new ItemCounter
                    {
                        ItemName = inventoryItem.GetPrefabPath(),
                        Count = 1
                    };

                    break;
                }
            }

            for (int i = 0; i < this.itemCounters.Count; i++)
            {
                ItemCounter itemCounter = this.itemCounters[i];
                if (itemCounter == null)
                    continue;

                if (itemCounter.Count > 0)
                    continue;

                this.itemCounters[i] = null;
            }

            this.UpdatePlacements();
        }

        private void OnHotbarSelectInput(int input)
        {
            this.hotbarIndexSelected = input;

            if (this.handTransform.Value == null)
                return;

            if (this.currentItemInHand != null)
            {
                this.currentItemInHand.SetParent(null);
                this.currentItemInHand.gameObject.SetActive(false);
                this.currentItemInHand.GetComponent<InventoryItem>().enabled = true;
            }

            int index = input - 1;

            //Since keyboards have 0 as the far right key this will stop errors and ensure it handle as expected by the player.
            if (index < 0)
                index = 9;

            ItemCounter itemCounter = this.itemCounters[index];
            if (itemCounter == null)
                return;

            foreach (InventoryItem inventoryItem in this.backpackItems.Value)
            {
                if (inventoryItem == null)
                    continue;

                if (!inventoryItem.CheckAgainstPrefab(itemCounter.ItemName))
                    continue;

                Transform t = this.handTransform.Value;
                this.currentItemInHand = inventoryItem.transform;
                inventoryItem.transform.parent = t;
                inventoryItem.transform.SetPositionAndRotation(t.position, t.rotation);
                inventoryItem.gameObject.SetActive(true);
                inventoryItem.enabled = false;

                break;
            }
        }

        private void UpdatePlacements()
        {
            int index = 0;

            foreach (Transform t in this.hotbar.transform.GetChild(0))
            {
                ItemCounter itemCounter = this.itemCounters[index];
                OnUIButtonClick button = t.GetComponent<OnUIButtonClick>();
                button.SetText(itemCounter == null || itemCounter.Count == 0
                    ? ""
                    : $"{itemCounter.ItemName}\n{itemCounter.Count}");
                index++;
            }

            foreach (Transform horizontalGroup in this.backpack.transform.GetChild(0))
            {
                foreach (Transform t in horizontalGroup)
                {
                    ItemCounter itemCounter = this.itemCounters[index];
                    OnUIButtonClick button = t.GetComponent<OnUIButtonClick>();
                    button.SetText(itemCounter == null || itemCounter.Count == 0
                        ? ""
                        : $"{itemCounter.ItemName}\n{itemCounter.Count}");
                    index++;
                }
            }

            this.OnHotbarSelectInput(this.hotbarIndexSelected);
        }

        public void Click(bool isHotbar, int index, OnUIButtonClick button)
        {
            Debug.Log($"{isHotbar} | {index}");
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
            //this.mouseFollower.gameObject.SetActive(true);

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

            button.SetText(a != null ? a.ItemName : "");

            this.buttonClicked.SetText(b != null ? b.ItemName : "");

            this.UpdatePlacements();
        }
    }
}