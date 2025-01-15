using UnityEngine;

namespace RPG.Inventory.Model
{
    [CreateAssetMenu(fileName = "New Action Item", menuName = "Debdas/Inventory/Action Item")]
    public class ActionItemSO : InventoryItemSO
    {
        [field: SerializeField] public bool IsConsumable { get; private set;} = false;

        public virtual bool TryUse(GameObject user)
        {
            return true;
        }

        public virtual void Use(GameObject user)
        {
            Debug.Log("Called Use()");
            Debug.Log($"Using action item {name} on {user}");
        }
    }
}
