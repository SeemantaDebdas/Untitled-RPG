using System;
using System.Collections.Generic;
using RPG.Core;
using RPG.Inventory;
using UnityEngine;

namespace RPG.Shop
{
    public class Shop : MonoBehaviour
    {
        [field: SerializeField] public string Name { get; private set; }
        [SerializeField] List<InventoryItem> items = new List<InventoryItem>();
        public event Action OnUpdate;
        Interactable interactable;
        
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
            yield return new ShopItem(items[0], 10, 10f, 0);
            yield return new ShopItem(items[1], 2, 100f, 0);
            yield return new ShopItem(items[2], 50, 50f, 0);
            yield return new ShopItem(items[3], 1, 10001f, 0);
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

        public void AddToTransaction(InventoryItem item, float amount)
        {
            
        }

        public void ConfirmTransaction()
        {
        }

    }
}
