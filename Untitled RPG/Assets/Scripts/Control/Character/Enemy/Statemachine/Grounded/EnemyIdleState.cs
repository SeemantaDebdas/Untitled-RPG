using RPG.Core;
using RPG.Data;
using UnityEngine;

namespace RPG.Control
{
    public class EnemyIdleState : EnemyBaseState
    {
        public override void Enter()
        {
            base.Enter();
            animator.PlayAnimation(CharacterAnimationData.Instance.Locomotion.Idle);
            
            fieldOfView.ResetAngle();
            
            SubscribeToHurtEvent();
        }

        public override void Exit()
        {
            base.Exit();
            
            UnsubscribeToHurtEvent();
        }
    }
}
