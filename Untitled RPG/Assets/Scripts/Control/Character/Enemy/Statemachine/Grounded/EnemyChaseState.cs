using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class EnemyChaseState : EnemyBaseState
    {
        [SerializeField] string animationName = string.Empty;
        [SerializeField] float speed = 1.876f;
        [SerializeField] float speedMultiplier = 0.75f;
        [SerializeField] float rotationSpeed = 6f;
        public override void Enter()
        {
            base.Enter();

            animator.PlayAnimation(animationName);

            animator.speed *= speedMultiplier;
        }

        public override void Exit()
        {
            base.Exit();

            animator.speed = 1f;
        }

        public override void Tick()
        {
            base.Tick();
            
            agent.SetDestination(fieldOfView.GetClosestTarget().position);

            HandleMovement(speed * speedMultiplier);
            FaceMovementDirection(CalculateDirection(), rotationSpeed);
        }
    }
}
