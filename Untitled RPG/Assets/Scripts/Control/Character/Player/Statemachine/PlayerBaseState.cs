using RPG.Data;
using RPG.Core;
using UnityEngine;
using System;
using RPG.DialogueSystem;

namespace RPG.Control
{
    public abstract class PlayerBaseState : CharacterBaseState
    {
        protected PlayerContext context;
        protected InputReader InputReader;
        protected PlayerConversant conversant;
        
        protected Vector2 moveInput = Vector2.zero;

        public override void Initialize(IStatemachine statemachine)
        {
            base.Initialize(statemachine);
            context = statemachine.Context as PlayerContext;

            InputReader = context.InputReader;
            conversant = context.PlayerConversant;
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
            moveInput = InputReader.MoveInput;
        }


        protected virtual void AddInputActionsCallback()
        {
            InputReader.OnWalkTogglePerformed += InputReader_OnWalkTogglePerformed;
            InputReader.OnAttackCancelled += InputReader_OnAttackCancelled;
            InputReader.OnHolsterPerformed += InputReader_OnHolsterPerformed;
        }

        protected virtual void RemoveInputActionsCallback()
        {
            InputReader.OnWalkTogglePerformed -= InputReader_OnWalkTogglePerformed;
            InputReader.OnAttackCancelled -= InputReader_OnAttackCancelled;
            InputReader.OnHolsterPerformed -= InputReader_OnHolsterPerformed;
        }


        protected virtual void InputReader_OnWalkTogglePerformed(){}

        protected virtual void InputReader_OnAttackCancelled()
        {
            if(weaponHandler.CurrentWeapon.IsSheathed) 
                weaponHandler.PlayCurrentWeaponUnsheathAnimation();
        }

        protected virtual void InputReader_OnHolsterPerformed()
        {
            if(!weaponHandler.CurrentWeapon.IsSheathed)
                weaponHandler.PlayCurrentWeaponSheathAnimation();
        }

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
            Transform cameraTransform = UnityEngine.Camera.main.transform;

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
