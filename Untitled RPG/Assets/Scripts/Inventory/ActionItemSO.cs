using UnityEngine;

namespace RPG.Inventory.Model
{
    [CreateAssetMenu(fileName = "New Action Item", menuName = "Inventory/Action Item")]
    public class ActionItemSO : InventoryItemSO
    {
        [field: SerializeField] public bool IsConsumable { get; private set;} = false;

        public virtual void Use(GameObject target)
        {
            Debug.Log("Called Use()");
            Debug.Log($"Using action item {name} on {target}");
        }
    }
}
