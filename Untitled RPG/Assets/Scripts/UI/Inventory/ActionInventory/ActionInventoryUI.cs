using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventory.UI
{
    public class ActionInventoryUI : MonoBehaviour
    {
        [SerializeField] RectTransform inventoryItemContainer;
        [SerializeField] InventoryItemUI inventoryItemUIPrefab;
        public List<InventoryItemUI> ItemList { get; private set; } = new();
        
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
    }
}
