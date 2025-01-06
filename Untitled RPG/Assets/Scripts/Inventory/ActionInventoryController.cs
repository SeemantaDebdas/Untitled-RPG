using System.Collections.Generic;
using RPG.Inventory.Model;
using RPG.Inventory.UI;
using UnityEngine;

namespace RPG.Inventory
{
    public class ActionInventoryController : MonoBehaviour
    {
        [SerializeField] List<InventoryItem> initialActionItemList = new();
        [SerializeField] private ActionInventorySO actionInventoryData;
        [SerializeField] private ActionInventoryUI actionInventoryUI;
        
        private void Start()
        {
            InitializeUI();
            InitializeData();

            UpdateInventoryItems();
        }

        private void InitializeData()
        {
            actionInventoryData.Initialize();
            
            //Populate Inventory List with initial elements
            foreach (InventoryItem item in initialActionItemList)
            {
                if (item.IsNull)
                    continue;
                
                Debug.Log(item.itemData.DisplayName);

                actionInventoryData.AddItem(item);
            }

            actionInventoryData.OnInventoryUpdated += InventoryData_OnInventoryUpdated;
        }
        
        private void OnDestroy()
        {
            actionInventoryData.OnInventoryUpdated -= InventoryData_OnInventoryUpdated;
        }
        
        private void InitializeUI()
        {
            actionInventoryUI.Populate(actionInventoryData.InventorySize);
        }
        
        void UpdateInventoryItems()
        {
            foreach (var dictItem in actionInventoryData.GetCurrentInventoryState())
            {
                actionInventoryUI.UpdateItemData(dictItem.Key, dictItem.Value.itemData.ItemImage, dictItem.Value.quantity);
            }
        }
        
        void InventoryData_OnInventoryUpdated(Dictionary<int, InventoryItem> inventoryState)
        {
            actionInventoryUI.ResetAllItems();
            UpdateInventoryItems();
        }
    }
}
