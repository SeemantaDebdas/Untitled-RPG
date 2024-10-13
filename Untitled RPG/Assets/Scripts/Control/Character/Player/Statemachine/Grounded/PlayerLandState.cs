using RPG.Core;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerLandState : PlayerBaseState
    {
        [SerializeField] ScriptableString landIdle;
        [SerializeField] ScriptableString landMove;
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
                animationName = landIdle.Value;
            }
            else
            {
                animationName = landMove.Value;
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
