using MEC;
using RPG.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RPG.Core
{
    [CreateAssetMenu(fileName = "Has Input Action", menuName = "Condition/Input/Has Input Action", order = 1)]
    public class HasInputAction : ConditionSO
    {
        [Space]
        [SerializeField] private bool performed = false;
        [SerializeField] private bool cancelled = false;
        [SerializeField] private bool started = false;

        [SerializeField] bool prematureReset = false;
        [SerializeField] float resetTimeWindow = 0.5f;

        public enum InputType
        {
            STARTED, PERFORMED, CANCELLED
        }

        [Space]
        [SerializeField] InputActionReference actionReference;
        [SerializeField] InputType inputType;

        CoroutineHandle resetInputStatesHandle;

        public override void Initialize(Context context)
        {
            Reset();

            actionReference.action.started += OnActionStarted;
            actionReference.action.performed += OnActionPerformed;
            actionReference.action.canceled += OnActionCancelled;

            // Enable the action
            //actionReference.action.Enable();
        }

        public override void Reset()
        {
            // Reset states
            ResetInputStates();

            // Unsubscribe from events
            actionReference.action.started -= OnActionStarted;
            actionReference.action.performed -= OnActionPerformed;
            actionReference.action.canceled -= OnActionCancelled;

            if(resetInputStatesHandle.IsValid)
                Timing.KillCoroutines(resetInputStatesHandle);
            // Disable the action
            //actionReference.action.Disable();
        }

        private void ResetInputStates()
        {
            started = false;
            performed = false;
            cancelled = false;
        }

        protected override bool ProcessCondition(Context context)
        {
            if (!actionReference.action.actionMap.enabled)
                return false;
            
            bool result = inputType switch
            {
                InputType.STARTED => started,
                InputType.PERFORMED => performed,
                InputType.CANCELLED => cancelled,
                _ => false,
            };

            if(result && prematureReset)
            {
                resetInputStatesHandle = Timing.RunCoroutine(ResetInputStatesAfterSeconds(resetTimeWindow));
            }

            return result;
        }

        IEnumerator<float> ResetInputStatesAfterSeconds(float resetTimeWindow)
        {
            yield return Timing.WaitForSeconds(resetTimeWindow);

            ResetInputStates();
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
            // Debug.Log(name + "Cancelled");
            cancelled = context.canceled; 
        }
    }
}
