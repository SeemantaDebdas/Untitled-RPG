using UnityEngine;
using UnityEngine.InputSystem;
using System;

namespace RPG.Core
{
    public class PlayerInput : MonoBehaviour, Control.IPlayerActions
    {
        public event Action OnJumpEvent;
        public Vector2 MoveInput { get; private set; }
        public event Action OnMovePerformed, OnMoveCancelled;

        public event Action OnSprintAction;
        Control control;

        private void OnEnable()
        {
            control ??= new Control();
            control.Enable();
        }

        private void OnDisable()
        {
            control.Disable();
        }

        private void Start()
        {
            control.Player.SetCallbacks(this);
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            OnJumpEvent?.Invoke();
        }

        public void OnLook(InputAction.CallbackContext context){}

        public void OnMove(InputAction.CallbackContext context)
        {
            MoveInput = context.ReadValue<Vector2>();

            if (context.performed)
                OnMovePerformed?.Invoke();

            if (context.canceled)
                OnMoveCancelled?.Invoke();

        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            if(!context.performed) return;

            OnSprintAction?.Invoke();
        }
    }
}
