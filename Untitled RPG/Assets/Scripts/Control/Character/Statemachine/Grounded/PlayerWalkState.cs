using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerWalkState : PlayerBaseState
    {
        [SerializeField] float baseSpeed = 1f;

        public override void Enter()
        {
            base.Enter();

            PlayAnimation(animationName);
        }

        public override void Tick()
        {
            base.Tick();
            HandleMovement(baseSpeed);
        }
    }
}
