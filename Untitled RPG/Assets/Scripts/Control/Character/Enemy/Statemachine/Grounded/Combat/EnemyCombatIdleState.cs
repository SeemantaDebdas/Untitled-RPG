using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class EnemyCombatIdleState : EnemyBaseState
    {
        [SerializeField] string animationName = string.Empty;
        public override void Enter()
        {
            base.Enter();

            animator.PlayAnimation(animationName);
        }
    }
}
