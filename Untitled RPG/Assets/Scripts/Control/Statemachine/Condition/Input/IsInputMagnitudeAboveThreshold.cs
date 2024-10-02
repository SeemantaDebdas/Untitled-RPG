using RPG.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RPG.Control
{
    [CreateAssetMenu(fileName = "IsInputMagnitudeAboveThreshold", menuName = "Condition/Input/Is Input Magnitude Above Threshold", order = 1)]
    public class IsInputMagnitudeAboveThreshold : ConditionSO
    {
        [SerializeField] InputActionReference actionReference;
        [SerializeField] float threshold = 0.1f;
        public override void Initialize(Context context)
        {
            actionReference.action.Enable();
        }

        public override void Reset()
        {
        }

        protected override bool ProcessCondition(Context context)
        {
            actionReference.action.Enable();
            return actionReference.action.ReadValue<Vector2>().magnitude > threshold;
        }
    }
}
