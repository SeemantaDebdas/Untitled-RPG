using RPG.Core;
using RPG.Data;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerMovingState : PlayerBaseState
    {
        [Header("WALK")]
        [SerializeField] float walkBaseSpeed = 1f;
        [SerializeField] float walkRotationSpeed = 6f;

        [Header("RUN")]
        [SerializeField] float runBaseSpeed = 1f;
        [SerializeField] float runRotationSpeed = 6f;

        float baseSpeed, rotationSpeed;
        string animationName = "Walk";

        public override void Enter()
        {
            base.Enter();
            EvaluateMoveAndRotationSpeed();

            animator.PlayAnimation(animationName);
        }

        public override void Tick()
        {
            HandleMovement(baseSpeed);
            FaceDirection(CalculateDirection(), rotationSpeed);
            
            base.Tick();
        }

        protected override void Input_OnWalkTogglePerformed()
        {
            base.Input_OnWalkTogglePerformed();

            if (!EvaluateMoveAndRotationSpeed())
                return;
            
            animator.PlayAnimation(animationName);
        }

        bool EvaluateMoveAndRotationSpeed()
        {
            if (input.ShouldWalk && baseSpeed != walkBaseSpeed)
            {
                baseSpeed = walkBaseSpeed;
                rotationSpeed = walkRotationSpeed;
                animationName = CharacterAnimationData.Instance.Locomotion.Walk;

                return true;
            }
            
            if(!input.ShouldWalk && baseSpeed != runBaseSpeed)
            {
                baseSpeed = runBaseSpeed;
                rotationSpeed = runRotationSpeed;
                animationName = CharacterAnimationData.Instance.Locomotion.Run;

                return true;
            }

            return false;
        }
    }
}
