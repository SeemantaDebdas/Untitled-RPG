using RPG.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    [CreateAssetMenu(fileName = "IsWithinAttackRadius", menuName = "Condition/NPC/Is Within Attack Radius", order = 1)]
    public class IsWithinAttackRadius : ConditionSO
    {
        public override void Initialize(Context context)
        {

        }

        public override void Reset()
        {

        }

        protected override bool ProcessCondition(Context context)
        {
            return (context as EnemyContext).AttackFOV.GetClosestTarget() != null;
        }
    }
}
