using UnityEngine;
using UnityEngine.InputSystem;
using System;

namespace RPG.Core
{
    public class InputReader : MonoBehaviour, Control.IPlayerActions
    {
        [SerializeField] InputActionAsset inputAsset;
        public event Action OnJumpEvent;
        public Vector2 MoveInput { get; private set; }

        public Vector3 MoveInput3
        {
            get
            {
                return new Vector3(MoveInput.x, 0, MoveInput.y);
            }
        }

        public event Action OnMovePerformed, OnMoveCancelled;

        public event Action OnWalkTogglePerformed;
        public bool ShouldWalk { get; private set; } = true;

        public event Action OnSprintPerformed;

        public event Action OnAttackPerformed, OnAttackCancelled;

        public event Action OnHolsterPerformed;

        public event Action<float> OnChangeWeaponPerformed;
        public event Action<float> OnScrollPerformed;

        public Control Control => control;
        Control control;
        
        string playerActionMapName = "Player";
        string uiActionMapName = "UI";
        
        private void OnDisable()
        {
            control.Disable();
        }

        private void Awake()
        {
            control ??= new Control();
            control.Enable();
            control.Player.SetCallbacks(this);
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            OnJumpEvent?.Invoke();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
        }

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
            if (!IsPlayerActionMapEnabled())
                return;
            
            if (context.performed)
            {
                OnAttackPerformed?.Invoke();
            }

            if(context.canceled)
            {
                OnAttackCancelled?.Invoke();
            }
        }

        public void OnHolster(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            OnHolsterPerformed?.Invoke();
        }

        public void OnScroll(InputAction.CallbackContext context)
        {
            if (!IsPlayerActionMapEnabled())
                return;
            
            if (!context.performed)
                return;

            float input = 0;

            if(context.control.device is Mouse)
            {
                input = context.ReadValue<Vector2>().y;
            }
            else if(context.control.device is Gamepad)
            {
                input = context.ReadValue<float>();
            }

            //Debug.Log(input);

            OnChangeWeaponPerformed?.Invoke(input);
            OnScrollPerformed?.Invoke(input);
        }

        public void OnRoll(InputAction.CallbackContext context)
        {
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
        }

        public void OnQuestUIToggle(InputAction.CallbackContext context)
        {
        }

        public void OnInventoryUIToggle(InputAction.CallbackContext context)
        {
        }

        public void OnAbility1(InputAction.CallbackContext context)
        {
            
        }

        public void OnAbility2(InputAction.CallbackContext context)
        {
            
        }

        public void OnAbility3(InputAction.CallbackContext context)
        {
            
        }

        public void OnAbility4(InputAction.CallbackContext context)
        {
            
        }

        public void OnAbility5(InputAction.CallbackContext context)
        {
            
        }

        public void OnCounter(InputAction.CallbackContext context)
        {
        }

        public void OnPickup(InputAction.CallbackContext context)
        {
            
        }

        bool IsPlayerActionMapEnabled()
        {
            return inputAsset.FindActionMap(playerActionMapName).enabled;
        }
    }
}
