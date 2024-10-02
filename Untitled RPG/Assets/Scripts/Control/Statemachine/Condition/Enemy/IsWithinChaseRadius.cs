using RPG.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    [CreateAssetMenu(fileName = "IsWithinChaseRadius", menuName = "Condition/NPC/Is Within Chase Radius", order = 1)]
    public class IsWithinChaseRadius : ConditionSO
    {
        public override void Initialize(Context context)
        {

        }

        public override void Reset()
        {

        }

        protected override bool ProcessCondition(Context context)
        {
            return (context as EnemyContext).ChaseFOV.GetClosestTarget() != null;
        }
    }
}
