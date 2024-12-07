using RPG.Data;
using UnityEngine;

namespace RPG.Core
{
    [CreateAssetMenu(fileName = "Is Within FOV", menuName = "Condition/Detection/Is Within FOV", order = 1)]
    public class IsWithinFOV : ConditionSO
    {
        public override void Initialize(Context context)
        {
            
        }

        public override void Reset()
        {
            
        }

        protected override bool ProcessCondition(Context context)
        {
            return (context as CharacterContext).FieldOfView.GetClosestTarget() != null;
        }
    }
}
