using RPG.Data;
using UnityEngine;

namespace RPG.Core
{
    [CreateAssetMenu(fileName = "Should Walk", menuName = "Condition/Input/Should Walk", order = 1)]
    public class ShouldWalk : ConditionSO 
    {
        public override void Initialize(Context context)
        {
            
        }

        protected override bool ProcessCondition(Context context)
        {
            return context is PlayerContext playerContext && playerContext.InputReader.ShouldWalk;
        }

        public override void Reset()
        {
            
        }
    }
}
