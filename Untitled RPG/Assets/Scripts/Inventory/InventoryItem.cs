using UnityEngine;

namespace RPG.Inventory
{
    [CreateAssetMenu(fileName = "New Inventory Item", menuName = "Inventory Item")]
    public class InventoryItem : ScriptableObject
    {
        [Tooltip("Auto-generated UUID for saving/loading. Clear this field if you want to generate a new one.")]
        [SerializeField] string itemID = null;

        [Tooltip("Item name to be displayed in UI.")]
        [field: SerializeField]
        public string DisplayName { get; private set; } = null;
        
        [Tooltip("Item description to be displayed in UI.")]
        [SerializeField][TextArea] string description = null;

        [Tooltip("The UI icon to represent this item in the inventory.")]
        [field: SerializeField]
        public Sprite Icon { get; private set; } = null;
        
        [Tooltip("If true, multiple items of this type can be stacked in the same inventory slot.")]
        [SerializeField] bool stackable = false;
    }
}
