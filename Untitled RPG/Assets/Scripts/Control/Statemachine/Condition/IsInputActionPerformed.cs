using RPG.Data;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RPG.Control
{
    [CreateAssetMenu(fileName = "IsInputActionPerformed", menuName = "Condition/Is Input Action Performed", order = 1)]
    public class IsInputActionPerformed : ConditionSO
    {
        [SerializeField] InputActionReference actionReference;
        public override void Initialize(Context context)
        {
            actionReference.action.Enable();
        }

        public override void Reset()
        {
            actionReference.action.Disable();
        }

        protected override bool ProcessCondition(Context context)
        {
            //return (actionReference.action.ReadValue<Vector2>() != Vector2.zero); 
            PlayerContext playerContext = context as PlayerContext;
            return playerContext.PlayerInput.MoveInput != Vector2.zero;
        }
    }
}
