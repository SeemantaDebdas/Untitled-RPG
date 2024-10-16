using RPG.Data;
using UnityEngine;

namespace RPG.Control
{
    public abstract class Statemachine : MonoBehaviour, IStatemachine
    {
        [SerializeField] protected State initialState = null;

        protected State currentState;
        public State CurrentState { get { return currentState; } }

        Context context;
        public virtual Context Context
        {
            get
            {
                if(context != null)
                    return context;

                context = new Context
                {
                    Transform = transform,
                };

                return context;
            }
        } 

        public virtual void SwitchState(State newState)
        {
            newState.Initialize(this);

            currentState?.Exit();
            currentState = newState;
            currentState?.Enter();
        }

        public void Update()
        {
            currentState?.HandleInput();
            currentState?.Tick();
        }

        public void FixedUpdate()
        {
            currentState?.FixedTick();
        }
    }
}
