using RPG.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    [CreateAssetMenu(fileName = "Is Within FOV", menuName = "Condition/Is Within FOV", order = 1)]
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
