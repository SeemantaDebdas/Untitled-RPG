using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerMovingState : PlayerBaseState
    {
        [Header("WALK")]
        [SerializeField] string walkAnimationName = "Walk";
        [SerializeField] float walkBaseState = 1f;
        [SerializeField] float walkRotationSpeed = 6f;

        [Header("RUN")]
        [SerializeField] string runAnimationName = "Run";
        [SerializeField] float runBaseState = 1f;
        [SerializeField] float runRotationSpeed = 6f;

        float baseSpeed, rotationSpeed;
        string animationName = "Walk";

        public override void Enter()
        {
            base.Enter();
            EvaluateMoveAndRotationSpeed();

            PlayAnimation(animationName);
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

            PlayAnimation(animationName);
        }

        bool EvaluateMoveAndRotationSpeed()
        {
            if (input.ShouldWalk && baseSpeed != walkBaseState)
            {
                baseSpeed = walkBaseState;
                rotationSpeed = walkRotationSpeed;
                animationName = walkAnimationName;

                return true;
            }
            
            if(!input.ShouldWalk && baseSpeed != runBaseState)
            {
                baseSpeed = runBaseState;
                rotationSpeed = runRotationSpeed;
                animationName = runAnimationName;

                return true;
            }

            return false;
        }
    }
}
