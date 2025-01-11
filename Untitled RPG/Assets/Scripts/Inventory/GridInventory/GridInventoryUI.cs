using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventory.UI
{
    public class GridInventoryUI : MonoBehaviour
    {
        [SerializeField] InventoryItemUI inventoryItemUIPrefab;
        [SerializeField] RectTransform inventoryItemContainer;
        
        public event Action<InventoryItemUI> OnItemSelected, 
            OnItemBeginDrag, 
            OnItemDroppedOn, 
            OnItemEndDrag = delegate { };

        public List<InventoryItemUI> ItemList { get; private set; } = new();

        public void Populate(int inventorySize)
        {
            foreach (RectTransform rectTransform in inventoryItemContainer)
            {
                Destroy(rectTransform.gameObject);
            }
            
            for (int i = 0; i < inventorySize; i++)
            {
                InventoryItemUI itemUI = Instantiate(inventoryItemUIPrefab, inventoryItemContainer);
                itemUI.Initialize();
                ItemList.Add(itemUI);
                
                itemUI.OnItemClicked += InventoryItemUI_OnItemClicked;
                itemUI.OnItemBeginDrag += InventoryItemUI_OnItemBeginDrag;
                itemUI.OnItemDroppedOn += InventoryItemUI_OnItemDroppedOn;
                itemUI.OnItemEndDrag += InventoryItemUI_OnItemEndDrag;
            }
            
            InventoryItemUI.OnRightMouseButtonClick += InventoryItemUI_OnRightMouseButtonClick;
        }

        private void OnDestroy()
        {
            foreach (InventoryItemUI itemUI in ItemList)
            {
                itemUI.OnItemClicked -= InventoryItemUI_OnItemClicked;
                itemUI.OnItemBeginDrag -= InventoryItemUI_OnItemBeginDrag;
                itemUI.OnItemDroppedOn -= InventoryItemUI_OnItemDroppedOn;
                itemUI.OnItemEndDrag -= InventoryItemUI_OnItemEndDrag;
            }
            
            InventoryItemUI.OnRightMouseButtonClick -= InventoryItemUI_OnRightMouseButtonClick;
        }

        public void UpdateItemData(int itemIndex, Sprite itemSprite, int itemQuantity)
        {
            if (itemIndex < 0 || itemIndex >= ItemList.Count)
                return;
            
            ItemList[itemIndex].SetData(itemSprite, itemQuantity);
        }

        void InventoryItemUI_OnItemClicked(InventoryItemUI itemUI)
        {
            OnItemSelected?.Invoke(itemUI);
        }

        void InventoryItemUI_OnRightMouseButtonClick(InventoryItemUI itemUI)
        {
            print("Item Left clicked");
        }

        void InventoryItemUI_OnItemBeginDrag(InventoryItemUI itemUI)
        {
            OnItemBeginDrag?.Invoke(itemUI);
            
            //Select item that you are dragging. Might change this later 
            InventoryItemUI_OnItemClicked(itemUI);
        }
        
        void InventoryItemUI_OnItemDroppedOn(InventoryItemUI itemUI)
        {
            OnItemDroppedOn?.Invoke(itemUI);
            
            
            //InventoryItemUI_OnItemClicked(itemUI);
        }
        
        void InventoryItemUI_OnItemEndDrag(InventoryItemUI itemUI)
        {
            OnItemEndDrag?.Invoke(itemUI);
            
            //Select item that you are dragging. Might change this later 
            //InventoryItemUI_OnItemClicked(itemUI);
        }

        public void ResetSelection()
        {
            //descriptionUI.ResetDescription();
            DeselectAllItems();
        }
        
        public void DeselectAllItems()
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
