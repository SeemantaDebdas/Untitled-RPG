using UnityEngine;
using RPG.Core;
using RPG.Data;

namespace RPG.Control
{
    public class PlayerIdleState : PlayerBaseState
    {
        public override void Initialize(IStatemachine statemachine)
        {
            base.Initialize(statemachine);
        }

        public override void Enter()
        {
            base.Enter();
            animator.PlayAnimation(CharacterAnimationData.Instance.Locomotion.Idle); 
        }

        public override void Tick()
        {
            base.Tick();

            HandleMovement();
        }
    }
}
