using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerMovingState : PlayerBaseState
    {
        [Header("WALK")]
        [SerializeField] string walkAnimationName = "Walk";
        [SerializeField] float walkBaseSpeed = 1f;
        [SerializeField] float walkRotationSpeed = 6f;

        [Header("RUN")]
        [SerializeField] string runAnimationName = "Run";
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
            FaceMovementDirection(CalculateDirection(), rotationSpeed);
            
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
                animationName = walkAnimationName;

                return true;
            }
            
            if(!input.ShouldWalk && baseSpeed != runBaseSpeed)
            {
                baseSpeed = runBaseSpeed;
                rotationSpeed = runRotationSpeed;
                animationName = runAnimationName;

                return true;
            }

            return false;
        }
    }
}
