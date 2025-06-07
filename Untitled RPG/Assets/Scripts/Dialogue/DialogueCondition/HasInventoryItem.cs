using RPG.Data;
using RPG.Inventory.Model;
using UnityEngine;

namespace RPG.DialogueSystem
{
    [CreateAssetMenu(fileName = "HasInventoryItem", menuName = "Condition/Dialogue/Has Inventory Item", order = 1)]

    public class HasInventoryItem : DialogueConditionSO
    {
        [SerializeField] InventoryItemSO item;
        [SerializeField] int quantity;
        public override void Initialize(Context context)
        {

        }

        public override void Reset()
        {

        }

        protected override bool ProcessCondition(Context context)
        {
            bool result = (context as PlayerContext).Inventory.HasItem(item, quantity);
            return result;
        }
    }
}
