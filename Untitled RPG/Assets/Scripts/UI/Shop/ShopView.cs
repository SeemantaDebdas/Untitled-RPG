using System;
using System.Collections.Generic;
using RPG.UI;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
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
        
        [Space]
        [SerializeField] TextMeshProUGUI totalTransactionText = null;
        [SerializeField] Button confirmTransactionButton = null;
        
        [Space]
        [SerializeField] private Button buyModeButton = null;
        [SerializeField] private Button sellModeButton = null;

        [FormerlySerializedAs("filterButtons")]
        [Header("Filter Buttons")] 
        [SerializeField] List<FilterButtonUI> filterButtonUIList = null;
        
 
        private Shop activeShop = null;

        private bool subscribedToEvents = false;

        private Color originalTransactionTextColor;
        
        public override void Initialize()
        {
            if (shopper == null)
            {
                shopper = GameObject.FindWithTag("Player").GetComponent<Shopper>();
            }

            if (shopper == null)
            {
                Debug.LogError("No shopper found");
                return;
            }
            
            SubscribeToEvents();
            Shopper_OnActiveShopUpdated(null);
            
            //initialize field state
            originalTransactionTextColor = totalTransactionText.color;
        }

        private void SubscribeToEvents()
        {
            if (subscribedToEvents)
                return;
            
            subscribedToEvents = true;
            
            shopper.OnActiveShopUpdated += Shopper_OnActiveShopUpdated;
            
            closeButton?.onClick.AddListener(() =>
            {
                shopper.SetActiveShop(null);
            });

            disableAction.action.performed += _ =>
            {
                shopper.SetActiveShop(null);
            };
            
            confirmTransactionButton?.onClick.AddListener(() =>
            {
                activeShop.ConfirmTransaction();
            });
            
            buyModeButton?.onClick.AddListener(() => activeShop.SelectMode(true));
            sellModeButton?.onClick.AddListener(() => activeShop.SelectMode(false));
        }

        private void OnDestroy()
        {
            subscribedToEvents = false;
            
            closeButton?.onClick.RemoveAllListeners();
            confirmTransactionButton?.onClick.RemoveAllListeners();
            buyModeButton?.onClick.RemoveAllListeners();
            sellModeButton?.onClick.RemoveAllListeners();
            
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
                foreach (FilterButtonUI filterButtonUI in filterButtonUIList)
                {
                    filterButtonUI.SetShop(activeShop);
                }
                
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

            foreach (FilterButtonUI filterButtonUI in filterButtonUIList)
            {
                filterButtonUI.RefreshUI();
            }
            
            totalTransactionText.text = $"{activeShop.TransactionTotal():N2}";
            totalTransactionText.color = activeShop.HasSufficientFunds() ? originalTransactionTextColor : Color.red;
            
            confirmTransactionButton.interactable = activeShop.CanTransact();
            TextMeshProUGUI confirmText = confirmTransactionButton.GetComponentInChildren<TextMeshProUGUI>();
            confirmText.text = activeShop.IsBuying ? "Buy" : "Sell";
            
            //Because we want to be able to click the sell button if we are in buying mode and vise-versa
            buyModeButton.interactable = !activeShop.IsBuying;
            sellModeButton.interactable = activeShop.IsBuying;
            
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
                    activeShop.AddToTransaction(shopItem.InventoryItemData, +1);
                };

                rowUI.OnRemoveButtonClicked += () =>
                {
                    activeShop.AddToTransaction(shopItem.InventoryItemData, -1);
                };
            }
        }
    }
}
