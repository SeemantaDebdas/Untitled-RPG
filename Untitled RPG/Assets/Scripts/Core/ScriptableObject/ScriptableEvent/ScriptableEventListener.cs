using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Core
{
    [System.Serializable]
    public class CustomUnityEvent : UnityEvent<Component, object> { }

    public class ScriptableEventListener : MonoBehaviour
    {
        [System.Serializable]
        public class EventResponsePair
        {
            [HideInInspector] public string inspectorName;
            public ScriptableEvent scriptableEvent;
            public CustomUnityEvent response;

            public void SetInspectorName()
            {
                inspectorName = scriptableEvent.name;
            }
        }

        [SerializeField] List<EventResponsePair> eventResponseList;

        void OnEnable()
        {
            foreach (var eventResponsePair in eventResponseList)
            {
                eventResponsePair.scriptableEvent.RegisterListener(this);
            }
        }

        private void OnValidate()
        {
            for(int i = 0; i <  eventResponseList.Count; i++) 
            {
                eventResponseList[i].SetInspectorName();
            }
        }

        void OnDisable()
        {
            foreach (var eventResponsePair in eventResponseList)
            {
                eventResponsePair.scriptableEvent.UnregisterListerner(this);
            }
        }

        public void OnEventRaised(ScriptableEvent gameEvent, Component caller, object data)
        {
            foreach (var eventResponsePair in eventResponseList)
            {
                if (eventResponsePair.scriptableEvent == gameEvent)
                {
                    eventResponsePair.response.Invoke(caller, data);
                }
            }
        }
    }

}
