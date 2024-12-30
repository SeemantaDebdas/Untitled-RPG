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
        [SerializeField] private List<ShopItemConfig> stockConfigList = new();
        public event Action OnUpdate;
        Interactable interactable;

        private Shopper activeShopper = null;
        
        Dictionary<InventoryItemSO, int> quantityInTransactionDict = new();
        Dictionary<InventoryItemSO, int> stockDict = new();

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

        public IEnumerable<ShopItem> GetFilteredItems()
        {
            return GetAllItems();
        }
        
        public IEnumerable<ShopItem> GetAllItems() 
        {
            foreach (ShopItemConfig config in stockConfigList)
            {
                float itemPrice = config.item.Price;
                itemPrice -= itemPrice * config.buyingDiscountPercentage * 0.01f;

                int transactionQuantity = 0;
                if (quantityInTransactionDict.TryGetValue(config.item, out int res))
                {
                    transactionQuantity = res;
                }
                
                yield return new ShopItem(config.item, stockDict[config.item], itemPrice, transactionQuantity);
            }
        }

        public void SelectFilter(ItemCategory category)
        {
        }

        public ItemCategory GetCategory() => ItemCategory.NONE;

        public void SelectMode(bool isBuying)
        {
        }

        public bool IsBuying()
        {
            return true;
        }

        public bool CanTransact()
        {
            // Check if Transaction is empty
            if (quantityInTransactionDict.Count == 0)
                return false;
            
            // Check if No money to buy transaction
            if (!HasSufficientFunds())
                return false;
            
            // Check if there's enough inventory space. Lecture: 1.23 & 1.24
            
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
            else if (quantityInTransactionDict[item] > stockDict[item])
            {
                quantityInTransactionDict[item] = stockDict[item];
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
                InventoryItemSO inventoryItem = shopItem.InventoryItemData;
                
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
            
            OnUpdate?.Invoke();
        }
        
        public void SetShopper(Shopper shopper)
        {
            activeShopper = shopper;
        }
    }
}
