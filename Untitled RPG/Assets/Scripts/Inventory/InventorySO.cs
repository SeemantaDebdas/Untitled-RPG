using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Inventory.Model
{
    [CreateAssetMenu(fileName = "Inventory", menuName = "Inventory/InventorySO")]
    public class InventorySO : ScriptableObject
    {
        [SerializeField] List<InventoryItem> inventoryItemList = new();
        [field: SerializeField] public int InventorySize { get; private set; } = 10;
        
        public event Action<Dictionary<int, InventoryItem>> OnInventoryUpdated; 

        public void Initialize()
        {
            inventoryItemList = new();
            
            for (int i = 0; i < InventorySize; i++)
            {
                inventoryItemList.Add(InventoryItem.GetEmptyItem());
            }
        }

        public int AddItem(InventoryItemSO item, int quantity)
        {
            int remainingQuantity = quantity;
            
            if (item.IsStackable)
            {
                remainingQuantity = AddStackableItem(item, quantity);
            }
            else
            {
                while (remainingQuantity > 0 && AddItemToFirstEmptySlot(item, 1))
                {
                    remainingQuantity--;
                }
            }

            InformAboutChange();
            return remainingQuantity;
        }

        bool AddItemToFirstEmptySlot(InventoryItemSO item, int itemQuantity)
        {
            InventoryItem newItem = new(item, itemQuantity);

            for (int i = 0; i < InventorySize; i++)
            {
                if (inventoryItemList[i].IsNull)
                {
                    inventoryItemList[i] = newItem;
                    return true;
                }
            }

            return false;
        }

        int AddStackableItem(InventoryItemSO item, int quantity)
        {
            for (int i = 0; i < inventoryItemList.Count; i++)
            {
                //we want to stack the item. so find the matching item
                if (inventoryItemList[i].IsNull)
                    continue;

                if (inventoryItemList[i].itemData.ItemID != item.ItemID) 
                    continue;


                int maxStackSize = inventoryItemList[i].itemData.MaxStackSize;
                int currentQuantity = inventoryItemList[i].quantity;
                int amountPossibleToTake = maxStackSize - currentQuantity;

                //if the number of items we want to insert is more than the slot can take
                if (quantity > amountPossibleToTake)
                {
                    //make the item take full stack size. 
                    inventoryItemList[i] = inventoryItemList[i].ChangeQuantity(item.MaxStackSize);
                        
                    quantity -= amountPossibleToTake;
                    break;
                }

                inventoryItemList[i] = inventoryItemList[i].ChangeQuantity(inventoryItemList[i].quantity + quantity);
                InformAboutChange();
                return 0;
            }

            // Use empty slots for remaining quantity
            while (quantity > 0 && AddItemToFirstEmptySlot(item, Mathf.Min(quantity, item.MaxStackSize)))
            {
                quantity -= Mathf.Min(quantity, item.MaxStackSize);
            }
            
            InformAboutChange();
            return quantity;
        }

        public Dictionary<int, InventoryItem> GetCurrentInventoryState()
        {
            Dictionary<int, InventoryItem> inventoryState = new();

            for (int i = 0; i < inventoryItemList.Count; i++)
            {
                if (inventoryItemList[i].IsNull)
                    continue;
                
                inventoryState[i] = inventoryItemList[i];
            }
            
            return inventoryState;
        }

        public InventoryItem GetItemAtIndex(int itemIndex)
        {
            return inventoryItemList[itemIndex];
        }

        public void AddItem(InventoryItem inventoryItem)
        {
            AddItem(inventoryItem.itemData, inventoryItem.quantity);
        }

        public void SwapItems(int index1, int index2)
        {
            (inventoryItemList[index1], inventoryItemList[index2]) = (inventoryItemList[index2], inventoryItemList[index1]);
            InformAboutChange();
        }

        private void InformAboutChange()
        {
            OnInventoryUpdated?.Invoke(GetCurrentInventoryState());
        }
        
        bool IsInventoryFull()
        {
            return !inventoryItemList.Any(item => item.IsNull);
        }
    }

    [System.Serializable]
    public struct InventoryItem
    {
        public int quantity;
        public InventoryItemSO itemData;

        public bool IsNull => itemData == null;

        public InventoryItem(InventoryItemSO itemData, int quantity)
        {
            this.itemData = itemData;
            this.quantity = quantity;
        }
        
        public InventoryItem ChangeQuantity(int newQuantity)
        {
            return new InventoryItem
            {
                itemData = this.itemData,
                quantity = newQuantity
            };
        }

        public static InventoryItem GetEmptyItem()
        {
            return new InventoryItem
            {
                itemData = null,
                quantity = 0
            };
        }
    }
}
