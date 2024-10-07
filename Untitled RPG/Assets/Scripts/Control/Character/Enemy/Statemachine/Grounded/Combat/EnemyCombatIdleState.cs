using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class EnemyCombatIdleState : EnemyBaseState
    {
        [SerializeField] string animationName = string.Empty;
        [SerializeField] string moveXParam = "moveX", moveYParam = "moveY";
        public override void Enter()
        {
            base.Enter();

            animator.PlayAnimation(animationName);
        }

        public override void Tick()
        {
            base.Tick();

            animator.SetFloat(moveXParam, 0, 0.085f, Time.deltaTime);
            animator.SetFloat(moveYParam, 0, 0.085f, Time.deltaTime);
        }
    }
}
