using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;
using UnityEditor;
using UnityEngine.Serialization;

namespace RPG.DialogueSystem
{
    public class DialogueNode : BaseNode
    {
        [field: SerializeField] public string Text { get; private set; }
        [field: SerializeField] public bool IsPlayerSpeaking { get; private set; }

        [Space]
        [SerializeField] private ScriptableEvent onEnterEvent;
        [SerializeField] private ScriptableEvent onExitEvent;

        public ScriptableEvent OnEnterEvent
        {
            get => onEnterEvent;
            set
            {
                if (onEnterEvent == value) return;

                Debug.Log($"OnEnterEvent changed from {onEnterEvent} to {value}");
                Undo.RecordObject(this, "Set OnEnterEvent");
                onEnterEvent = value;
                EditorUtility.SetDirty(this);
            }
        }

        public ScriptableEvent OnExitEvent
        {
            get => onExitEvent;
            set
            {
                if (onExitEvent == value) return;
                Undo.RecordObject(this, "Set OnExitEvent");
                onExitEvent = value;
                EditorUtility.SetDirty(this);
            }
        }

        public void SetText(string newText)
        {
            if(Text == newText)
                return;
            
            Undo.RecordObject(this, "Update Node Text");
            Text = newText;
            
            EditorUtility.SetDirty(this);
        }
        
        public void SetIsPlayerSpeaking(bool isPlayerSpeaking)
        {
            Undo.RecordObject(this, "Change Dialogue Speaker");
            
            IsPlayerSpeaking = isPlayerSpeaking;
            EditorUtility.SetDirty(this);
        } 
    }
}