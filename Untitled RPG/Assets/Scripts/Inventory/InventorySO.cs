using System;
using System.Collections.Generic;
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

        public void AddItem(InventoryItemSO item, int quantity)
        {
            for (int i = 0; i < inventoryItemList.Count; i++)
            {
                if (inventoryItemList[i].IsNull)
                {
                    inventoryItemList[i] = new InventoryItem
                    {
                        itemData = item,
                        quantity = quantity
                    };

                    return;
                }
            }
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
    }

    [System.Serializable]
    public struct InventoryItem
    {
        public int quantity;
        public InventoryItemSO itemData;

        public bool IsNull => itemData == null;

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
