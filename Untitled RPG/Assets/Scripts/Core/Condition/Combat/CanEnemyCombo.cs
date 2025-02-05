using RPG.Data;
using UnityEngine;

namespace RPG.Core
{
    [CreateAssetMenu(fileName = "Can Enemy Combo", menuName = "Condition/Combat/Can Enemy Combo", order = 1)]
    public class CanEnemyCombo : ConditionSO
    {
        bool shouldCombo = false;
        public override void Initialize(Context context)
        { 
            int value = Random.Range(0, 2);
            //Debug.Log(value);
            shouldCombo = value == 1;
        }

        protected override bool ProcessCondition(Context context)
        {
            if (!shouldCombo)
                return false;
            
            CharacterContext characterContext = (CharacterContext)context;


            bool canCombo = characterContext.CombatHandler.CanCombo();
            
            //Debug.Log(canCombo);
            
            return canCombo;
        }

        public override void Reset()
        {
            
        }
    }
}
