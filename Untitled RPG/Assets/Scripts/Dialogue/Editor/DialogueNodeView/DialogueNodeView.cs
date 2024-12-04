using System;
using RPG.Core;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace RPG.DialogueSystem.Editor
{
    [UxmlElement]
    public partial class DialogueNodeView : BaseNodeView
    {
        public EventObjectField onEnterField, onExitField;
        public string dialogueText;

        public DialogueNodeView()
        {
        }

        public DialogueNodeView(DialogueNode node) : base(node,"Assets/Scripts/Dialogue/Editor/DialogueNodeView/NodeViewEditor.uxml")
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node), "DialogueNode cannot be null.");
            
            this.node = node;
            title = "Dialogue Node";
            
            if(node.IsPlayerSpeaking)
                titleContainer.AddToClassList("optionNode");
            else 
                titleContainer.AddToClassList("defaultNode");

            viewDataKey = node.name;
            dialogueText = node.Text;
            
            onEnterField = this.Q<EventObjectField>("on-enter-event");
            onExitField = this.Q<EventObjectField>("on-exit-event");
            
            onEnterField.value = node.OnEnterEvent;
            onExitField.value = node.OnExitEvent;

            style.left = this.node.Position.x;
            style.top = this.node.Position.y;
            
            CreateInputPort();
            CreateOutputPort();
            CreateTextArea();
            BindOnEnterAndExitEvents();

            topContainer.style.flexDirection = FlexDirection.Row;
        }
        
        private void CreateTextArea()
        {
            TextField textField = new TextField()
            {
                value = dialogueText,
            };
            
            textField.AddToClassList("nodeTextField");
            
            // Add a callback to save changes to the DialogueNode
            textField.RegisterValueChangedCallback(evt =>
            {
                // Update the node's text
                ((DialogueNode)node).SetText(evt.newValue);
            });

            VisualElement customDataContainer = new VisualElement()
            {
                name = "text-area"
            };
            
            customDataContainer.AddToClassList("dsCustomDataContainer");
            
            Foldout textFoldout = new Foldout()
            {
                text = "Dialogue Text",
                value = false
            };
            
            // textField.AddToClassList("dsNodeTextField");
            // textField.AddToClassList("dsNodeQuoteTextField");
            
            textFoldout.Add(textField);

            customDataContainer.Add(textFoldout);
            customDataContainer.AddToClassList("textFieldContainer");
            
            if (extensionContainer != null)
            {
                extensionContainer.Add(customDataContainer);
                RefreshExpandedState(); // Ensures the container is visible
            }
            else
            {
                Debug.LogError("Extension container is null!");
            }
        }

        void BindOnEnterAndExitEvents()
        {
            DialogueNode dialogueNode = ((DialogueNode)node);
            
            if (onEnterField == null || onExitField == null)
            {
                Debug.LogError("EventObjectFields are not correctly initialized.");
                return;
            }
            
            onEnterField.RegisterCallback<ChangeEvent<Object>>(evt =>
            {
                Debug.Log("Event Changed");

                if (evt.newValue == null)
                {
                    dialogueNode.OnEnterEvent = null;
                    return;
                }

                if (evt.newValue is not ScriptableEvent newEnterEvent)
                    return;
                
                if (newEnterEvent == dialogueNode.OnEnterEvent) 
                    return;

                Debug.Log("Event not same as that of node");
                
                dialogueNode.OnEnterEvent = newEnterEvent;
            });
            
            onExitField.RegisterCallback<ChangeEvent<Object>>(evt =>
            {
                if (evt.newValue == null)
                {
                    dialogueNode.OnExitEvent = null;
                    return;
                }
                
                if (evt.newValue is not ScriptableEvent newExitEvent)
                    return;
                
                if (newExitEvent == dialogueNode.OnExitEvent) 
                    return;

                dialogueNode.OnExitEvent = newExitEvent;
            });
        }

    }
}
