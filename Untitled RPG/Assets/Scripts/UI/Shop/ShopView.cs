using System;
using RPG.UI;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace RPG.Shop.UI
{
    public class ShopView : View
    {
        [SerializeField] private InputActionReference disableAction;

        [SerializeField] private Shopper shopper = null;

        [Space] 
        [SerializeField] private TextMeshProUGUI shopName = null;

        [SerializeField] private Button closeButton = null;
        [SerializeField] private Transform rowContainer = null;
        [SerializeField] private RowUI rowPrefab = null;
 
        private Shop activeShop = null;
        
        public override void Initialize()
        {
            #region Get Shopper if not assigned in inspector

            if (shopper == null)
            {
                shopper = GameObject.FindWithTag("Player").GetComponent<Shopper>();
            }

            if (shopper == null)
            {
                Debug.LogError("No shopper found");
                return;
            }
            

            #endregion

            shopper.OnActiveShopUpdated += Shopper_OnActiveShopUpdated;
            
            closeButton?.onClick.AddListener(() =>
            {
                shopper.SetActiveShop(null);
            });

            disableAction.action.performed += _ =>
            {
                shopper.SetActiveShop(null);
            };
            
            Shopper_OnActiveShopUpdated(null);
        }

        private void OnDestroy()
        {
            shopper.OnActiveShopUpdated -= Shopper_OnActiveShopUpdated;
        }

        private void Shopper_OnActiveShopUpdated(Shop newShop)
        {
            if (activeShop != null)
            {
                activeShop.OnUpdate -= UpdateShopUI;
            }
            
            if (newShop != null)
            {
                activeShop = newShop;
                activeShop.OnUpdate += UpdateShopUI;
                UpdateShopUI();
                
                OnShowRequest?.Invoke();
                return;
            }
            
            OnHideRequest?.Invoke();
        }

        private void UpdateShopUI()
        {
            if (activeShop == null)
                return;

            shopName.text = activeShop.Name;

            RegenerateRowContainer();
        }

        private void RegenerateRowContainer()
        {
            foreach (Transform rowTransform in rowContainer)
            {
                Destroy(rowTransform.gameObject);
            }

            foreach (ShopItem shopItem in activeShop.GetFilteredItems())
            {
                RowUI rowUI = Instantiate(rowPrefab, rowContainer);
                rowUI.Setup(shopItem);
                
                rowUI.OnAddButtonClicked += () =>
                {
                    activeShop.AddToTransaction(shopItem.InventoryItemSO, +1);
                };

                rowUI.OnRemoveButtonClicked += () =>
                {
                    activeShop.AddToTransaction(shopItem.InventoryItemSO, -1);
                };
            }
        }
    }
}
