using RPG.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    [CreateAssetMenu(fileName = "IsWeaponEquipped", menuName = "Condition/Is Weapon Equipped", order = 1)]
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

            return !characterContext.WeaponHandler.IsSheathed;
        }
    }
}
