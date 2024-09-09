using RPG.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public abstract class State : MonoBehaviour
    {
        [SerializeField] CommonTransitions commonTransitions;
        [SerializeField] protected List<Transition> transitionList;

        protected IStatemachine statemachine = null;

        public virtual void Initialize(IStatemachine statemachine)
        {
            this.statemachine = statemachine;
        }

        public virtual void Enter() { }
        public virtual void Exit() 
        {
            ResetTransitions();
        }

        public virtual void Tick() 
        {
            if (transitionList == null || transitionList.Count < 1)
            {
                Debug.Log("Transition List is null or empty");
                return;
            }

            EvaluateTransitions();
        }
        public virtual void FixedTick() { }
        public virtual void HandleInput() { }

        protected bool EvaluateTransitions()
        {
            foreach (Transition transition in transitionList)
            {
                if (transition.EvaluateConditions(statemachine.Context))
                {
                    SwitchState(transition.ToState);
                    return true;
                }
            }

            if (commonTransitions == null) 
                return false;

            foreach(Transition transition in commonTransitions.TransitionList)
            {
                if (transition.EvaluateConditions(statemachine.Context))
                {
                    SwitchState(transition.ToState);
                    return true;
                }
            }
            return false;
        }

        protected void ResetTransitions()
        {
            foreach (Transition transition in transitionList)
            {
                transition.Reset();
            }

            if (commonTransitions == null)
                return;
            
            foreach (Transition transition in commonTransitions.TransitionList)
            {
                transition.Reset();
            }
        }

        protected void SwitchState(State toState)
        {
            statemachine.SwitchState(toState);
        }
    }

}
