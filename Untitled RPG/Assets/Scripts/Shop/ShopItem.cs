using RPG.Inventory;
using UnityEngine;

namespace RPG.Shop
{
    public class ShopItem
    {
        public InventoryItem InventoryItem { get; private set; }
        private int availability;
        private float price;
        private int quantityInTransaction;

        public ShopItem(InventoryItem item, int availability, float price, int quantityInTransaction)
        {
            InventoryItem = item;
            this.availability = availability;
            this.price = price;
            this.quantityInTransaction = quantityInTransaction;
        }

        public string GetName()
        {
            return InventoryItem.DisplayName;
        }

        public Sprite GetIcon()
        {
            return InventoryItem.ItemImage;
        }

        public int GetAvailability()
        {
            return availability;
        }

        public float GetPrice()
        {
            return price;
        }

        public int GetQuantity()
        {
            return quantityInTransaction;
        }
    }
}