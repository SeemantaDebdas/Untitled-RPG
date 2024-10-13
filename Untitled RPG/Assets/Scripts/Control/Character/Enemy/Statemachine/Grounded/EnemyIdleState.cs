using RPG.Core;
using UnityEngine;

namespace RPG.Control
{
    public class EnemyIdleState : EnemyBaseState
    {
        [SerializeField] string animationName = "Idle";
        public override void Enter()
        {
            base.Enter();
            animator.PlayAnimation(animationName);
        }
    }
}
