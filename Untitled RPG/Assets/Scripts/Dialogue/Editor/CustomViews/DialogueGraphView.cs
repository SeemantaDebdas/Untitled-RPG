using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace RPG.DialogueSystem.Editor
{
    [UxmlElement]
    public partial class DialogueGraphView : GraphView
    {
        Dialogue dialogue;
        public Action<BaseNodeView> OnNodeSelected;

        public DialogueGraphView()
        {
            style.flexGrow = 1;
            Insert(0, new GridBackground());
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Dialogue/Editor/DialogueSystemEditor.uss");
            styleSheets.Add(styleSheet);

            AddManipulators();

            Undo.undoRedoPerformed += () =>
            {
                if (dialogue == null)
                    return;
                
                Populate(dialogue);
                AssetDatabase.SaveAssets();
            };
        }

        private void AddManipulators()
        {
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
        }

        public void Populate(Dialogue dialogue)
        {
            this.dialogue = dialogue;

            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnGraphViewChanged;

            //Create view for root node and all other nodes.
            CreateRootNodeView(dialogue.RootNode);
            dialogue.Nodes.ForEach(node => CreateDialogueNodeView(node));
            
            //Create edges for root node and all other nodes
            CreateEdges(dialogue.RootNode);
            dialogue.Nodes.ForEach(CreateEdges);
        }

        private RootNodeView CreateRootNodeView(RootNode rootNode)
        {
            RootNodeView nodeView = new RootNodeView(rootNode);
            
            SubscribeToNodeViewEvents(nodeView);

            AddElement(nodeView);
            
            return nodeView;
        }

        private DialogueNodeView CreateDialogueNodeView(DialogueNode node)
        {
            DialogueNodeView nodeView = new DialogueNodeView(node);
            
            SubscribeToNodeViewEvents(nodeView);

            AddElement(nodeView);
            
            return nodeView;
        }

        private void SubscribeToNodeViewEvents(BaseNodeView nodeView)
        {
            nodeView.OnNodeSelected += OnNodeSelected;
            // Subscribe to drag events
            nodeView.OnDragStarted += OnNodeDragStarted;
            nodeView.OnDragEnded += OnNodeDragEnded;
            
            nodeView.RegisterCallback<GeometryChangedEvent>((e) =>
            {
                // Ensure the node's center aligns with the mouse position
                float nodeWidth = nodeView.resolvedStyle.width;
                float nodeHeight = nodeView.resolvedStyle.height;

                // Calculate the new position so the center aligns with the mouse
                Vector2 nodePosition = nodeView.node.Position - new Vector2(nodeWidth / 2, nodeHeight / 2);

                // Set the node position after the size is determined
                nodeView.node.SetPosition(nodePosition);
                nodeView.SetPosition(new Rect(nodePosition, new Vector2(nodeWidth, nodeHeight)));
            });
        }

        void CreateEdges(BaseNode parent)
        {
            List<DialogueNode> children = dialogue.GetChildrenOfNode(parent).ToList();
            
            if(children.Count == 0)
                return;

            BaseNodeView parentView = GetNodeViewFromNode(parent);
                
            children.ForEach(child =>
            {
                DialogueNodeView childView = GetNodeViewFromNode(child) as DialogueNodeView;

                if (parentView == null)
                {
                    Debug.LogError("Parent View Null.");
                    return;
                }

                if (childView == null)
                {
                    Debug.LogError("Child View Null.");
                    return;
                }
                
                if (parentView.outputPort == null || childView.inputPort == null)
                {
                    Debug.LogError("Ports are not initialized correctly.");
                    return;
                }
                
                Edge edge = parentView.outputPort.ConnectTo(childView.inputPort);
                AddElement(edge);
            });
        }
        
        private DialogueNode CreateNode(Vector2 instantiationPosition, BaseNode parent = null)
        {
            DialogueNode node = dialogue.CreateAndAddNode(parent);
            node.SetPosition(instantiationPosition);

            return node;
        }

        #region Graph View Methods

        private GraphViewChange OnGraphViewChanged(GraphViewChange changes)
        {
            HandleNodeRemoval(changes);
            AddChildrenAfterEdgeCreation(changes);
            
            return changes;
        }
        
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            Vector2 mousePosition = viewTransform.matrix.inverse.MultiplyPoint(evt.localMousePosition);
            //Vector2 mousePosition = contentViewContainer.WorldToLocal(evt.localMousePosition);

            evt.menu.AppendAction($"New Node", action =>
            {
                var newNode = CreateNode(mousePosition);
                CreateDialogueNodeView(newNode);
            });
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList().Where(endPort => 
                endPort.direction != startPort.direction
                && endPort.node != startPort.node).ToList();
        }
        

        #endregion

        #region Drag and Drop to Create Node
        
        BaseNodeView draggingNodeView = null;
        
        bool IsMouseOnAnyGraphElement(Vector2 mousePosition)
        {
            //Vector2 localMousePosition = contentViewContainer.WorldToLocal(mousePosition);

            bool clickedOnNode = nodes.ToList().Any(node => node.GetPosition().Contains(mousePosition));
            bool clickedOnPort = ports.ToList().Any(port => port.worldBound.Contains(mousePosition));
            bool clickedOnEdge = edges.ToList().Any(edge => edge.worldBound.Contains(mousePosition));

            if (!clickedOnNode && !clickedOnPort && !clickedOnEdge)
            {
                // No node, port, or edge under the mouse, consider it an empty space click
                Debug.Log("Clicked on empty space.");
                return false;
                //OnEmptySpaceClicked?.Invoke(localMousePosition);
            }

            Debug.Log("Clicked on a graph element.");

            return true;
        }

        private void OnNodeDragStarted(BaseNodeView nodeView)
        {
            // Track the node being dragged
            draggingNodeView = nodeView;
        }

        private void OnNodeDragEnded(Vector2 releasePosition)
        {
            if (draggingNodeView == null) return;
            
            Vector2 position = contentViewContainer.WorldToLocal(releasePosition);
            //Vector2 position = viewTransform.matrix.inverse.MultiplyPoint(releasePosition);

            // Check if the release position is in empty space
            var hitElement = IsMouseOnAnyGraphElement(position);

            if (!hitElement)
            {
                // Create a new child node if the release position is in empty space
                CreateChildNode(draggingNodeView, position);
            }

            // Clear the dragging reference
            draggingNodeView = null;
        }

        #endregion
        private void HandleNodeRemoval(GraphViewChange changes)
        {       
            if (changes.elementsToRemove != null)
            {
                foreach (var element in changes.elementsToRemove)
                {
                    if (element is DialogueNodeView nodeView)
                    {
                        nodeView.OnNodeSelected -= OnNodeSelected;
                        // Unsubscribe to drag events
                        nodeView.OnDragStarted -= OnNodeDragStarted;
                        nodeView.OnDragEnded -= OnNodeDragEnded;
                        
                        dialogue.DeleteNode(nodeView.node as DialogueNode);//<==================!!!!!!!!!!!!!!!
                    }
                    
                    if (element is Edge edge)
                    {
                        RemoveChildrenAfterEdgeRemoval(edge);
                    }
                }
            }
        }
        
        private void AddChildrenAfterEdgeCreation(GraphViewChange changes)
        {
            if (changes.edgesToCreate != null)
            {
                foreach (var edgeToCreate in changes.edgesToCreate)
                {
                    BaseNodeView parentView = edgeToCreate.output.node as BaseNodeView;
                    DialogueNodeView childView = edgeToCreate.input.node as DialogueNodeView;

                    //dialogue.AddChild(parentView.node, childView.node);
                    if (!parentView.node.IsChild(childView.node as DialogueNode))
                    {
                        parentView.node.AddChild(childView.node as DialogueNode);
                    }
                }
            }
        }
        

        
        private void RemoveChildrenAfterEdgeRemoval(Edge removedEdge)
        {
            BaseNodeView parentView = removedEdge.output.node as BaseNodeView;
            DialogueNodeView childView = removedEdge.input.node as DialogueNodeView;
            
            parentView.node.RemoveChild(childView.node as DialogueNode);
        }
        
        private void CreateChildNode(BaseNodeView parentView, Vector2 position)
        {
            DialogueNode newChildNode = CreateNode(position, parentView.node);
            DialogueNodeView childNodeView = CreateDialogueNodeView(newChildNode);

            // Connect parent to child
            Edge edge = parentView.outputPort.ConnectTo(childNodeView.inputPort);
            AddElement(edge);

            parentView.node.AddChild(newChildNode);
        }
        
        private BaseNodeView GetNodeViewFromNode(BaseNode node)
        {
            return GetNodeByGuid(node.name) as BaseNodeView;
        }

    }
}
