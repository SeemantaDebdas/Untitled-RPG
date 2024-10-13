using UnityEngine;
using RPG.Core;

namespace RPG.Control
{
    public class PlayerIdleState : PlayerBaseState
    {
        [SerializeField] ScriptableString animationName;
        public override void Initialize(IStatemachine statemachine)
        {
            base.Initialize(statemachine);
        }

        public override void Enter()
        {
            base.Enter();
            animator.PlayAnimation(animationName.Value); 
        }

        public override void Tick()
        {
            base.Tick();

            HandleMovement();
        }
    }
}
