using System;
using System.Collections.Generic;
using RPG.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RPG.Inventory.UI
{
    public class InventoryView : View
    {
        [SerializeField] private InputActionReference enableAction, disableAction;
        
        [SerializeField] InventoryItemUI inventoryItemUIPrefab;
        [SerializeField] RectTransform inventoryItemContainer;

        [Space] 
        [SerializeField] private InventoryDescriptionUI descriptionUI;

        [Space] public Sprite testSprite;
        public string testTitle, testDescription;

        private List<InventoryItemUI> itemList = new();
        
        public override void Initialize()
        {
            enableAction.action.performed += _ =>
            {
                OnShowRequest?.Invoke();
            };

            disableAction.action.performed += _ =>
            {
                OnHideRequest?.Invoke();
            };
            
            descriptionUI.ResetDescription();
        }

        public void Populate(int inventorySize)
        {
            foreach (RectTransform rectTransform in inventoryItemContainer)
            {
                Destroy(rectTransform.gameObject);
            }
            
            for (int i = 0; i < inventorySize; i++)
            {
                InventoryItemUI item = Instantiate(inventoryItemUIPrefab, inventoryItemContainer);

                item.OnItemClicked += InventoryItemUI_OnItemClicked;
                item.OnRightMouseButtonClick += InventoryItemUI_OnRightMouseButtonClick;
                
                item.OnItemBeginDrag += InventoryItemUI_OnItemBeginDrag;
                item.OnItemEndDrag += InventoryItemUI_OnItemEndDrag;
                item.OnItemDroppedOn += InventoryItemUI_OnItemDroppedOn;
                
                item.Initialize();
                
                itemList.Add(item);
            }
            
            //testing
            itemList[0].SetData(testSprite, 200);
        }

        private void OnDestroy()
        {
            foreach (InventoryItemUI item in itemList)
            {
                item.OnItemClicked -= InventoryItemUI_OnItemClicked;
                item.OnRightMouseButtonClick -= InventoryItemUI_OnRightMouseButtonClick;
                
                item.OnItemBeginDrag -= InventoryItemUI_OnItemBeginDrag;
                item.OnItemEndDrag -= InventoryItemUI_OnItemEndDrag;
                item.OnItemDroppedOn -= InventoryItemUI_OnItemDroppedOn;
            }
        }

        void InventoryItemUI_OnItemClicked(InventoryItemUI itemUI)
        {
            itemList[0].Select();
            descriptionUI.SetDescription(testTitle, testDescription);
        }

        void InventoryItemUI_OnRightMouseButtonClick(InventoryItemUI itemUI)
        {
            print("Item Left clicked");
        }

        void InventoryItemUI_OnItemBeginDrag(InventoryItemUI itemUI)
        {
            print("Item Dragged");
        }
        
        void InventoryItemUI_OnItemEndDrag(InventoryItemUI itemUI)
        {
            print("Item End Drag");
        }
        
        void InventoryItemUI_OnItemDroppedOn(InventoryItemUI itemUI)
        {
            print("Item Dropped On");
        }
    }
}
