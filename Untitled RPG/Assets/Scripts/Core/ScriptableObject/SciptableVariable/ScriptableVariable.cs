using System;
using UnityEngine;

namespace RPG.Core
{
    public interface IValueSetter<T>
    {
        void SetValue(T value);
    }

    public abstract class ScriptableVariable<T> : ScriptableObject
    {
        [SerializeField] protected T value;

        public T Value => value;

        public event Action<T> OnValueChanged;

        public void SetValue(T value, UnityEngine.Object caller)
        {
            if(caller is IListValueSetter<T> setter)
            {
                this.value = value;
                OnValueChanged?.Invoke(value);
            }
            else
            {
                Debug.LogWarning($"{caller.name} is not authorized to set this value!");
            }
        }
    }
}
