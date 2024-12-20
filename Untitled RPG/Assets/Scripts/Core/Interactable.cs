using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace RPG.Core
{
    /// <summary>
    /// The idea behind this script is it interaction system can be used both
    /// as a component and as inheritance.
    /// A script can be IInteractable, or it can use this component to decouple it's
    /// logic from interaction logic
    /// </summary>
    public class Interactable : MonoBehaviour, IInteractable
    {
        [FormerlySerializedAs("OnFocus")] [SerializeField] private UnityEvent<Interactor> onFocus;
        [FormerlySerializedAs("OnUnfocus")] [SerializeField] private UnityEvent<Interactor> onUnfocus;
        [FormerlySerializedAs("OnInRange")] [SerializeField] private UnityEvent<Interactor> onInRange;
        [FormerlySerializedAs("OnOutOfRange")] [SerializeField] private UnityEvent<Interactor> onOutOfRange;
        [FormerlySerializedAs("OnInteract")] [SerializeField] private UnityEvent<Interactor> onInteract;

        public event Action<Interactor> OnInteract, OnFocus, OnUnfocus, OnInRange, OnOutOfRange;
        public event Action OnDestroyed;
        
        public void Focus(Interactor interactor)
        {
            onFocus?.Invoke(interactor);
            OnFocus?.Invoke(interactor);
        }

        public void UnFocus(Interactor interactor)
        {
            onUnfocus?.Invoke(interactor);
            OnUnfocus?.Invoke(interactor);
        }

        public void InRange(Interactor interactor)
        {
            onInRange?.Invoke(interactor);
            OnInRange?.Invoke(interactor);
        }

        public void OutOfRange(Interactor interactor)
        {
            onOutOfRange?.Invoke(interactor);
            OnOutOfRange?.Invoke(interactor);
        }

        public void Interact(Interactor interactor)
        {
            onInteract?.Invoke(interactor);
            OnInteract?.Invoke(interactor);
        }

        private void OnDestroy()
        {
            OnDestroyed?.Invoke();
        }
    }
}
