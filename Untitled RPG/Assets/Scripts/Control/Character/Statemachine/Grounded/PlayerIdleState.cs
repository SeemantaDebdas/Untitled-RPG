using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            PlayAnimation(animationName); 
        }
    }
}
