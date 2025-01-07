using System;
using System.Collections.Generic;
using RPG.UI;
using UnityEngine;
using UnityEngine.InputSystem;

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
            
            InventoryItemUI.OnRightMouseButtonClick += InventoryItemUI_OnRightMouseButtonClick;
                
            //InventoryItemUI.OnItemBeginDrag += InventoryItemUI_OnItemBeginDrag;
            //InventoryItemUI.OnItemDroppedOn += InventoryItemUI_OnItemDroppedOn;
        }

        private void OnDestroy()
        {
            
            InventoryItemUI.OnRightMouseButtonClick -= InventoryItemUI_OnRightMouseButtonClick;
            
            //InventoryItemUI.OnItemBeginDrag -= InventoryItemUI_OnItemBeginDrag;
            //InventoryItemUI.OnItemDroppedOn -= InventoryItemUI_OnItemDroppedOn;
        }

        public void UpdateItemData(int itemIndex, Sprite itemSprite, int itemQuantity)
        {
            // if (itemIndex < 0 || itemIndex >= ItemList.Count)
            //     return;
            //
            // ItemList[itemIndex].SetData(itemSprite, itemQuantity);
        }
        

        public void CreateDraggedItem(Sprite itemSprite, int itemQuantity)
        {
            mouseFollower.Enable();
            mouseFollower.SetData(itemSprite, itemQuantity);
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
            //InventoryItemUI_OnItemClicked(itemUI);
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
            //InventoryItemUI_OnItemClicked(itemUI);
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
            // foreach (InventoryItemUI itemUI in ItemList)
            // {
            //     itemUI.Deselect();
            // }    
        }

        public void ResetAllItems()
        {
            // foreach (InventoryItemUI itemUI in ItemList)
            // {
            //     itemUI.ResetData();
            //     itemUI.Deselect();
            // }
        }
    }
}
