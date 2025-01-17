using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.DialogueSystem
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Debdas/Dialogue")]
    public class Dialogue : ScriptableObject , ISerializationCallbackReceiver
    {
        [field: SerializeField] public RootNode RootNode { get; private set; }
        [field: SerializeField] public List<DialogueNode> Nodes { get; private set; } = new();
        
        Dictionary<string, DialogueNode> nodesDictionary = new();

        private void Awake()
        {
            OnValidate();
        }

        private void OnValidate()
        {
            nodesDictionary.Clear();

            // Ensure Nodes list is not null
            Nodes ??= new List<DialogueNode>();

            foreach (DialogueNode dialogueNode in Nodes)
            {
                if (dialogueNode != null)
                {
                    nodesDictionary[dialogueNode.name] = dialogueNode;
                }
            }
        }

        public IEnumerable<DialogueNode> GetAllNodes() => Nodes;

        public IEnumerable<DialogueNode> GetChildrenOfNode(BaseNode parent)
        {
            foreach (string uID in parent.Children)
            {
                if(nodesDictionary.TryGetValue(uID, out var value))
                    yield return value;
            }
        }

        public IEnumerable<DialogueNode> GetChoiceChildrenOfNode(DialogueNode parent)
        {
            IEnumerable<DialogueNode> choices = GetChildrenOfNode(parent);

            foreach (DialogueNode dialogueNode in choices)
            {
                if(dialogueNode.IsPlayerSpeaking)
                    yield return dialogueNode;
            }
        }

        public IEnumerable<DialogueNode> GetNonChoiceChildrenOfNode(BaseNode parent)
        {
            IEnumerable<DialogueNode> choices = GetChildrenOfNode(parent);

            foreach (DialogueNode dialogueNode in choices)
            {
                if(!dialogueNode.IsPlayerSpeaking)
                    yield return dialogueNode;
            }
        }

        public DialogueNode CreateAndAddNode(BaseNode parent)
        {
            var newNode = CreateNode(parent);

            Undo.RegisterCreatedObjectUndo(newNode, "Create Dialogue Node");
            Undo.RecordObject(this, "Add Dialogue Node");
            
            AddNode(newNode);
            
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            
            return newNode;
        }
        
        private DialogueNode CreateNode(BaseNode parent)
        {
            DialogueNode newNode = CreateInstance<DialogueNode>();
            newNode.name = Guid.NewGuid().ToString();

            if (parent != null)
            {
                parent.AddChild(newNode);
                
                if(parent is DialogueNode parentDialogueNode)
                    newNode.SetIsPlayerSpeaking(!parentDialogueNode.IsPlayerSpeaking);
            }
            
            //AssetDatabase.SaveAssets();
            return newNode;
        }

        private void AddNode(DialogueNode newNode)
        {
            Nodes.Add(newNode);
            OnValidate();
            
            //AssetDatabase.SaveAssets();
        }
        
        public void DeleteNode(DialogueNode nodeToDelete)
        {
            if (!Nodes.Contains(nodeToDelete))
            {
                Debug.Log("Node not saved in asset. Returning");
                return;
            }
            
            Undo.RecordObject(this, "Delete Dialogue Node");
            
            Nodes.Remove(nodeToDelete);
            RemoveNodeFromChildrenList(nodeToDelete);
            OnValidate();
            Undo.DestroyObjectImmediate(nodeToDelete);
            
            AssetDatabase.SaveAssets();
        }

        private void RemoveNodeFromChildrenList(DialogueNode nodeToDelete)
        {
            foreach (DialogueNode node in Nodes)
            {
                node.RemoveChild(nodeToDelete);
            }
            
            AssetDatabase.SaveAssets();
        }


        public void OnBeforeSerialize()
        {
            if (RootNode == null)
            {
                CreateRootNode();
            }
            
            if (AssetDatabase.GetAssetPath(this) != "")
            {
                if (AssetDatabase.GetAssetPath(RootNode) == "")
                {
                    AssetDatabase.AddObjectToAsset(RootNode, this);
                    EditorUtility.SetDirty(this);
                }
                
                foreach (DialogueNode node in Nodes)
                {
                    if (node != null && AssetDatabase.GetAssetPath(node) == "")
                    {
                        AssetDatabase.AddObjectToAsset(node, this);
                        EditorUtility.SetDirty(this);
                    }
                }
            }
        }

        private RootNode CreateRootNode()
        {
            RootNode = CreateInstance<RootNode>();
            RootNode.name = Guid.NewGuid().ToString();
            
            //AssetDatabase.SaveAssets();
            return RootNode;
        }

        public void OnAfterDeserialize(){}
    }
}
