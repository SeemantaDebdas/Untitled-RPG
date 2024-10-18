using RPG.Core;
using RPG.Data;
using UnityEngine;

namespace RPG.Control
{
    public class EnemyFallState : EnemyBaseState
    {
        [SerializeField] float forwardNudgeForce = 0.1f;

        Vector3 momentum = Vector3.zero;
        public override void Enter()
        {
            base.Enter();

            momentum = controller.velocity;
            momentum.y = 0f;

            //add a bit of nudge towards character forward dir to push the character off the ledge
            momentum += context.Transform.forward * forwardNudgeForce;

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
