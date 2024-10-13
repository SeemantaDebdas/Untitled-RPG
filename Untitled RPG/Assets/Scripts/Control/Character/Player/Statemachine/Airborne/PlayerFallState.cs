using RPG.Core;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerFallState : PlayerBaseState
    {
        [SerializeField] ScriptableString animationName;

        Vector3 momentum = Vector3.zero;
        public override void Enter()
        {
            base.Enter();

            momentum = controller.velocity;
            momentum.y = 0f;

            animator.PlayAnimation(animationName.Value);

            physicsHandler.ResetVerticalVelocity();
        }

        public override void Tick()
        {
            base.Tick();

            Move(momentum);
        }
    }
}
