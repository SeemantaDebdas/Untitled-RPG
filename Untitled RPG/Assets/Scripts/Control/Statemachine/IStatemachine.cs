using RPG.Data;

namespace RPG.Control
{
    public interface IStatemachine 
    {
        State CurrentState { get; }
        Context Context { get; }
        void SwitchState(State newState);
    }
}
