using RPG.Control;
using RPG.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    [CreateAssetMenu(fileName = "IsClimbableObjectInfront", menuName = "Condition/Is Climbable Object In front", order = 1)]
    public class IsClimbableObjectInfront : ConditionSO
    {
        public override void Initialize(Context context)
        {
            
        }

        public override void Reset()
        {
            
        }

        protected override bool ProcessCondition(Context context)
        {
            EnvironmentScanner scanner = (context as PlayerContext).EnvironmentScanner;

            if(!scanner.IsObjectInfront())
                return false;

            RaycastHit? obj = scanner.GetObjectInfront(context.Transform.position);

            return scanner.IsObjectBelow(obj.Value.point);
        }
    }
}
