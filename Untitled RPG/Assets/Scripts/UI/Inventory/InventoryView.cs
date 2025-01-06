using System;
using System.Collections.Generic;
using RPG.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace RPG.Inventory.UI
{
    public class InventoryView : View
    {
        [SerializeField] private InputActionReference enableAction, disableAction;
        
        [SerializeField] InventoryItemUI inventoryItemUIPrefab;
        [SerializeField] RectTransform inventoryItemContainer;

        [Space] 
        [SerializeField] private InventoryDescriptionUI descriptionUI;
        
        [Space]
        [SerializeField] InventoryMouseFollower mouseFollower;

        public event Action<int> OnDescriptionRequested, OnItemActionRequested;
        public Action<InventoryItemUI> OnStartDragging;
        public event Action<InventoryItemUI, InventoryItemUI> OnSwapItems;

        public List<InventoryItemUI> ItemList { get; private set; } = new();
        private InventoryItemUI currentlyDraggedItem = null;
        
        public override void Initialize()
        {
            enableAction.action.performed += _ =>
            {
                OnShowRequest?.Invoke();
            };

            disableAction.action.performed += _ =>
            {
                OnHideRequest?.Invoke();
            };

            ResetSelection();
        }

        public void Populate(int inventorySize)
        {
            foreach (RectTransform rectTransform in inventoryItemContainer)
            {
                Destroy(rectTransform.gameObject);
            }
            
            for (int i = 0; i < inventorySize; i++)
            {
                InventoryItemUI item = Instantiate(inventoryItemUIPrefab, inventoryItemContainer);
                item.Initialize();
                ItemList.Add(item);
            }
            
            InventoryItemUI.OnItemClicked += InventoryItemUI_OnItemClicked;
            InventoryItemUI.OnRightMouseButtonClick += InventoryItemUI_OnRightMouseButtonClick;
                
            InventoryItemUI.OnItemBeginDrag += InventoryItemUI_OnItemBeginDrag;
            InventoryItemUI.OnItemEndDrag += InventoryItemUI_OnItemEndDrag;
            InventoryItemUI.OnItemDroppedOn += InventoryItemUI_OnItemDroppedOn;
        }

        private void OnDestroy()
        {
            InventoryItemUI.OnItemClicked -= InventoryItemUI_OnItemClicked;
            InventoryItemUI.OnRightMouseButtonClick -= InventoryItemUI_OnRightMouseButtonClick;
            
            InventoryItemUI.OnItemBeginDrag -= InventoryItemUI_OnItemBeginDrag;
            InventoryItemUI.OnItemEndDrag -= InventoryItemUI_OnItemEndDrag;
            InventoryItemUI.OnItemDroppedOn -= InventoryItemUI_OnItemDroppedOn;
        }

        public void UpdateItemData(int itemIndex, Sprite itemSprite, int itemQuantity)
        {
            if (itemIndex < 0 || itemIndex >= ItemList.Count)
                return;
            
            ItemList[itemIndex].SetData(itemSprite, itemQuantity);
        }
        
        public void UpdateDescription(int itemIndex, string itemDisplayName, string itemDescription)
        {
            descriptionUI.SetDescription(itemDisplayName, itemDescription);
        }

        public void CreateDraggedItem(Sprite itemSprite, int itemQuantity)
        {
            mouseFollower.Enable();
            mouseFollower.SetData(itemSprite, itemQuantity);
        }

        void InventoryItemUI_OnItemClicked(InventoryItemUI itemUI)
        {
            int index = ItemList.IndexOf(itemUI);
            if (index == -1)
                return;
            
            DeselectAllItems();
            ItemList[index].Select();
            
            OnDescriptionRequested?.Invoke(index);
        }

        void InventoryItemUI_OnRightMouseButtonClick(InventoryItemUI itemUI)
        {
            print("Item Left clicked");
        }

        void InventoryItemUI_OnItemBeginDrag(InventoryItemUI itemUI)
        {
            // int index = ItemList.IndexOf(itemUI);
            // if (index == -1)
            //     return;
            
            currentlyDraggedItem = itemUI;
            OnStartDragging?.Invoke(currentlyDraggedItem);
            
            //Select item that you are dragging. Might change this later 
            InventoryItemUI_OnItemClicked(itemUI);
        }
        
        void InventoryItemUI_OnItemEndDrag(InventoryItemUI itemUI)
        {
            ResetDraggedItem();
        }
        
        void InventoryItemUI_OnItemDroppedOn(InventoryItemUI itemUI)
        {
            // int index = ItemList.IndexOf(itemUI);
            // if (index == -1 || currentlyDraggedItem == null)
            //     return;
            
            //print(currentlyDraggedItemIndex + " " + index);
            
            OnSwapItems?.Invoke(currentlyDraggedItem, itemUI);
            InventoryItemUI_OnItemClicked(itemUI);
        }

        public void ResetSelection()
        {
            descriptionUI.ResetDescription();
            DeselectAllItems();
            ResetDraggedItem();
        }

        void ResetDraggedItem()
        {
            currentlyDraggedItem = null;
            mouseFollower.Disable();
        }
        
        void DeselectAllItems()
        {
            foreach (InventoryItemUI itemUI in ItemList)
            {
                itemUI.Deselect();
            }    
        }

        public void ResetAllItems()
        {
            foreach (InventoryItemUI itemUI in ItemList)
            {
                itemUI.ResetData();
                itemUI.Deselect();
            }
        }
    }
}
