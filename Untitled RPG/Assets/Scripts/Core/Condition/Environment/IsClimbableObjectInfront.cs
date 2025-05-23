using RPG.Control;
using RPG.Data;
using UnityEngine;

namespace RPG.Core
{
    [CreateAssetMenu(fileName = "IsClimbableObjectInfront", menuName = "Condition/Detection/Is Climbable Object In front", order = 1)]
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
            EnvironmentScanner scanner = (context as CharacterContext).EnvironmentScanner;

            if(!scanner.IsObjectInfront())
                return false;

            Debug.Log("Object infornt");

            RaycastHit? obj = scanner.GetObjectInfront(context.Transform.position);

            bool isObjectBelow = scanner.IsObjectBelow(obj.Value.point);

            Debug.Log(isObjectBelow);

            return isObjectBelow;
        }
    }
}
