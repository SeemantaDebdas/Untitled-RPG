using RPG.Data;
using RPG.Core;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RPG.Control
{
    public abstract class PlayerBaseState : CharacterBaseState
    {
        protected PlayerContext context;
        protected PlayerInput input;


        protected Vector2 moveInput = Vector2.zero;


        public override void Initialize(IStatemachine statemachine)
        {
            base.Initialize(statemachine);
            context = statemachine.Context as PlayerContext;

            animator = context.Animator;
            input = context.PlayerInput;
            controller = context.CharacterController;
            physicsHandler = context.PhysicsHandler;
            weaponHandler = context.WeaponHandler;
        }

        public override void Enter()
        {
            base.Enter();

            //Debug.Log("Enter: " + GetType().Name);

            AddInputActionsCallback();
        }

        public override void Exit()
        {
            base.Exit();

            RemoveInputActionsCallback();
        }

        public override void HandleInput()
        {
            base.HandleInput();
            moveInput = input.MoveInput;
        }


        protected virtual void AddInputActionsCallback()
        {
            input.OnWalkTogglePerformed += Input_OnWalkTogglePerformed;
        }

        protected virtual void RemoveInputActionsCallback()
        {
            input.OnWalkTogglePerformed -= Input_OnWalkTogglePerformed;
        }

        protected virtual void Input_OnWalkTogglePerformed(){}

        #region ROTATION

        protected override void FaceDirection(Vector3 movement, float rotationSpeed)
        {
            if(moveInput ==  Vector2.zero)
                return;

            context.Transform.rotation = Quaternion.Slerp(context.Transform.rotation,
                                                Quaternion.LookRotation(movement),
                                                rotationSpeed * Time.deltaTime);
        }

        protected override Vector3 CalculateDirection()
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
