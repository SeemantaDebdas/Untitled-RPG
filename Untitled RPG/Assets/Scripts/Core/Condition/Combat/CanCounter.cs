using RPG.Combat.Rework;
using RPG.Data;
using UnityEngine;

namespace RPG.Core
{
    [CreateAssetMenu(fileName = "Can Counter", menuName = "Condition/Combat/Can Counter", order = 1)]
    public class CanCounter : ConditionSO
    {
        public override void Initialize(Context context)
        {
            
        }

        protected override bool ProcessCondition(Context context)
        {
            CombatHandler combatHandler = (context as CharacterContext).CombatHandler;

            return combatHandler.CanCounter();
        }

        public override void Reset()
        {
            
        }
    }
}
