using RPG.Data;
using UnityEngine;

namespace RPG.Core
{
    [CreateAssetMenu(fileName = "IsWeaponEquipped", menuName = "Condition/Combat/Is Weapon Equipped", order = 1)]
    public class IsWeaponEquipped : ConditionSO
    {
        public override void Initialize(Context context)
        {
            
        }

        public override void Reset()
        {
            
        }

        protected override bool ProcessCondition(Context context)
        {
            CharacterContext characterContext = (CharacterContext)context;

            return !characterContext.WeaponHandler.IsCurrentWeaponSheathed();
        }
    }
}
