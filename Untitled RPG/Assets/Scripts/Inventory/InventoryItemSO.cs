using UnityEngine;
using UnityEngine.Serialization;

namespace RPG.Inventory
{
    [CreateAssetMenu(fileName = "New Inventory Item", menuName = "Inventory/InventoryItemSO")]
    public class InventoryItemSO : ScriptableObject
    {
        [field: SerializeField] 
        public string ItemID { get; private set; }
        
        [field: SerializeField, TextArea]
        public string Description { get; private set; } = null;

        [field: SerializeField]
        public Sprite ItemImage { get; private set; } = null;
        
        [field: SerializeField] 
        public bool IsStackable { get; private set; } = false;

        [field: SerializeField] 
        public int MaxStackSize { get; private set; } = 1;
        
        [field: SerializeField] 
        public float Price { get; private set; } = 0;
        public string DisplayName => name;
    }
}
