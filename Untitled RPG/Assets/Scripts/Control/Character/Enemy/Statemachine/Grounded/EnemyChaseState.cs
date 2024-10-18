using RPG.Core;
using RPG.Data;
using UnityEngine;

namespace RPG.Control
{
    public class EnemyChaseState : EnemyBaseState
    {
        [SerializeField] float speed = 1.876f;
        [SerializeField] float speedMultiplier = 0.75f;
        [SerializeField] float rotationSpeed = 6f;
        public override void Enter()
        {
            base.Enter();

            animator.PlayAnimation(CharacterAnimationData.Instance.Locomotion.Run);

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
            
            if(chaseFov.GetClosestTarget() != null) 
                agent.SetDestination(chaseFov.GetClosestTarget().position);

            HandleMovement(speed * speedMultiplier);
            FaceDirection(CalculateDirection(), rotationSpeed);
        }
    }
}
