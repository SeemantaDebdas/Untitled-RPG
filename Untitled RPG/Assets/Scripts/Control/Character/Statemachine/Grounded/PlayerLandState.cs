using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerLandState : PlayerBaseState
    {
        [SerializeField] string idleLand = "IdleLand";
        [SerializeField] string moveLand = "MoveLand";
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
                animationName = idleLand;
            }
            else
            {
                animationName = moveLand;
            }

            PlayAnimation(animationName, 0.1f);
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
