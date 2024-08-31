using RPG.Data;

namespace RPG.Control
{
    public interface IStatemachine 
    {
        Context Context { get; }
        void SwitchState(State newState);
    }
}
