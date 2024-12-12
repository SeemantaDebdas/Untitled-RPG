using RPG.Inventory;
using UnityEngine;

namespace RPG.Shop
{
    public class ShopItem
    {
        private InventoryItem item;
        private int availability;
        private float price;
        private int quantityInTransaction;

        public ShopItem(InventoryItem item, int availability, float price, int quantityInTransaction)
        {
            this.item = item;
            this.availability = availability;
            this.price = price;
            this.quantityInTransaction = quantityInTransaction;
        }

        public string GetName()
        {
            return item.DisplayName;
        }

        public Sprite GetIcon()
        {
            return item.Icon;
        }

        public int GetAvailability()
        {
            return availability;
        }

        public float GetPrice()
        {
            return price;
        }
    }
}