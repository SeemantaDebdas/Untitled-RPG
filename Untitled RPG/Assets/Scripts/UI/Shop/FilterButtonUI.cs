using System;
using RPG.Inventory.Model;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Shop.UI
{
    public class FilterButtonUI : MonoBehaviour
    {
        [SerializeField] ItemCategory itemCategory;

        private Button button = null;
        private Shop currentShop = null;

        
        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(SelectFilter);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveAllListeners();
        }

        public void SetShop(Shop shop)
        {
            currentShop = shop;
        }

        public void RefreshUI()
        {
            Debug.Assert(button != null, "Button is null");
            Debug.Assert(currentShop != null, "currentShop is null");
            
            button.interactable = currentShop.GetFilter() != itemCategory;
        }
        
        void SelectFilter()
        {
            if (currentShop == null)
            {
                Debug.LogWarning("Active shop is null", this);
                return;
            }
            currentShop.SelectFilter(itemCategory);
        }
    }
}
