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

        public event Action<int> OnDescriptionRequested, OnItemActionRequested, OnStartDragging;
        public event Action<int, int> OnSwapItems;
        
        private List<InventoryItemUI> itemList = new();
        private int currentlyDraggedItemIndex = -1;
        
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

                item.OnItemClicked += InventoryItemUI_OnItemClicked;
                item.OnRightMouseButtonClick += InventoryItemUI_OnRightMouseButtonClick;
                
                item.OnItemBeginDrag += InventoryItemUI_OnItemBeginDrag;
                item.OnItemEndDrag += InventoryItemUI_OnItemEndDrag;
                item.OnItemDroppedOn += InventoryItemUI_OnItemDroppedOn;
                
                item.Initialize();
                
                itemList.Add(item);
            }
        }

        private void OnDestroy()
        {
            foreach (InventoryItemUI item in itemList)
            {
                item.OnItemClicked -= InventoryItemUI_OnItemClicked;
                item.OnRightMouseButtonClick -= InventoryItemUI_OnRightMouseButtonClick;
                
                item.OnItemBeginDrag -= InventoryItemUI_OnItemBeginDrag;
                item.OnItemEndDrag -= InventoryItemUI_OnItemEndDrag;
                item.OnItemDroppedOn -= InventoryItemUI_OnItemDroppedOn;
            }
        }

        public void UpdateItemData(int itemIndex, Sprite itemSprite, int itemQuantity)
        {
            if (itemIndex < 0 || itemIndex >= itemList.Count)
                return;
            
            itemList[itemIndex].SetData(itemSprite, itemQuantity);
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
            int index = itemList.IndexOf(itemUI);
            if (index == -1)
                return;
            
            DeselectAllItems();
            itemList[index].Select();
            
            OnDescriptionRequested?.Invoke(index);
        }

        void InventoryItemUI_OnRightMouseButtonClick(InventoryItemUI itemUI)
        {
            print("Item Left clicked");
        }

        void InventoryItemUI_OnItemBeginDrag(InventoryItemUI itemUI)
        {
            int index = itemList.IndexOf(itemUI);
            if (index == -1)
                return;
            
            currentlyDraggedItemIndex = index;
            OnStartDragging?.Invoke(index);
            
            //Select item that you are dragging. Might change this later 
            InventoryItemUI_OnItemClicked(itemUI);
        }
        
        void InventoryItemUI_OnItemEndDrag(InventoryItemUI itemUI)
        {
            ResetDraggedItem();
        }
        
        void InventoryItemUI_OnItemDroppedOn(InventoryItemUI itemUI)
        {
            int index = itemList.IndexOf(itemUI);
            if (index == -1)
                return;
            
            OnSwapItems?.Invoke(currentlyDraggedItemIndex, index);
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
            currentlyDraggedItemIndex = -1;
            mouseFollower.Disable();
        }
        
        void DeselectAllItems()
        {
            foreach (InventoryItemUI itemUI in itemList)
            {
                itemUI.Deselect();
            }    
        }

        public void ResetAllItems()
        {
            foreach (InventoryItemUI itemUI in itemList)
            {
                itemUI.ResetData();
                itemUI.Deselect();
            }
        }
    }
}
