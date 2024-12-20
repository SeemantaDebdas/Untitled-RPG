using RPG.Inventory;
using RPG.Inventory.Model;
using UnityEngine;

namespace RPG.Shop
{
    public class ShopItem
    {
        public InventoryItemSO InventoryItemSO { get; private set; }
        private int availability;
        private float price;
        private int quantityInTransaction;

        public ShopItem(InventoryItemSO item, int availability, float price, int quantityInTransaction)
        {
            InventoryItemSO = item;
            this.availability = availability;
            this.price = price;
            this.quantityInTransaction = quantityInTransaction;
        }

        public string GetName()
        {
            return InventoryItemSO.DisplayName;
        }

        public Sprite GetIcon()
        {
            return InventoryItemSO.ItemImage;
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