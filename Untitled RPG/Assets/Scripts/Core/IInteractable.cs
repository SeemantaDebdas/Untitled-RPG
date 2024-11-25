using UnityEngine;

namespace RPG.Core
{
    public interface IInteractable
    {
        Transform transform { get; }
        GameObject gameObject { get; }
        void Focus(Interactor interactor);
        void UnFocus(Interactor interactor);
        void InRange(Interactor interactor);
        void OutOfRange(Interactor interactor);
        void Interact(Interactor interactor);
    }
}
