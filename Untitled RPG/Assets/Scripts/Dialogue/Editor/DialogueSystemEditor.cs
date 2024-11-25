using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

namespace RPG.DialogueSystem.Editor
{
    public class DialogueSystemEditor : EditorWindow
    {
        [SerializeField] private VisualTreeAsset visualTreeAsset = default;

        private DialogueGraphView graphView;
        DialogueInspectorView inspectorView;

        [MenuItem("Window/DialogueSystemEditor")]
        public static void OpenWindow()
        {
            GetWindow<DialogueSystemEditor>("DialogueSystemEditor");
        }
        
        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            if (EditorUtility.InstanceIDToObject(instanceID) is not Dialogue) 
                return false;
            
            OpenWindow();
            return true;

        }

        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;
            visualTreeAsset.CloneTree(root);
            
            graphView = root.Q<DialogueGraphView>();
            inspectorView = root.Q<DialogueInspectorView>();
            
            graphView.OnNodeSelected += GraphView_OnNodeSelected;
            

            OnSelectionChange();
        }


        private void OnSelectionChange()
        {
            if (Selection.activeObject is Dialogue dialogue && AssetDatabase.CanOpenAssetInEditor(dialogue.GetInstanceID()))
            {
                graphView.Populate(dialogue);
            }
        }
        
        private void GraphView_OnNodeSelected(DialogueNodeView nodeView)
        {
            //inspectorView.UpdateSelection(nodeView);
        }
    }
}