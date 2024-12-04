using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace RPG.DialogueSystem.Editor
{
    public class BaseNodeView : Node
    {
        public BaseNode node;
        
        public event Action<BaseNodeView> OnNodeSelected;
        public event Action<BaseNodeView> OnDragStarted;
        public event Action<Vector2> OnDragEnded;
        
        public Port inputPort, outputPort;

        public BaseNodeView()
        {
        }

        public BaseNodeView(BaseNode baseNode, string pathToUxml) : base(pathToUxml)
        {
            
        }
        
        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            
            node.SetPosition(new (newPos.center.x, newPos.center.y));
        }
        
        public override void OnSelected()
        {
            base.OnSelected();
            OnNodeSelected?.Invoke(this);
        }
        
        protected void CreateInputPort()
        {
            inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            inputPort.portName = "";
            inputContainer.Add(inputPort);
        }
        
        protected void CreateOutputPort()
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
