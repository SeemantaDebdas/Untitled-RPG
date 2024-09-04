using UnityEditor.EditorTools;
using UnityEditor.ShortcutManagement;
using UnityEditor;
using UnityEngine;
using RPG.Core;

namespace RPG
{
    [EditorTool("Path Manipulation Tool", typeof(Path))]
    public class PathManipulationTool : EditorTool
    {
        public override GUIContent toolbarIcon => EditorGUIUtility.IconContent("AvatarPivot");


        [Shortcut("Activate Path Manipulation Tool", KeyCode.U)]
        static void PathManipulationToolShortcut()
        {
            if (Selection.GetFiltered<Path>(SelectionMode.TopLevel).Length > 0)
            {
                ToolManager.SetActiveTool<PathManipulationTool>();
            }
        }

        public override void OnToolGUI(EditorWindow window)
        {
            if (window is not SceneView) return;

            foreach (var obj in targets)
            {
                if (obj is not Path path)
                    continue;

                for (int i = 0; i < path.WaypointDataArray.Length; i++)
                {
                    Vector3 waypointPosition = path.WaypointDataArray[i].position;
                    Quaternion waypointRotation = Quaternion.Euler(path.WaypointDataArray[i].rotation);

                    EditorGUI.BeginChangeCheck();

                    Handles.Label(waypointPosition + Vector3.up * 0.5f, "Point: " + (i + 1));
                    DrawConnectionLines(path, i);

                    if (waypointRotation != Quaternion.identity)
                        waypointPosition = Handles.PositionHandle(waypointPosition, waypointRotation);
                    else
                        waypointPosition = Handles.PositionHandle(waypointPosition, Quaternion.identity);

                    waypointRotation = Handles.RotationHandle(waypointRotation, waypointPosition);

                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(path, "Moved Control Point");
                        path.WaypointDataArray[i].position = waypointPosition;
                        path.WaypointDataArray[i].rotation = waypointRotation.eulerAngles;
                    }
                }
            }
        }

        private void DrawConnectionLines(Path path, int index)
        {
            int fromIndex = index;
            int toIndex = (fromIndex + 1) % path.WaypointDataArray.Length;
            Handles.DrawLine(path.WaypointDataArray[fromIndex].position, path.WaypointDataArray[toIndex].position);
        }
    }
}
