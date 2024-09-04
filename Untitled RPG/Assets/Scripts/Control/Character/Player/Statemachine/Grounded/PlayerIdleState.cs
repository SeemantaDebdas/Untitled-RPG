using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Control
{
    public class PlayerIdleState : PlayerBaseState
    {
        [SerializeField] string animationName = "Idle";
        public override void Initialize(IStatemachine statemachine)
        {
            base.Initialize(statemachine);
        }

        public override void Enter()
        {
            base.Enter();
            animator.PlayAnimation(animationName); 
        }

        public override void Tick()
        {
            base.Tick();

            HandleMovement();
        }
    }
}
