using System;
using System.Collections.Generic;
using RPG.Ability;
using RPG.Inventory.Model;
using RPG.Inventory.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RPG.Inventory
{
    public class ActionInventoryController : MonoBehaviour
    {
        [SerializeField] List<InventoryItem> initialActionItemList = new();
        [SerializeField] private ActionInventorySO inventoryData;
        [SerializeField] private ActionInventoryUI inventoryUI;
        [SerializeField] private ActionInventoryUI hudInventoryUI;
        
        [Header("Inputs")]
        [SerializeField] InputActionReference ability1Action;
        [SerializeField] InputActionReference ability2Action;
        [SerializeField] InputActionReference ability3Action;
        [SerializeField] InputActionReference ability4Action;
        [SerializeField] InputActionReference ability5Action;

        public event Action<InventoryItem> OnItemClicked, OnItemBeginDrag, OnItemDroppedOn, OnItemEndDrag;
        
        AbilityCooldownTimeHandler abilityCooldownTimeHandler;

        private void Awake()
        {
            abilityCooldownTimeHandler = GetComponent<AbilityCooldownTimeHandler>();
        }

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

            abilityCooldownTimeHandler.OnCooldownTimeChanged += AbilityCooldownTimeHandler_OnCooldownTimeChanged;
        }

        private void InitializeData()
        {
            inventoryData.Initialize();
            
            //Populate Inventory List with initial elements
            foreach (InventoryItem item in initialActionItemList)
            {
                if (item.IsNull)
                    continue;
                
                Debug.Log(item.itemData.DisplayName);

                inventoryData.AddItem(item);
            }

            inventoryData.OnInventoryUpdated += InventoryData_OnInventoryUpdated;
        }
        
        private void InitializeUI()
        {
            inventoryUI.Populate(inventoryData.InventorySize);
            hudInventoryUI.Populate(inventoryData.InventorySize);

            inventoryUI.OnItemSelected += InventoryUI_OnItemSelected;
            inventoryUI.OnItemBeginDrag += InventoryUI_OnItemBeginDrag;
            inventoryUI.OnItemDroppedOn += InventoryUI_OnItemDroppedOn;
            inventoryUI.OnItemEndDrag += InventoryUI_OnItemEndDrag;
        }
        
        private void OnDestroy()
        {
            //DATA EVENTS
            inventoryData.OnInventoryUpdated -= InventoryData_OnInventoryUpdated;
            
            //UI EVENTS
            inventoryUI.OnItemSelected -= InventoryUI_OnItemSelected;
            
                        
            inventoryUI.OnItemBeginDrag -= InventoryUI_OnItemBeginDrag;
            inventoryUI.OnItemDroppedOn -= InventoryUI_OnItemDroppedOn;
            inventoryUI.OnItemEndDrag -= InventoryUI_OnItemEndDrag;
            
            abilityCooldownTimeHandler.OnCooldownTimeChanged -= AbilityCooldownTimeHandler_OnCooldownTimeChanged;

        }

        void AbilityCooldownTimeHandler_OnCooldownTimeChanged(InventoryItem item, float time)
        {
            ActionItemUI actionItemUI = hudInventoryUI.ItemList[item.index] as ActionItemUI;

            AbilitySO abilityData = (item.itemData as AbilitySO);
            float fillAmount = time / abilityData.CooldownTime;
            
            //print($"Item name: {item.itemData.DisplayName} / Fill: {fillAmount}");
            
            actionItemUI.SetCooldownImageFillAmount(fillAmount);
        }

        #region Inventory UI Events

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
        
        #endregion
        
        InventoryItem GetItemFromItemUI(InventoryItemUI itemUI)
        {
            int inventoryItemIndex = inventoryUI.ItemList.IndexOf(itemUI);
            
            if(inventoryItemIndex == -1)
                return new InventoryItem();
                
            var inventoryItem = inventoryData.GetItemAtIndex(inventoryItemIndex);
            
            return inventoryItem;
        }

        void UpdateInventoryItems()
        {
            foreach (var dictItem in inventoryData.GetCurrentInventoryState())
            {
                inventoryUI.UpdateItemData(dictItem.Key, dictItem.Value.itemData.ItemImage, dictItem.Value.quantity);
                hudInventoryUI.UpdateItemData(dictItem.Key, dictItem.Value.itemData.ItemImage, dictItem.Value.quantity);
            }
        }
        
        void InventoryData_OnInventoryUpdated(Dictionary<int, InventoryItem> inventoryState)
        {
            inventoryUI.ResetAllItems();
            hudInventoryUI.ResetAllItems();
            UpdateInventoryItems();
        }

        void UseAction(int actionId)
        {
            InventoryItem item = inventoryData.InventoryItemList[actionId - 1];
            
            if (item.IsNull)
            {
                print("Item Data not found");
                return;
            }
            
            AbilitySO abilityItemData = item.itemData as AbilitySO;

            if (abilityItemData == null)
            {
                print("Action Item Data not found");
                return;
            }
    
            // Check cooldown or availability
            if (abilityCooldownTimeHandler == null || abilityCooldownTimeHandler.GetTimeRemainingBeforeUse(item) > 0f)
                return;

            if (!abilityItemData.TryUse(gameObject))
            {
                return;
            }
            
            abilityCooldownTimeHandler.StartCooldown(item, abilityItemData.CooldownTime);
            
            if (abilityItemData.IsConsumable)
            {
                //remove from actionInventoryData
            }
        }

        public void DeselectAllItems()
        {
            inventoryUI.DeselectAllItems();
        }
        
        public void SetItemData(InventoryItem item, InventoryItem data)
        {
            this.inventoryData.SetItemData(item, data);
        }
        
        public bool HasItem(InventoryItem item)
        {
            return inventoryData.HasItem(item);
        }

        public void SwapItems(InventoryItem item1, InventoryItem item2)
        {
            inventoryData.SwapItems(item1, item2);
        }
    }
}
