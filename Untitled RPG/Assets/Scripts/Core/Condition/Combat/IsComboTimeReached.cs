using RPG.Core;
using RPG.Data;
using UnityEngine;

namespace RPG.Core
{
    [CreateAssetMenu(fileName = "IsComboTimeReached", menuName = "Condition/Combat/Is Combo Time Reached", order = 1)]
    public class IsComboTimeReached : ConditionSO
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

            return characterContext.CombatHandler.CanCombo();
        }
    }
}
