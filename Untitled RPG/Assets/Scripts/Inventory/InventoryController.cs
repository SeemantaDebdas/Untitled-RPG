using System;
using System.Collections.Generic;
using RPG.Inventory.Model;
using RPG.Inventory.UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace RPG.Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private InventorySO inventoryData = null;
        [SerializeField] private InventoryView inventoryUI = null;
        
        [Header("Action Items")] 
        [SerializeField] private ActionInventorySO actionInventoryData = null;
        [SerializeField] private ActionInventoryUI actionInventoryUI = null;

        [SerializeField] List<InventoryItem> initialInventoryItemList = new();

        //bring the enable disable functionality here

        private void Start()
        {
            InitializeUI();
            InitializeData();

            UpdateInventoryItems();
        }

        private void InitializeData()
        {
            inventoryData.Initialize();
            
            //Populate Inventory List with initial elements
            foreach (InventoryItem item in initialInventoryItemList)
            {
                if (item.IsNull)
                    continue;
                
                Debug.Log(item.itemData.DisplayName);

                inventoryData.AddItem(item);
            }

            inventoryData.OnInventoryUpdated += InventoryData_OnInventoryUpdated;
        }


        void UpdateInventoryItems()
        {
            foreach (var dictItem in inventoryData.GetCurrentInventoryState())
            {
                inventoryUI.UpdateItemData(dictItem.Key, dictItem.Value.itemData.ItemImage, dictItem.Value.quantity);
            }
        }

        private void OnDestroy()
        {
            inventoryUI.OnDescriptionRequested -= InventoryUI_OnDescriptionRequested;
            inventoryUI.OnItemActionRequested -= InventoryUI_OnItemActionRequested;
            inventoryUI.OnStartDragging -= InventoryUI_OnStartDragging;
            inventoryUI.OnSwapItems -= InventoryUI_OnSwapItems;
            
            inventoryUI.OnShowRequest -= InventoryUI_OnShowRequest;
            inventoryData.OnInventoryUpdated -= InventoryData_OnInventoryUpdated;
        }

        private void InitializeUI()
        {
            inventoryUI.Populate(inventoryData.InventorySize);

            inventoryUI.OnDescriptionRequested += InventoryUI_OnDescriptionRequested;
            inventoryUI.OnItemActionRequested += InventoryUI_OnItemActionRequested;
            inventoryUI.OnStartDragging += InventoryUI_OnStartDragging;
            inventoryUI.OnSwapItems += InventoryUI_OnSwapItems;

            inventoryUI.OnShowRequest += InventoryUI_OnShowRequest;
        }

        #region Inventory UI Events
        
        void InventoryUI_OnDescriptionRequested(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAtIndex(itemIndex);

            if (inventoryItem.IsNull)
            {
                inventoryUI.ResetSelection();
                return;
            }
            
            inventoryUI.UpdateDescription(itemIndex, inventoryItem.itemData.DisplayName, inventoryItem.itemData.Description);
        }
        
        void InventoryUI_OnItemActionRequested(int itemIndex)
        {
            
        }

        void InventoryUI_OnStartDragging(InventoryItemUI inventoryItemUI)
        {
            InventoryItem inventoryItem;
            if (inventoryItemUI is ActionItemUI actionItemUI)
            {
                int actionItemIndex= actionInventoryUI.ItemList.IndexOf(actionItemUI);
                inventoryItem = actionInventoryData.InventoryItemList[actionItemIndex];
            }
            else
            {
                int inventoryItemIndex = inventoryUI.ItemList.IndexOf(inventoryItemUI);
                inventoryItem = inventoryData.GetItemAtIndex(inventoryItemIndex);
            }

            if (inventoryItem.IsNull)
            {
                Debug.LogWarning("Inventory Item is Null");
                return;
            }
            
            inventoryUI.CreateDraggedItem(inventoryItem.itemData.ItemImage, inventoryItem.quantity);
        }

        void InventoryUI_OnSwapItems(InventoryItemUI item1, InventoryItemUI item2)
        {
            // Check if either item is an ActionItem
            bool isItem1Action = item1 is ActionItemUI;
            bool isItem2Action = item2 is ActionItemUI;

            if (isItem1Action && isItem2Action)
            {
                // Both items are Action Items - handle swapping within the action inventory
                int actionItemIndex1 = actionInventoryUI.ItemList.IndexOf(item1);
                int actionItemIndex2 = actionInventoryUI.ItemList.IndexOf(item2);

                actionInventoryData.SwapItems(actionItemIndex1, actionItemIndex2);
                print("Both items are action items and have been swapped.");
                return;
            }

            if (isItem1Action || isItem2Action)
            {
                // Handle cross-swapping between Action and Regular Inventory
                ActionItemUI actionItemUI = isItem1Action ? (ActionItemUI)item1 : (ActionItemUI)item2;
                InventoryItemUI inventoryItemUI = isItem1Action ? item2 : item1;

                int actionItemIndex = actionInventoryUI.ItemList.IndexOf(actionItemUI);
                int inventoryItemIndex = inventoryUI.ItemList.IndexOf(inventoryItemUI);

                InventoryItem actionItem = actionInventoryData.GetItemAtIndex(actionItemIndex);
                InventoryItem inventoryItem = inventoryData.GetItemAtIndex(inventoryItemIndex);

                // Perform the swap
                actionInventoryData.SetItemData(actionItemIndex, inventoryItem);
                inventoryData.SetItemData(inventoryItemIndex, actionItem);

                print($"Swapped an action item with a regular item: ActionItemIndex {actionItemIndex}, InventoryItemIndex {inventoryItemIndex}.");
                return;
            }

            // Handle regular inventory item swapping
            inventoryData.SwapItems(
                inventoryUI.ItemList.IndexOf(item1),
                inventoryUI.ItemList.IndexOf(item2)
            );

            print("Swapped two regular inventory items.");
        }
        
        void InventoryUI_OnShowRequest()
        {
            UpdateInventoryItems();
        }
        
        #endregion
        
        void InventoryData_OnInventoryUpdated(Dictionary<int, InventoryItem> inventoryState)
        {
            inventoryUI.ResetAllItems();
            UpdateInventoryItems();
        }

    }
}
