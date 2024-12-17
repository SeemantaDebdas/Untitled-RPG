using System;
using RPG.Inventory.UI;
using UnityEngine;

namespace RPG.Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private InventoryView view = null;
        [SerializeField] int inventorySize = 10;
        
        //bring the enable disable functionality here

        private void Start()
        {
            view.Populate(inventorySize);
        }
    }
}
