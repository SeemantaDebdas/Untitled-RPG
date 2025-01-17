using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RPG.Core
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "Input/Input Reader", order = 1)]
    public class InputReaderSO : ScriptableObject
    {
        public event Action OnJumpEvent;
        public Vector2 MoveInput { get; private set; }
        public event Action OnMovePerformed, OnMoveCancelled;

        public event Action OnWalkTogglePerformed;
        public bool ShouldWalk { get; private set; }

        public event Action OnSprintPerformed;

        public event Action OnAttackPerformed;

        public event Action OnHolsterPerformed;

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
            //control.Player.SetCallbacks(this);
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            OnJumpEvent?.Invoke();
        }

        public void OnLook(InputAction.CallbackContext context) { }

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
            if (!context.performed) return;

            OnSprintPerformed?.Invoke();
        }

        public void OnWalkToggle(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            ShouldWalk = !ShouldWalk;
            OnWalkTogglePerformed?.Invoke();
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            OnAttackPerformed?.Invoke();
        }

        public void OnHolster(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            OnHolsterPerformed?.Invoke();
        }
    }
}
