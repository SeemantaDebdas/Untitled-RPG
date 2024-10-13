using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public interface IListValueSetter<T> : IEnumerableValueSetter<T>
    {
        void RemoveItem(T item);
    }

    public abstract class ScriptableList<T> : ScriptableVariable<List<T>>
    {
        public event Action<T> OnItemAdded, OnItemRemoved;
        public event Action OnListCleared;

        private void OnEnable()
        {
            value ??= new List<T>();
        }

        public void AddItem(T item, UnityEngine.Object caller) 
        {
            if (caller is IListValueSetter<T> setter)
            {
                if (value.Contains(item))
                    return;

                value.Add(item);
                OnItemAdded?.Invoke(item);
            }
            else
            {
                Debug.LogWarning($"{caller.name} is not authorized to modify this list!");
            }
        }

        public void RemoveItem(T item, UnityEngine.Object caller)
        {
            if (caller is IListValueSetter<T> setter)
            {
                if (!value.Contains(item))
                    return;

                value.Remove(item);
                OnItemRemoved?.Invoke(item);
            }
            else
            {
                Debug.LogWarning($"{caller.name} is not authorized to modify this list!");
            }

            Debug.Log(value.Count);
        }

        public void ClearList(UnityEngine.Object caller)
        {
            if (caller is IListValueSetter<T> setter)
            {
                value.Clear();
                OnListCleared?.Invoke();
            }
            else
            {
                Debug.LogWarning($"{caller.name} is not authorized to modify this list!");
            }
        }
    }
}
