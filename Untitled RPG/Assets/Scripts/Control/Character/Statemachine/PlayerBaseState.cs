using RPG.Data;
using RPG.Core;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public abstract class PlayerBaseState : State
    {
        [field: SerializeField] protected string animationName;

        protected PlayerContext context;
        protected Animator animator;
        protected PlayerInput input;
        protected CharacterController controller;

        protected Vector2 moveInput = Vector2.zero;
        protected float speedMultiplier = 1f;
        protected float slopeSpeedMultiplier = 1f;

        public override void Initialize(IStatemachine statemachine)
        {
            this.statemachine = statemachine;
            context = statemachine.Context as PlayerContext;

            animator = context.Animator;
            input = context.PlayerInput;
            controller = context.CharacterController;
        }

        public override void Enter()
        {
            base.Enter();

            Debug.Log("Enter: " + GetType().Name);
        }

        public override void HandleInput()
        {
            base.HandleInput();
            moveInput = input.MoveInput;
        }


        protected void PlayAnimation(string animationName, float transitionDuration = 0.25f, int layer = 0) 
        {
            animator.CrossFadeInFixedTime(animationName, transitionDuration, layer);
        }

        protected void HandleMovement(float baseSpeed)
        {
            if (moveInput == Vector2.zero || speedMultiplier == 0f)
            {
                Move();
                return;
            }

            float movementSpeed = GetMovementSpeed(baseSpeed);

            Move(movementSpeed * CalculateDirection());
        }

        protected void Move(Vector3 motion)
        {
            controller.Move((context.PhysicsHandler.Movement + motion) * Time.deltaTime);
        }

        protected void Move()
        {
            controller.Move(context.PhysicsHandler.Movement * Time.deltaTime);
        }

        protected float GetMovementSpeed(float baseSpeed)
        {
            float movementSpeed = baseSpeed * speedMultiplier * slopeSpeedMultiplier;
            return movementSpeed;
        }


        #region ROTATION

        protected void FaceMovementDirection(Vector3 movement, float rotationSpeed)
        {
            if (moveInput == Vector2.zero)
                return;

            context.Transform.rotation = Quaternion.Slerp(context.Transform.rotation,
                                                          Quaternion.LookRotation(movement),
                                                          rotationSpeed * Time.deltaTime);
        }

        protected Vector3 CalculateDirection()
        {
            Transform cameraTransform = Camera.main.transform;

            Vector3 cameraForward = cameraTransform.forward;
            cameraForward.y = 0;
            cameraForward.Normalize();

            Vector3 cameraRight = cameraTransform.right;
            cameraRight.y = 0;
            cameraRight.Normalize();

            return cameraRight * moveInput.x + cameraForward * moveInput.y;
        }

        #endregion
    }
}
