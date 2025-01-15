using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    [CreateAssetMenu(fileName = "NewScriptableEvent", menuName = "Debdas/Variables/ScriptableEvent")]
    public class ScriptableEvent : ScriptableObject
    {
        readonly List<ScriptableEventListener> listeners = new List<ScriptableEventListener>();

        public void Raise(Component caller, object data)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised(this, caller, data);
            }
        }

        public void RegisterListener(ScriptableEventListener listener)
        {
            if (listeners.Contains(listener))
                return;

            listeners.Add(listener);
        }

        public void UnregisterListerner(ScriptableEventListener listener)
        {
            if (!listeners.Contains(listener))
                return;

            listeners.Remove(listener);
        }

    }
}
