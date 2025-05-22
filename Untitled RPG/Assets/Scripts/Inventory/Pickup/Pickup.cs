 using System;
using RPG.Core;
using RPG.Inventory.Model;
using UnityEngine;
using UnityEngine.Serialization;

namespace RPG.Inventory.Pickup
{
    public class Pickup : MonoBehaviour
    {
        [field: SerializeField] public InventoryItemSO Item { get; private set; } = null;
        [field: SerializeField] public int Quantity { get; private set; } = 1;

        public string ItemName => Item.DisplayName;
    }
}
