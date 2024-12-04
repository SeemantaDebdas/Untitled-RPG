using UnityEngine;
using UnityEngine.UIElements;


namespace RPG.DialogueSystem.Editor
{
    [UxmlElement]
    public partial class DialogueInspectorView : VisualElement
    {
        private UnityEditor.Editor editor;
        public DialogueInspectorView(){}

        public void UpdateSelection(BaseNodeView nodeView)
        {
            Clear();
            
            Object.DestroyImmediate(editor);
            
            editor = UnityEditor.Editor.CreateEditor(nodeView.node);

            IMGUIContainer container = new IMGUIContainer(() =>
            {
                if(editor.target != null)
                    editor.OnInspectorGUI();
            });
            Add(container);
        }
    }
}
