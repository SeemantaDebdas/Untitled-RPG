using UnityEngine;

namespace RPG.Control
{
    public abstract class State : MonoBehaviour
    {
        public abstract void Initialize(IStatemachine statemachine);
        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void Tick() { }
        public virtual void FixedTick() { }
        public virtual void HandleInput() { }
    }
}
