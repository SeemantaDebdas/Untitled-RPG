using System;
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

        public event Action<InventoryItem> OnItemClicked, OnItemBeginDrag, OnItemDroppedOn, OnItemEndDrag;
        
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

            actionInventoryData.OnInventoryUpdated += GridInventoryData_OnInventoryUpdated;
        }
        
        private void InitializeUI()
        {
            actionInventoryUI.Populate(actionInventoryData.InventorySize);

            actionInventoryUI.OnItemSelected += InventoryUI_OnItemSelected;
            actionInventoryUI.OnItemBeginDrag += InventoryUI_OnItemBeginDrag;
            actionInventoryUI.OnItemDroppedOn += InventoryUI_OnItemDroppedOn;
            actionInventoryUI.OnItemEndDrag += InventoryUI_OnItemEndDrag;
        }
        
        private void OnDestroy()
        {
            //DATA EVENTS
            actionInventoryData.OnInventoryUpdated -= GridInventoryData_OnInventoryUpdated;
            
            //UI EVENTS
            actionInventoryUI.OnItemSelected -= InventoryUI_OnItemSelected;
            
                        
            actionInventoryUI.OnItemBeginDrag -= InventoryUI_OnItemBeginDrag;
            actionInventoryUI.OnItemDroppedOn -= InventoryUI_OnItemDroppedOn;
            actionInventoryUI.OnItemEndDrag -= InventoryUI_OnItemEndDrag;

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
            int inventoryItemIndex = actionInventoryUI.ItemList.IndexOf(itemUI);
            
            if(inventoryItemIndex == -1)
                return new InventoryItem();
                
            var inventoryItem = actionInventoryData.GetItemAtIndex(inventoryItemIndex);
            
            return inventoryItem;
        }

        void UpdateInventoryItems()
        {
            foreach (var dictItem in actionInventoryData.GetCurrentInventoryState())
            {
                actionInventoryUI.UpdateItemData(dictItem.Key, dictItem.Value.itemData.ItemImage, dictItem.Value.quantity);
            }
        }
        
        void GridInventoryData_OnInventoryUpdated(Dictionary<int, InventoryItem> inventoryState)
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

        public void DeselectAllItems()
        {
            actionInventoryUI.DeselectAllItems();
        }
        
        public void SetItemData(InventoryItem item, InventoryItem data)
        {
            actionInventoryData.SetItemData(item, data);
        }
        
        public bool HasItem(InventoryItem item)
        {
            return actionInventoryData.HasItem(item);
        }

        public void SwapItems(InventoryItem item1, InventoryItem item2)
        {
            actionInventoryData.SwapItems(item1, item2);
        }
    }
}
