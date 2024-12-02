using RPG.Core;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

namespace RPG.DialogueSystem.Editor
{
    [UxmlElement("EventObjectField")]
    public partial class EventObjectField : ObjectField
    {
        [UxmlAttribute("label-text")]
        public string LabelText
        {
            get => label.text;
            set => label.text = value;
        }

        private Label label;

        public EventObjectField()
        {
            // Create the label
            label = this.Q<Label>();

            // Restrict the type in the built-in object picker
            objectType = typeof(ScriptableEvent);

            // Subscribe to value change event
            RegisterCallback<ChangeEvent<Object>>(OnValueChanged);
        }

        private void OnValueChanged(ChangeEvent<Object> evt)
        {
            // Validate if the assigned object is a ScriptableEvent
            if (evt.newValue is ScriptableEvent scriptableEvent)
            {
                value = scriptableEvent;
                //Debug.Log($"Assigned object is a valid ScriptableEvent: {evt.newValue.name}");
            }
            else if (evt.newValue != null)
            {
                //Debug.LogWarning($"Assigned object is not a ScriptableEvent. Reverting to previous value.");
                SetValueWithoutNotify(evt.previousValue); // Revert to the previous value
            }
            else if (evt.newValue == null)
            {
                Debug.Log("Value is set to null");
                value = null;
            }
        }
    }
}
