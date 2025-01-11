using System;
using RPG.Inventory.Model;
using RPG.Inventory.UI;
using UnityEngine;

namespace RPG.Inventory
{
    public class InventoryMediator : MonoBehaviour
    {
        [SerializeField] private GridInventoryController gridInventoryController = null;
        [SerializeField] private ActionInventoryController actionInventoryController = null;
        
        [Header("UI Elements")]
        [SerializeField] private InventoryDescriptionUI descriptionUI;
        [SerializeField] private InventoryMouseFollower mouseFollower = null;

        InventoryItem draggedItem;

        void OnEnable()
        {
            gridInventoryController.OnItemClicked += InventoryController_OnItemClicked;
            gridInventoryController.OnItemEndDrag += InventoryController_OnItemEndDrag;
            gridInventoryController.OnItemBeginDrag += InventoryController_OnItemBeginDrag;
            gridInventoryController.OnItemDroppedOn += GridInventoryController_OnItemDroppedOn;
            
            actionInventoryController.OnItemClicked += InventoryController_OnItemClicked;
            actionInventoryController.OnItemEndDrag += InventoryController_OnItemEndDrag;
            actionInventoryController.OnItemBeginDrag += InventoryController_OnItemBeginDrag;
            actionInventoryController.OnItemDroppedOn += ActionInventoryController_OnItemDroppedOn;
        }


        void OnDestroy()
        {
            gridInventoryController.OnItemClicked -= InventoryController_OnItemClicked;
            gridInventoryController.OnItemEndDrag -= InventoryController_OnItemEndDrag;
            gridInventoryController.OnItemBeginDrag -= InventoryController_OnItemBeginDrag;
            gridInventoryController.OnItemDroppedOn += GridInventoryController_OnItemDroppedOn;
            
            actionInventoryController.OnItemClicked -= InventoryController_OnItemClicked;
            actionInventoryController.OnItemEndDrag -= InventoryController_OnItemEndDrag;
            actionInventoryController.OnItemBeginDrag -= InventoryController_OnItemBeginDrag;
            actionInventoryController.OnItemDroppedOn -= ActionInventoryController_OnItemDroppedOn;
        }


        void InventoryController_OnItemClicked(InventoryItem item)
        {
            actionInventoryController.DeselectAllItems();
            gridInventoryController.DeselectAllItems();
            
            descriptionUI.SetDescription(item.itemData.name, item.itemData.Description);
        }
        
        void InventoryController_OnItemBeginDrag(InventoryItem item)
        {
            draggedItem = item;
            mouseFollower.Enable();
            mouseFollower.SetData(item.itemData.ItemImage, item.quantity);
        }
        
        void GridInventoryController_OnItemDroppedOn(InventoryItem itemDroppedOn)
        {
            if (!gridInventoryController.HasItem(draggedItem))
            {
                //dragged item belongs to action inventory
                if (itemDroppedOn.IsNull || itemDroppedOn.itemData is ActionItemSO)
                {
                    gridInventoryController.SetItemData(itemDroppedOn, new(draggedItem));
                    actionInventoryController.SetItemData(draggedItem, new(itemDroppedOn));
                    
                    return;
                }

            }
            
            gridInventoryController.SwapItems(draggedItem, itemDroppedOn);
        }
        
        void ActionInventoryController_OnItemDroppedOn(InventoryItem itemDroppedOn)
        {
            if (!actionInventoryController.HasItem(draggedItem))
            {
                //dragged item belongs to grid inventory
                if (draggedItem.itemData is ActionItemSO)
                {
                    actionInventoryController.SetItemData(itemDroppedOn, new(draggedItem));
                    gridInventoryController.SetItemData(draggedItem, new(itemDroppedOn));
                    
                    return;
                }
            }
            
            actionInventoryController.SwapItems(draggedItem, itemDroppedOn);
        }
        
        void InventoryController_OnItemEndDrag(InventoryItem item)
        {
            draggedItem = null;
            mouseFollower.Disable();
        }
    }
}
