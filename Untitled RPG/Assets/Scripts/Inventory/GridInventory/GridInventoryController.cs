using System;
using System.Collections.Generic;
using RPG.Inventory.Model;
using RPG.Inventory.UI;
using UnityEngine;

namespace RPG.Inventory
{
    public class GridInventoryController : MonoBehaviour
    {
        [SerializeField] private GridInventorySO gridInventoryData = null;
        [SerializeField] private GridInventoryUI gridInventoryUI = null;

        [SerializeField] List<InventoryItem> initialInventoryItemList = new();

        public event Action<InventoryItem> OnItemClicked, OnItemBeginDrag, OnItemDroppedOn, OnItemEndDrag;

        private void Start()
        {
            InitializeUI();
            InitializeData();

            UpdateInventoryItems();
        }

        private void InitializeData()
        {
            gridInventoryData.Initialize();

            //Populate Inventory List with initial elements
            foreach (InventoryItem item in initialInventoryItemList)
            {
                if (item.IsNull)
                    continue;

                //Debug.Log(item.itemData.DisplayName);

                gridInventoryData.AddItem(item);
            }

            gridInventoryData.OnInventoryUpdated += GridInventoryData_OnInventoryUpdated;
        }


        void UpdateInventoryItems()
        {
            foreach (var dictItem in gridInventoryData.GetCurrentInventoryState())
            {
                gridInventoryUI.UpdateItemData(dictItem.Key, dictItem.Value.itemData.ItemImage, dictItem.Value.quantity);
            }
        }

        private void InitializeUI()
        {
            gridInventoryUI.Populate(gridInventoryData.InventorySize);

            gridInventoryUI.OnItemSelected += InventoryUI_OnItemSelected;

            gridInventoryUI.OnItemBeginDrag += InventoryUI_OnItemBeginDrag;
            gridInventoryUI.OnItemDroppedOn += InventoryUI_OnItemDroppedOn;
            gridInventoryUI.OnItemEndDrag += InventoryUI_OnItemEndDrag;
        }

        private void OnDestroy()
        {
            gridInventoryUI.OnItemSelected -= InventoryUI_OnItemSelected;

            gridInventoryUI.OnItemBeginDrag -= InventoryUI_OnItemBeginDrag;
            gridInventoryUI.OnItemDroppedOn -= InventoryUI_OnItemDroppedOn;
            gridInventoryUI.OnItemEndDrag -= InventoryUI_OnItemEndDrag;

            gridInventoryData.OnInventoryUpdated -= GridInventoryData_OnInventoryUpdated;
        }

        void InventoryUI_OnItemEndDrag(InventoryItemUI itemUI)
        {
            InventoryItem inventoryItem = GetItemFromItemUI(itemUI);
            OnItemEndDrag?.Invoke(inventoryItem);
        }

        void InventoryUI_OnItemDroppedOn(InventoryItemUI itemUI)
        {
            InventoryItem inventoryItem = GetItemFromItemUI(itemUI);
            OnItemDroppedOn?.Invoke(inventoryItem);
        }

        void InventoryUI_OnItemSelected(InventoryItemUI itemUI)
        {
            InventoryItem inventoryItem = GetItemFromItemUI(itemUI);

            if (inventoryItem.IsNull)
            {
                Debug.LogWarning("Inventory Item is Null");
                return;
            }

            OnItemClicked?.Invoke(inventoryItem);

            itemUI.Select();
        }

        void InventoryUI_OnItemBeginDrag(InventoryItemUI itemUI)
        {
            InventoryItem inventoryItem = GetItemFromItemUI(itemUI);

            if (inventoryItem.IsNull)
            {
                Debug.LogWarning("Inventory Item is Null");
                return;
            }

            OnItemBeginDrag?.Invoke(inventoryItem);
        }

        InventoryItem GetItemFromItemUI(InventoryItemUI itemUI)
        {
            int inventoryItemIndex = gridInventoryUI.ItemList.IndexOf(itemUI);

            if (inventoryItemIndex == -1)
                return new InventoryItem();

            var inventoryItem = gridInventoryData.GetItemAtIndex(inventoryItemIndex);

            return inventoryItem;
        }

        void InventoryUI_OnShowRequest()
        {
            UpdateInventoryItems();
        }

        void GridInventoryData_OnInventoryUpdated(Dictionary<int, InventoryItem> inventoryState)
        {
            gridInventoryUI.ResetAllItems();
            UpdateInventoryItems();
        }

        public void DeselectAllItems()
        {
            gridInventoryUI.DeselectAllItems();
        }

        public bool HasItem(InventoryItem item)
        {
            return gridInventoryData.HasItem(item);
        }

        public bool HasItem(InventoryItemSO item, int quantity)
        {
            return gridInventoryData.HasItem(item, quantity);
        }

        public void SwapItems(InventoryItem item1, InventoryItem item2)
        {
            gridInventoryData.SwapItems(item1, item2);
        }

        public void SetItemData(InventoryItem item, InventoryItem data)
        {
            gridInventoryData.SetItemData(item, data);
        }
    }
}
