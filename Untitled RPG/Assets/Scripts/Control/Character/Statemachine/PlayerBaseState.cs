using RPG.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public abstract class PlayerBaseState : State
    {
        [field: SerializeField] protected string animationName;

        protected IStatemachine statemachine;
        protected PlayerContext context;
        protected Animator animator;
        public override void Initialize(IStatemachine statemachine)
        {
            this.statemachine = statemachine;
            context = statemachine.Context as PlayerContext;

            if (context == null)
                Debug.Log("Context is null");

            animator = context.Animator;
        }

        public override void Enter()
        {
            base.Enter();

            Debug.Log("Enter: " + GetType().Name);
        }

        protected void PlayAnimation(string animationName, float transitionDuration = 0.25f, int layer = 0) 
        {
            animator.CrossFadeInFixedTime(animationName, transitionDuration, layer);
        }
    }
}
