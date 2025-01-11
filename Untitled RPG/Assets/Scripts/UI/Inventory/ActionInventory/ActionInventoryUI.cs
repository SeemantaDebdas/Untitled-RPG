using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventory.UI
{
    public class ActionInventoryUI : MonoBehaviour
    {
        [SerializeField] RectTransform inventoryItemContainer;
        [SerializeField] InventoryItemUI inventoryItemUIPrefab;
        public List<InventoryItemUI> ItemList { get; private set; } = new();
        
        public event Action<InventoryItemUI> OnItemSelected, 
            OnItemBeginDrag, 
            OnItemDroppedOn, 
            OnItemEndDrag = delegate { };
        
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

            //InventoryItemUI.OnItemClicked += InventoryItemUI_OnItemClicked;
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
            InventoryItemUI_OnItemClicked(itemUI);
            //itemUI.Select();
            OnItemBeginDrag?.Invoke(itemUI);
            //Select item that you are dragging. Might change this later 
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

        public void UpdateItemData(int itemIndex, Sprite itemSprite, int itemQuantity)
        {
            if (itemIndex < 0 || itemIndex >= ItemList.Count)
                return;
            
            ItemList[itemIndex].SetData(itemSprite, itemQuantity);
        }
        
        public void ResetAllItems()
        {
            foreach (InventoryItemUI itemUI in ItemList)
            {
                itemUI.ResetData();
                itemUI.Deselect();
            }
        }
        
        public void DeselectAllItems()
        {
            foreach (InventoryItemUI itemUI in ItemList)
            {
                itemUI.Deselect();
            }    
        }
    }
}
