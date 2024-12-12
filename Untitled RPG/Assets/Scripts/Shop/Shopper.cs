using System;
using UnityEngine;

namespace RPG.Shop
{
    public class Shopper : MonoBehaviour
    {
        private Shop activeShop = null;
        
        public event Action<Shop> OnActiveShopUpdated;
        
        public void SetActiveShop(Shop shop)
        {
            activeShop = shop;
            OnActiveShopUpdated?.Invoke(activeShop);
        }
    }
}
