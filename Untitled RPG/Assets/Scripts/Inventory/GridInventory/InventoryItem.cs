using System;

namespace RPG.Inventory.Model
{
    [Serializable]
    public class InventoryItem : IEquatable<InventoryItem>
    {
        public int index;
        public int quantity;
        public InventoryItemSO itemData;

        public bool IsNull => itemData == null;

        public InventoryItem()
        {
            this.index = -1;
            this.itemData = null;
            this.quantity = 0;
        }

        public InventoryItem(InventoryItem copyItem)
        {
            this.index = copyItem.index;
            this.itemData = copyItem.itemData;
            this.quantity = copyItem.quantity;
        }

        public InventoryItem(int index, InventoryItemSO itemData, int quantity)
        {
            this.index = index;
            this.itemData = itemData;
            this.quantity = quantity;
        }
        
        public void SetQuantity(int quantity)
        {
            this.quantity = quantity;
        }

        public bool Equals(InventoryItem other)
        {
            return quantity == other.quantity && Equals(itemData, other.itemData);
        }

        public override bool Equals(object obj)
        {
            return obj is InventoryItem other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(quantity, itemData);
        }
    }
}