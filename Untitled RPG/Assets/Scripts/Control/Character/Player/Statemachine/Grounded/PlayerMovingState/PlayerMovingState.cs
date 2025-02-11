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
        [SerializeField] ScriptableFloat runFloat;
        [SerializeField] float runRotationSpeed = 6f;

        float baseSpeed, rotationSpeed;
        string animationName = "Walk";

        public override void Enter()
        {
            base.Enter();
            runFloat.OnValueChanged += _ => EvaluateMoveAndRotationSpeed();
            EvaluateMoveAndRotationSpeed();

            animator.PlayAnimation(animationName);
        }

        public override void Exit()
        {
            base.Exit();
            runFloat.OnValueChanged -= _ => EvaluateMoveAndRotationSpeed();
        }

        public override void Tick()
        {
            //print(runFloat.Value);
            HandleMovement(baseSpeed);
            FaceMovementDirection(CalculateDirection(), rotationSpeed);
            
            base.Tick();
        }

        protected override void InputReader_OnWalkTogglePerformed()
        {
            base.InputReader_OnWalkTogglePerformed();

            if (!EvaluateMoveAndRotationSpeed())
                return;
            
            animator.PlayAnimation(animationName);
        }

        bool EvaluateMoveAndRotationSpeed()
        {
            if (InputReader.ShouldWalk && baseSpeed != walkBaseSpeed)
            {
                baseSpeed = walkBaseSpeed;
                rotationSpeed = walkRotationSpeed;
                animationName = CharacterAnimationData.Instance.Locomotion.Walk;

                return true;
            }
            
            if(!InputReader.ShouldWalk && baseSpeed != runFloat.Value)
            {
                baseSpeed = runFloat.Value;
                rotationSpeed = runRotationSpeed;
                animationName = CharacterAnimationData.Instance.Locomotion.Run;

                return true;
            }

            return false;
        }
    }
}
