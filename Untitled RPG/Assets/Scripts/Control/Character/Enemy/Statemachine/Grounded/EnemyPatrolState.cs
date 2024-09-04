using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class EnemyPatrolState : EnemyBaseState
    {
        [SerializeField] string animationName = string.Empty;
        [SerializeField] float speed = 1.0f;
        [SerializeField] float rotationSpeed = 6.0f;

        public override void Enter()
        {
            base.Enter();

            //agent.SetDestination(path.GetCurrentWaypoint());

            animator.PlayAnimation(animationName);
            navmeshPath = new();
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
            path.IncrementCurrentWaypointIndex();
        }
    }
}
