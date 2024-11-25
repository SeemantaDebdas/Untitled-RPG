using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Serialization;

namespace RPG.DialogueSystem
{
    public class DialogueNode : ScriptableObject
    {
        [field: SerializeField] public string Text { get; private set; }
        [field: SerializeField] public List<string> Children { get; private set; } = new();
        [field: SerializeField] public bool IsPlayerSpeaking { get; private set; }
        [field: SerializeField] public Vector2 Position { get; private set; }

        public void SetPosition(Vector2 position)
        { 
            Undo.RecordObject(this, "Update root node position");
            Position = position;
            EditorUtility.SetDirty(this);
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
        
        public bool IsChild(DialogueNode node) => Children.Contains(node.name);

        public void AddChild(DialogueNode child)
        {
            if (Children.Contains(child.name))
                return;

            Undo.RecordObject(this, "Link Dialogue Node");
            Children.Add(child.name);
            
            EditorUtility.SetDirty(this);
        }

        public void RemoveChild(DialogueNode child)
        {
            if (!Children.Contains(child.name))
                return;
            
            Undo.RecordObject(this, "Unlink Dialogue Node");    
            Children.Remove(child.name);
            
            EditorUtility.SetDirty(this);
        }
    }
}