using RPG.Core;
using RPG.Data;
using UnityEngine;

namespace RPG.Control
{
    public abstract class PlayerMovingState : PlayerBaseState, IValueSetter<float>
    {
        [SerializeField] protected string animationName = "";
        [SerializeField] protected ScriptableFloat moveFloat, speedFloat;
        [SerializeField] protected float rotationSpeed = 6f;

        public override void Enter()
        {
            base.Enter();

            moveFloat.SetValue(speedFloat.Value, this);
            //Debug.Log("Setting moveFloat: " + speedFloat);
            moveFloat.SetValue(speedFloat.Value, this);
            animator.PlayAnimation(animationName, 0.1f);
        }

        public override void Tick()
        {
            base.Tick();
            HandleMovement(speedFloat.Value);
            FaceMovementDirection(CalculateDirection(), rotationSpeed);
        }
    }
}
