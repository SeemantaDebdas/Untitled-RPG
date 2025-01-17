using UnityEngine;
using UnityEngine.Serialization;

namespace RPG.Inventory.Model
{
    [CreateAssetMenu(fileName = "New Inventory Item", menuName = "Debdas/Inventory/InventoryItemSO")]
    public class InventoryItemSO : ScriptableObject
    {
        public int ItemID => GetInstanceID();
        [field: SerializeField]
        public ItemCategory Category { get; private set; }
        
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
