using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace RPG.DialogueSystem.Editor
{
    [UxmlElement]
    public partial class RootNodeView : BaseNodeView
    {
        public RootNodeView(){}
        public RootNodeView(RootNode node) : base(node,
            "Assets/Scripts/Dialogue/Editor/RootNodeView/RootNodeViewEditor.uxml")
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node), "Root Node cannot be null.");
            
            this.node = node;
            title = "Root Node";

            viewDataKey = node.name;

            style.left = this.node.Position.x;
            style.top = this.node.Position.y;
            
            CreateOutputPort();

            topContainer.style.flexDirection = FlexDirection.Row;
        }
    }
}
