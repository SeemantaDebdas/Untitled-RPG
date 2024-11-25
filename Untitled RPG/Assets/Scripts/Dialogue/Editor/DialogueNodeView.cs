using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace RPG.DialogueSystem.Editor
{
    [UxmlElement]
    public partial class DialogueNodeView : Node
    {
        public DialogueNode node;

        public event Action<DialogueNodeView> OnNodeSelected;
        public event Action<DialogueNodeView> OnDragStarted;
        public event Action<Vector2> OnDragEnded;

        public Port inputPort, outputPort;
        public string dialogueText;

        public DialogueNodeView()
        {
        }

        public DialogueNodeView(DialogueNode node) : base("Assets/Scripts/Dialogue/Editor/NodeViewEditor.uxml")
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

            style.left = this.node.Position.x;
            style.top = this.node.Position.y;
            
            CreateInputPort();
            CreateOutputPort();
            CreateTextArea();

            topContainer.style.flexDirection = FlexDirection.Row;
        }
        
        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            
            node.SetPosition(new (newPos.center.x, newPos.center.y));
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
                node.SetText(evt.newValue);
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

        private void CreateInputPort()
        {
            inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            inputPort.portName = "";
            inputContainer.Add(inputPort);
        }
        
        private void CreateOutputPort()
        {
            outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
            outputPort.portName = "";
            
            // Register the drag start and end events
            outputPort.RegisterCallback<MouseDownEvent>(evt => OnDragStarted?.Invoke(this));
            outputPort.RegisterCallback<MouseUpEvent>(evt => OnDragEnded?.Invoke(evt.mousePosition));
            
            outputContainer.Add(outputPort);
        }
    }
}
