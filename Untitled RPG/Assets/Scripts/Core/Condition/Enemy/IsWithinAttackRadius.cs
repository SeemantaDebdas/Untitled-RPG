using RPG.Data;
using UnityEngine;

namespace RPG.Core
{
    [CreateAssetMenu(fileName = "IsWithinAttackRadius", menuName = "Condition/NPC/Is Within Attack Radius", order = 1)]
    public class IsWithinAttackRadius : ConditionSO
    {
        [SerializeField] private float attackRangeOffset = 2.5f;
        public override void Initialize(Context context)
        {

        }

        public override void Reset()
        {

        }

        protected override bool ProcessCondition(Context context)
        {
            FieldOfView fieldOfView = (context as EnemyContext).ChaseFOV;
            Transform closestTarget = fieldOfView.GetClosestTarget();
            float distance = Vector3.Distance(closestTarget.position, context.Transform.position);

            return distance < fieldOfView.Radius - attackRangeOffset;
        }
    }
}
