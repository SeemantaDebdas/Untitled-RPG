using System;
using System.Collections.Generic;
using RPG.Inventory.Model;
using RPG.Inventory.UI;
using UnityEngine;

namespace RPG.Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private InventorySO inventoryData = null;
        [SerializeField] private InventoryView inventoryUI = null;

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
                    return;

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

        void InventoryUI_OnStartDragging(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAtIndex(itemIndex);
            if (inventoryItem.IsNull)
                return;
            
            inventoryUI.CreateDraggedItem(inventoryItem.itemData.ItemImage, inventoryItem.quantity);
        }

        void InventoryUI_OnSwapItems(int index1, int index2)
        {
            inventoryData.SwapItems(index1, index2);
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
