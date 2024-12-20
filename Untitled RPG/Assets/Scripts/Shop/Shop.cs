using System;
using System.Collections.Generic;
using RPG.Core;
using RPG.Inventory;
using RPG.Inventory.Model;
using UnityEngine;
using UnityEngine.Serialization;

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
        
        Dictionary<InventoryItemSO, int> transactionDictionary = new();
        
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
            foreach (ShopItemConfig config in stockConfigList)
            {
                float itemPrice = config.item.Price;
                itemPrice -= itemPrice * config.buyingDiscountPercentage * 0.01f;

                int transactionAmount = 0;
                if (transactionDictionary.TryGetValue(config.item, out int currentTransaction))
                {
                    transactionAmount = currentTransaction;
                }
                
                yield return new ShopItem(config.item, config.initialStock, itemPrice, transactionAmount);
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
            return true;
        }
        
        public float TransactionTotal()
        {
            return 0f;
        }

        public void AddToTransaction(InventoryItemSO item, int quantity)
        {
            transactionDictionary.TryAdd(item, 0);

            transactionDictionary[item] += quantity;

            if (transactionDictionary[item] <= 0)
            {
                transactionDictionary.Remove(item);
            }
            
            OnUpdate?.Invoke();
        }

        /// <summary>
        /// 1. Transact to and from inventory
        /// 2. Cancel out the transaction
        /// 3. Credit or Debit funds
        /// </summary>
        public void ConfirmTransaction()
        {
            
        }

    }
}
