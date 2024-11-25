using UnityEngine;
using UnityEngine.UIElements;


namespace RPG.DialogueSystem.Editor
{
    [UxmlElement]
    public partial class DialogueInspectorView : VisualElement
    {
        private UnityEditor.Editor editor;
        public DialogueInspectorView(){}

        public void UpdateSelection(DialogueNodeView nodeView)
        {
            Clear();
            
            Object.DestroyImmediate(editor);
            
            editor = UnityEditor.Editor.CreateEditor(nodeView.node);

            IMGUIContainer container = new IMGUIContainer(editor.OnInspectorGUI);
            Add(container);
        }
    }
}
