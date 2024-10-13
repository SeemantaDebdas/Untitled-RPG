using RPG.Core;
using UnityEngine;

namespace RPG.Control
{
    public class EnemyFallState : EnemyBaseState
    {
        [SerializeField] string animationName = "Fall";
        [SerializeField] float forwardNudgeForce = 0.1f;

        Vector3 momentum = Vector3.zero;
        public override void Enter()
        {
            base.Enter();

            momentum = controller.velocity;
            momentum.y = 0f;

            //add a bit of nudge towards character forward dir to push the character off the ledge
            momentum += context.Transform.forward * forwardNudgeForce;

            animator.PlayAnimation(animationName);

            physicsHandler.ResetVerticalVelocity();
        }

        public override void Tick()
        {
            base.Tick();

            Move(momentum);
        }
    }
}
