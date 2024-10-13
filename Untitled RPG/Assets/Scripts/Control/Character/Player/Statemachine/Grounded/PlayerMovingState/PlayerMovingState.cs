using RPG.Core;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerMovingState : PlayerBaseState
    {
        [Header("WALK")]
        [SerializeField] ScriptableString walkAnimation;
        [SerializeField] float walkBaseSpeed = 1f;
        [SerializeField] float walkRotationSpeed = 6f;

        [Header("RUN")]
        [SerializeField] ScriptableString runAnimation;
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
                animationName = walkAnimation.Value;

                return true;
            }
            
            if(!input.ShouldWalk && baseSpeed != runBaseSpeed)
            {
                baseSpeed = runBaseSpeed;
                rotationSpeed = runRotationSpeed;
                animationName = runAnimation.Value;

                return true;
            }

            return false;
        }
    }
}
