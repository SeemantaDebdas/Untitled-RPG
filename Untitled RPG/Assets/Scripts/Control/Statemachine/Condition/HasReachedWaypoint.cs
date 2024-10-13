using RPG.Data;
using UnityEngine;

namespace RPG.Control
{
    [CreateAssetMenu(fileName = "HasReachedWaypoint", menuName = "Condition/NPC/Has Reached Waypoint", order = 1)]
    public class HasReachedWaypoint : ConditionSO
    {
        [SerializeField] float distanceThreshold = 0.1f;
        public override void Initialize(Context context)
        {
        }

        public override void Reset()
        {
        }

        protected override bool ProcessCondition(Context context)
        {
            EnemyContext enemyContext = context as EnemyContext;
            Vector3 position = context.Transform.position;
            Vector3 waypoint = enemyContext.Path.GetCurrentWaypoint();

            float distanceToWaypoint = Vector3.Distance(position, waypoint);

            if (distanceToWaypoint <= distanceThreshold)
            {
                return true;
            }

            return false;
        }
    }
}
