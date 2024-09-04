using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerFallState : PlayerBaseState
    {
        [SerializeField] string animationName = "Fall";

        Vector3 momentum = Vector3.zero;
        public override void Enter()
        {
            base.Enter();

            momentum = controller.velocity;
            momentum.y = 0f;

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
