using System;
using UnityEngine;

namespace RPG.Core
{
    public interface IValueSetter<T>
    {
    }

    public abstract class ScriptableVariable<T> : ScriptableObject
    {
        [SerializeField] protected T value;

        public T Value => value;

        public event Action<T> OnValueChanged;

        public void SetValue(T value, UnityEngine.Object caller)
        {
            if(caller is IValueSetter<T> setter)
            {
                this.value = value;
                OnValueChanged?.Invoke(value);
                //Debug.Log(name + " Setting value: " + value);
            }
        }
    }
}
