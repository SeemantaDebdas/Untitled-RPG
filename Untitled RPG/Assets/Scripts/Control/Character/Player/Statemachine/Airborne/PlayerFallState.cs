using RPG.Core;
using RPG.Data;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerFallState : PlayerBaseState
    {
        Vector3 momentum = Vector3.zero;
        public override void Enter()
        {
            base.Enter();

            momentum = controller.velocity;
            momentum.y = 0f;

            animator.PlayAnimation(CharacterAnimationData.Instance.Locomotion.Fall);

            physicsHandler.ResetVerticalVelocity();
        }

        public override void Tick()
        {
            base.Tick();

            Move(momentum);
        }
    }
}
