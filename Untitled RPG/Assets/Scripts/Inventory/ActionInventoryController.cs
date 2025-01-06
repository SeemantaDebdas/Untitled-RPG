using System.Collections.Generic;
using RPG.Inventory.Model;
using RPG.Inventory.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RPG.Inventory
{
    public class ActionInventoryController : MonoBehaviour
    {
        [SerializeField] List<InventoryItem> initialActionItemList = new();
        [SerializeField] private ActionInventorySO actionInventoryData;
        [SerializeField] private ActionInventoryUI actionInventoryUI;
        
        [Header("Inputs")]
        [SerializeField] InputActionReference ability1Action;
        [SerializeField] InputActionReference ability2Action;
        [SerializeField] InputActionReference ability3Action;
        [SerializeField] InputActionReference ability4Action;
        [SerializeField] InputActionReference ability5Action;
        
        private void Start()
        {
            InitializeUI();
            InitializeData();

            UpdateInventoryItems();

            ability1Action.action.performed += _ => UseAction(1);
            ability2Action.action.performed += _ => UseAction(2);
            ability3Action.action.performed += _ => UseAction(3);
            ability4Action.action.performed += _ => UseAction(4);
            ability5Action.action.performed += _ => UseAction(5);
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

        void UseAction(int actionId)
        {
            InventoryItemSO itemData = actionInventoryData.InventoryItemList[actionId - 1].itemData;

            if (itemData == null)
            {
                print("Item Data not found");
                return;
            }
            
            ActionItemSO actionItemData = itemData as ActionItemSO;

            if (actionItemData == null)
            {
                print("Action Item Data not found");
                return;
            }
            
            actionItemData.Use(gameObject);
            
            if (actionItemData.IsConsumable)
            {
                //remove from actionInventoryData
            }
        }
    }
}
