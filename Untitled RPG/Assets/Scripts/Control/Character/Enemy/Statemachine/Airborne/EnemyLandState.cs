using RPG.Core;
using RPG.Data;
using UnityEngine;

namespace RPG.Control
{
    public class EnemyLandState : EnemyBaseState
    {
        public override void Enter()
        {
            base.Enter();

            animator.PlayAnimation(CharacterAnimationData.Instance.Locomotion.LandIdle, 0.1f);
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
