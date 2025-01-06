using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace RPG.Inventory.Model
{
    [CreateAssetMenu(fileName = "Inventory", menuName = "Inventory/InventorySO")]
    public class InventorySO : ScriptableObject
    {
        [field: SerializeField] public List<InventoryItem> InventoryItemList { get; private set; } = new();
        [field: SerializeField] public int InventorySize { get; private set; } = 10;
        
        public event Action<Dictionary<int, InventoryItem>> OnInventoryUpdated; 

        public void Initialize()
        {
            InventoryItemList = new();
            
            for (int i = 0; i < InventorySize; i++)
            {
                InventoryItemList.Add(InventoryItem.GetEmptyItem());
            }
        }

        #region Add Item

        public void AddItem(InventoryItem inventoryItem)
        {
            AddItem(inventoryItem.itemData, inventoryItem.quantity);
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
                if (InventoryItemList[i].IsNull)
                {
                    InventoryItemList[i] = newItem;
                    return true;
                }
            }

            return false;
        }

        int AddStackableItem(InventoryItemSO item, int quantity)
        {
            for (int i = 0; i < InventoryItemList.Count; i++)
            {
                //we want to stack the item. so find the matching item
                if (InventoryItemList[i].IsNull)
                    continue;

                if (InventoryItemList[i].itemData.ItemID != item.ItemID) 
                    continue;


                int maxStackSize = InventoryItemList[i].itemData.MaxStackSize;
                int currentQuantity = InventoryItemList[i].quantity;
                int amountPossibleToTake = maxStackSize - currentQuantity;

                //if the number of items we want to insert is more than the slot can take
                if (quantity > amountPossibleToTake)
                {
                    //make the item take full stack size. 
                    InventoryItemList[i] = InventoryItemList[i].ChangeQuantity(item.MaxStackSize);
                        
                    quantity -= amountPossibleToTake;
                    break;//may not want to break from here. Get all slots that has this item and fill that. Maybe continue instead of break.
                }

                InventoryItemList[i] = InventoryItemList[i].ChangeQuantity(InventoryItemList[i].quantity + quantity);
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
        

        #endregion

        public int RemoveItem(int itemIndex, int quantityToRemove)
        {
            if (InventoryItemList.Count <= itemIndex) 
                return quantityToRemove;
            if(InventoryItemList[itemIndex].IsNull) 
                return quantityToRemove;

            if (InventoryItemList[itemIndex].quantity < quantityToRemove)
            {
                InventoryItemList[itemIndex] = InventoryItem.GetEmptyItem();
                InformAboutChange();
                
                return quantityToRemove - InventoryItemList[itemIndex].quantity;
            }
            
            int remainingQuantity = InventoryItemList[itemIndex].quantity - quantityToRemove;
            InventoryItemList[itemIndex] = InventoryItemList[itemIndex].ChangeQuantity(remainingQuantity);
            
            InformAboutChange();

            return 0;
        }

        public void RemoveItem(InventoryItemSO item, int quantityToRemove)
        {
            for (int i = 0; i < InventoryItemList.Count && quantityToRemove > 0; i++)
            {
                if(InventoryItemList[i].itemData != item)
                    continue;
                
                quantityToRemove = RemoveItem(i, quantityToRemove);
            }
        }

        public void SetItemData(int index, InventoryItem data)
        {
            InventoryItemList[index] = data;
            InformAboutChange();
        }

        public Dictionary<int, InventoryItem> GetCurrentInventoryState()
        {
            Dictionary<int, InventoryItem> inventoryState = new();

            for (int i = 0; i < InventoryItemList.Count; i++)
            {
                if (InventoryItemList[i].IsNull)
                    continue;
                
                inventoryState[i] = InventoryItemList[i];
            }
            
            return inventoryState;
        }

        public InventoryItem GetItemAtIndex(int itemIndex)
        {
            return InventoryItemList[itemIndex];
        }

        public int GetItemCount(InventoryItemSO item)
        {
            int count = 0;
            foreach (InventoryItem inventoryItem in InventoryItemList)
            {
                if (inventoryItem.itemData == item)
                {
                    count += inventoryItem.quantity;
                }
            }

            return count;
        }

        public void SwapItems(int index1, int index2)
        {
            (InventoryItemList[index1], InventoryItemList[index2]) = (InventoryItemList[index2], InventoryItemList[index1]);
            InformAboutChange();
        }

        void InformAboutChange()
        {
            OnInventoryUpdated?.Invoke(GetCurrentInventoryState());
        }
        
        bool IsInventoryFull()
        {
            return !InventoryItemList.Any(item => item.IsNull);
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
