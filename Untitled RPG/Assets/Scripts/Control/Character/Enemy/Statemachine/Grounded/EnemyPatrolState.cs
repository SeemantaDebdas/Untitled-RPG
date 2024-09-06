using RPG.Core;
using UnityEngine;

namespace RPG.Control
{
    public class EnemyPatrolState : EnemyBaseState
    {
        [SerializeField] string animationName = string.Empty;
        [SerializeField] float speed = 1.0f;
        [SerializeField] float rotationSpeed = 6.0f;
        [SerializeField] float distanceThreshold = 0.1f;

        public override void Enter()
        {
            base.Enter();

            //agent.SetDestination(path.GetCurrentWaypoint());

            animator.PlayAnimation(animationName);
            navmeshPath = new();

            agent.SetDestination(path.GetCurrentWaypoint());
            
        }

        public override void Tick()
        {
            base.Tick();

            HandleMovement(speed);
            FaceMovementDirection(CalculateDirection(), rotationSpeed);
        }

        public override void Exit()
        {
            base.Exit();

            Vector3 position = context.Transform.position;
            Vector3 waypoint = context.Path.GetCurrentWaypoint();

            float distanceToWaypoint = Vector3.Distance(position, waypoint);

            if (distanceToWaypoint <= distanceThreshold)
            {
                path.IncrementCurrentWaypointIndex();
            }
        }
        private void OnDrawGizmosSelected()
        {
            if (agent == null)
            {
                return;
            }

            if (agent.hasPath)
            {
                for (int i = 0; i < agent.path.corners.Length - 1; i++)
                {
                    Debug.DrawLine(agent.path.corners[i], agent.path.corners[i + 1], Color.blue);
                }
            }
        }
    }
}
