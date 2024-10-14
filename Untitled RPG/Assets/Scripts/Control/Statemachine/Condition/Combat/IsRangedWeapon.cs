using RPG.Combat;
using RPG.Data;
using UnityEngine;

namespace RPG.Control
{
    [CreateAssetMenu(fileName = "IsRangedWeapon", menuName = "Condition/Combat/Is Ranged Weapon", order = 1)]
    public class IsRangedWeapon : ConditionSO
    {
        public override void Initialize(Context context)
        {
            
        }

        public override void Reset()
        {
            
        }

        protected override bool ProcessCondition(Context context)
        {
            if(context is CharacterContext characterContext)
            {
                if (characterContext.WeaponHandler.CurrentWeapon.WeaponData is RangedWeaponSO)
                    return true;
            }

            return false;
        }
    }
}
