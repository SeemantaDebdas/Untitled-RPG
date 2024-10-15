using RPG.Core;
using RPG.Data;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerLandState : PlayerBaseState
    {
        [SerializeField] float baseSpeed = 2f;

        string animationName;
        Vector3 momentum;

        public override void Enter()
        {
            base.Enter();

            momentum = context.CharacterController.velocity;
            momentum.y = 0;

            if (input.MoveInput.magnitude < 0.1f)
            {
                animationName = CharacterAnimationData.Instance.LandIdle;
            }
            else
            {
                animationName = CharacterAnimationData.Instance.LandMove;
            }

            animator.PlayAnimation(animationName, 0.1f);
        }

        public override void Tick()
        {
            base.Tick();

            if (momentum.magnitude > 0.1f && input.MoveInput.magnitude > 0.1f)
            {
                HandleMovement(baseSpeed);
            }
            else
            {
                Move();
            }
        }
    }
}
