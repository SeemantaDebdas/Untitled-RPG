using RPG.Data;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RPG.Control
{
    [CreateAssetMenu(fileName = "Has Input Action", menuName = "Condition/Has Input Action", order = 1)]
    public class HasInputAction : ConditionSO
    {
        public enum InputType
        {
            STARTED, PERFORMED, CANCELLED
        }

        [SerializeField] InputActionReference actionReference;
        [SerializeField] InputType inputType;

        private bool performed = false;
        private bool cancelled = false;
        private bool started = false;

        public override void Initialize(Context context)
        {
            performed = false;
            cancelled = false;
            started = false;

            actionReference.action.started += OnActionStarted;
            actionReference.action.performed += OnActionPerformed;
            actionReference.action.canceled += OnActionCancelled;

            // Enable the action
            actionReference.action.Enable();
        }

        public override void Reset()
        {
            // Reset states
            performed = false;
            cancelled = false;
            started = false;

            // Unsubscribe from events
            actionReference.action.started -= OnActionStarted;
            actionReference.action.performed -= OnActionPerformed;
            actionReference.action.canceled -= OnActionCancelled;

            // Disable the action
            //actionReference.action.Disable();
        }

        protected override bool ProcessCondition(Context context)
        {
            return inputType switch
            {
                InputType.STARTED => started,
                InputType.PERFORMED => performed,
                InputType.CANCELLED => cancelled,
                _ => false,
            };
        }

        private void OnActionStarted(InputAction.CallbackContext context)
        {
            started = context.started; 
        }

        private void OnActionPerformed(InputAction.CallbackContext context)
        {
            performed = context.performed; 
        }

        private void OnActionCancelled(InputAction.CallbackContext context)
        {
            cancelled = context.canceled; 
        }
    }
}
