using UnityEngine;
using System.Collections;
using UnityEditor;

[InitializeOnLoad]
public class ToolsMenu : Editor {

    public static int SelectedTool {
        get { return EditorPrefs.GetInt("SelectedEditorTool", 0); }
        set {
            if(value == SelectedTool) return;
            EditorPrefs.SetInt("SelectedEditorTool", value);

            switch(value) {
                case 0:
                    EditorPrefs.SetBool("IsLevelEditorEnabled", false);

                    Tools.hidden = false;
                    break;
                case 1:
                    EditorPrefs.SetBool("IsLevelEditorEnabled", true);
                    EditorPrefs.SetBool("SelectBlockNextToMousePosition", false);
                    EditorPrefs.SetFloat("CubeHandleColorR", Color.magenta.r);
                    EditorPrefs.SetFloat("CubeHandleColorG", Color.magenta.g);
                    EditorPrefs.SetFloat("CubeHandleColorB", Color.magenta.b);

                    //Hide Unitys Tool handles (like the move tool) while we draw our own stuff
                    Tools.hidden = true;
                    break;
                default:
                    EditorPrefs.SetBool("IsLevelEditorEnabled", true);
                    EditorPrefs.SetBool("SelectBlockNextToMousePosition", true);
                    EditorPrefs.SetFloat("CubeHandleColorR", Color.yellow.r);
                    EditorPrefs.SetFloat("CubeHandleColorG", Color.yellow.g);
                    EditorPrefs.SetFloat("CubeHandleColorB", Color.yellow.b);

                    //Hide Unitys Tool handles (like the move tool) while we draw our own stuff
                    Tools.hidden = true;
                    break;
            }
        }
    }

    static ToolsMenu() {
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
        SceneView.onSceneGUIDelegate += OnSceneGUI;

        EditorApplication.hierarchyWindowChanged -= OnSceneChanged;
        EditorApplication.hierarchyWindowChanged += OnSceneChanged;
    }

    void OnDestroy() {
        SceneView.onSceneGUIDelegate -= OnSceneGUI;

        EditorApplication.hierarchyWindowChanged -= OnSceneChanged;
    }

    static void OnSceneChanged() {
        Tools.hidden = ToolsMenu.SelectedTool != 0;
    }

    static void OnSceneGUI(SceneView sceneView) {
        DrawToolsMenu(sceneView.position);
    }

    static void DrawToolsMenu(Rect position) {
        Handles.BeginGUI();

        GUILayout.BeginArea(new Rect(0, position.height - 35, position.width, 20), EditorStyles.toolbar);

        string[] buttonLabels = new string[] { "None", "Erase", "Paint" };
        SelectedTool = GUILayout.SelectionGrid(SelectedTool, buttonLabels, 3, EditorStyles.toolbarButton, GUILayout.Width(300));

        GUILayout.EndArea();

        Handles.EndGUI();
    }
}
