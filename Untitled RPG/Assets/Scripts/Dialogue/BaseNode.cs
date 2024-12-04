using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.DialogueSystem
{
    public class BaseNode : ScriptableObject
    {
        [field: SerializeField] public List<string> Children { get; protected set; } = new();
        [field: SerializeField] public Vector2 Position { get; protected set; }
        
        public void SetPosition(Vector2 position)
        { 
            Undo.RecordObject(this, "Update root node position");
            Position = position;
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