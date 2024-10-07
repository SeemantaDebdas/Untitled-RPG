using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class EnemyLandState : EnemyBaseState
    {
        [SerializeField] string animationName = "IdleLand";

        public override void Enter()
        {
            base.Enter();

            animator.PlayAnimation(animationName, 0.1f);
        }

        public override void Exit()
        {
            base.Exit();

            agent.ResetPath();
        }

        public override void Tick()
        {
            base.Tick();

            Move();
        }
    }
}
