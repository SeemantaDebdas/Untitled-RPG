using System;
using RPG.Inventory.Model;
using UnityEngine;

namespace RPG.Shop
{
    public class Shopper : MonoBehaviour
    {
        [field: SerializeField] public InventorySO Inventory { get; private set; }
        
        public event Action<Shop> OnActiveShopUpdated;
        private Shop activeShop = null;
        
        public void SetActiveShop(Shop shop)
        {
            if (activeShop != null)
            {
                activeShop.SetShopper(null);
            }
            
            activeShop = shop;

            if (activeShop != null)
            {
                activeShop.SetShopper(this);
            }
            
            OnActiveShopUpdated?.Invoke(activeShop);
        }
    }
}
