using RPG.Data;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RPG.Control
{
    [CreateAssetMenu(fileName = "IsInputActionPerformed", menuName = "Condition/Is Input Action Performed", order = 1)]
    public class IsInputActionPerformed : ConditionSO
    {
        [SerializeField] InputActionReference actionReference;
        private bool performed = false;

        public override void Initialize(Context context)
        {
            actionReference.action.performed += (InputAction.CallbackContext _) => performed = true;
            actionReference.action.canceled += (InputAction.CallbackContext _) => performed = false; 
            
            actionReference.action.Enable();
        }

        public override void Reset()
        {
            //Debug.Log("Reset Calledd");
            
            //actionReference.action.performed -= OnActionPerformed;
            //actionReference.action.canceled -= OnActionCanceled;

            //actionReference.action.Disable();
        }

        protected override bool ProcessCondition(Context context)
        {
            return performed;
        }
    }
}
