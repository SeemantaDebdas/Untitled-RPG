using System;
using System.Collections.Generic;
using RPG.Core;
using RPG.Inventory;
using RPG.Inventory.Model;
using UnityEngine;

namespace RPG.Shop
{
    public class Shop : MonoBehaviour
    {
        [Serializable]
        class ShopItemConfig
        {
            public InventoryItemSO item;
            public int initialStock;
            [Range(0, 100)]
            public float buyingDiscountPercentage;
        }
        
        [field: SerializeField] public string Name { get; private set; }
        [SerializeField, Range(0, 100)] float sellingPercentage;
        [SerializeField] private List<ShopItemConfig> stockConfigList = new();
        public event Action OnUpdate;
        Interactable interactable;

        private Shopper activeShopper = null;
        
        Dictionary<InventoryItemSO, int> quantityInTransactionDict = new();
        Dictionary<InventoryItemSO, int> stockDict = new();

        private ItemCategory currentCategoryFilter = ItemCategory.NONE;

        private void Awake()
        {
            foreach (ShopItemConfig shopItemConfig in stockConfigList)
            {
                stockDict[shopItemConfig.item] = shopItemConfig.initialStock;
            }
        }

        private void OnEnable()
        {
            if(interactable == null) 
                interactable = GetComponent<Interactable>();

            interactable.OnInteract += Interactable_OnInteract;
        }

        private void OnDestroy()
        {
            interactable.OnInteract -= Interactable_OnInteract;
        }

        private void Interactable_OnInteract(Interactor interactor)
        {
            if (interactor.TryGetComponent(out Shopper shopper))
            {
                shopper.SetActiveShop(this);
            }
        }

        public bool IsBuying { get; private set; } = true;

        public IEnumerable<ShopItem> GetFilteredItems()
        {
            foreach (ShopItem shopItem in GetAllItems())
            {
                ItemCategory itemCategory = shopItem.InventoryItemData.Category;
                if(itemCategory == currentCategoryFilter || currentCategoryFilter == ItemCategory.NONE)
                    yield return shopItem;
            }
        }
        
        public IEnumerable<ShopItem> GetAllItems() 
        {
            foreach (ShopItemConfig config in stockConfigList)
            {
                float itemPrice = GetItemPrice(config);

                int transactionQuantity = 0;
                if (quantityInTransactionDict.TryGetValue(config.item, out int res))
                {
                    transactionQuantity = res;
                }
                
                yield return new ShopItem(config.item, GetAvailability(config.item), itemPrice, transactionQuantity);
            }
        }

        public void SelectFilter(ItemCategory category)
        {
            currentCategoryFilter = category;
            //print(category.ToString());
            
            OnUpdate?.Invoke();
        }

        public ItemCategory GetFilter() => currentCategoryFilter;

        public void SelectMode(bool isBuying)
        {
            IsBuying = isBuying;
            OnUpdate?.Invoke();
        }

        public bool CanTransact()
        {
            // Check if Transaction is empty
            if (quantityInTransactionDict.Count == 0)
                return false;
            
            if (IsBuying)
            {
                // Check if No money to buy transaction
                if (!HasSufficientFunds())
                    return false;
                
                // Check if there's enough inventory space. Lecture: 1.23 & 1.24
            }
            
            return true;
        }

        public bool HasSufficientFunds()
        {
            CoinPurse coinPurse = activeShopper.GetComponent<CoinPurse>();

            if (coinPurse == null)
                return false;
            
            //print($"Current Balance: {coinPurse.GetBalance()}/ Transaction Price: {priceInTransaction}");
            if (coinPurse.GetBalance() >= TransactionTotal())
                return true;

            return false;
        }

        public float TransactionTotal()
        {
            float transactionTotal = 0;
            
            foreach (ShopItem shopItem in GetAllItems())
            {
                transactionTotal += shopItem.GetQuantityInTransaction() * shopItem.GetPrice();    
            }
            
            return transactionTotal;
        }

        public void AddToTransaction(InventoryItemSO item, int toAddQuantity)
        {
            quantityInTransactionDict.TryAdd(item, 0);
            
            quantityInTransactionDict[item] += toAddQuantity;

            if (quantityInTransactionDict[item] <= 0)
            {
                quantityInTransactionDict.Remove(item);
            }
            else
            {
                int availability = GetAvailability(item);
                if (quantityInTransactionDict[item] > availability)
                {
                    quantityInTransactionDict[item] = availability;
                }
            }

            OnUpdate?.Invoke();
        }

        /// <summary>
        /// Confirm by buying or selling
        /// 1. Transact to and from inventory
        /// 2. Cancel out the transaction
        /// 3. Credit or Debit funds
        /// </summary>
        public void ConfirmTransaction()
        {
            if (activeShopper == null)
            {
                Debug.LogWarning("No shopper set. Please select a shopper.");
                return;
            }

            InventorySO inventory = activeShopper.Inventory;
            CoinPurse coinPurse = activeShopper.GetComponent<CoinPurse>();

            if (inventory == null || coinPurse == null)
            {
                Debug.LogWarning("No inventory or purse set. Please set inventory or purse.");
                return;
            }
            
            foreach (ShopItem shopItem in GetAllItems())
            {
                if (IsBuying)
                {
                    BuyItem(shopItem, coinPurse);
                }
                else
                {
                    SellItem(shopItem, inventory, coinPurse);
                }
            }
            
            OnUpdate?.Invoke();
        }

        void BuyItem(ShopItem shopItem, CoinPurse coinPurse)
        {
            InventoryItemSO inventoryItem = shopItem.InventoryItemData;
            InventorySO inventory = activeShopper.Inventory;
            
            int quantity = shopItem.GetQuantityInTransaction();
            int quantityThatCanBeBought = (int)coinPurse.GetBalance() / (int)shopItem.GetPrice();

            int toAdd = Math.Min(quantity, quantityThatCanBeBought);
            int notAddedQuantity = inventory.AddItem(inventoryItem, toAdd);
            int addedQuantity = toAdd - notAddedQuantity;

            print($"Quantity: {quantity}. Q That can be bought: {quantityThatCanBeBought}. To Add: {toAdd}");
            print($"To Debit: {addedQuantity * shopItem.GetPrice()}");

            AddToTransaction(inventoryItem, -addedQuantity);
            stockDict[inventoryItem] -= addedQuantity;
            coinPurse.DebitBalance(addedQuantity * shopItem.GetPrice());
        }

        void SellItem(ShopItem shopItem, InventorySO inventory, CoinPurse coinPurse)
        {
            //Do this one by one as this is dependent on the quantity in inventory
            int quantityToSell = shopItem.GetQuantityInTransaction();
            
            AddToTransaction(shopItem.InventoryItemData, -quantityToSell);

            inventory.RemoveItem(shopItem.InventoryItemData, quantityToSell);
            stockDict[shopItem.InventoryItemData] += quantityToSell;
            
            coinPurse.CreditBalance(quantityToSell * shopItem.GetPrice());
        }

        public void SetShopper(Shopper shopper)
        {
            activeShopper = shopper;
        }
        
        private int GetAvailability(InventoryItemSO item)
        {
            if(IsBuying)
                return stockDict[item];

            return GetItemCountInInventory(item);
        }

        private int GetItemCountInInventory(InventoryItemSO item)
        {
            InventorySO inventory = activeShopper.Inventory;

            if (inventory == null)
                return 0;

            return inventory.GetItemCount(item);
        }

        float GetItemPrice(ShopItemConfig config)
        {
            float itemPrice = config.item.Price;
            
            if(IsBuying)
                itemPrice -= itemPrice * config.buyingDiscountPercentage * 0.01f;
            else
                itemPrice = itemPrice * sellingPercentage * 0.01f;

            return itemPrice;
        }
    }
}
