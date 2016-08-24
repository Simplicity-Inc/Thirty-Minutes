using UnityEngine;
using System.Collections;
using UnityEditor;

[InitializeOnLoad]
public class LevelEditorHandle : Editor {

    public static Vector2 currentHandlePosition = Vector2.zero;
    public static bool IsMouseInValidArea = false;

    static Vector2 m_OldHandlePosition = Vector2.zero;

    static float snapValue = 1;

    static LevelEditorHandle() {
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
        SceneView.onSceneGUIDelegate += OnSceneGUI;
    }

    void OnDestory() {
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
    }
    static void OnSceneGUI(SceneView sceneView) {
        bool isLevelEditorEnabled = EditorPrefs.GetBool("IsLevelEditorEnabled", true);

        if(isLevelEditorEnabled == false) return;

        UpdateHandlePosition(sceneView);
        UpdateIsMouseInValidArea(sceneView.position);
        UpdateRepaint();

        DrawPositionPreview();
    }

    static void UpdateHandlePosition(SceneView sceneView) {
        if(Event.current == null) return;

        Vector3 mousePosition = Event.current.mousePosition;
        mousePosition.y = sceneView.camera.pixelHeight - mousePosition.y;
        mousePosition = sceneView.camera.ScreenToWorldPoint(mousePosition);
        //mousePosition.y = -mousePosition.y;

        float snapInverse = 1 / snapValue;

        float x, y;

        x = Mathf.Round(mousePosition.x * snapInverse) / snapInverse;
        y = Mathf.Round(mousePosition.y * snapInverse) / snapInverse;

        currentHandlePosition = mousePosition = new Vector2(x, y);
    }

    static void UpdateIsMouseInValidArea(Rect sceneView) {
        bool isInValidArea = Event.current.mousePosition.y < sceneView.height - 35;

        if(isInValidArea != IsMouseInValidArea) {
            IsMouseInValidArea = isInValidArea;
            SceneView.RepaintAll();
        }
    }

    static void UpdateRepaint() {
        if(currentHandlePosition != m_OldHandlePosition) {
            SceneView.RepaintAll();
            m_OldHandlePosition = currentHandlePosition;
        }
    }

    static void DrawPositionPreview() {
        if(IsMouseInValidArea == false) return;

        Handles.color = new Color(EditorPrefs.GetFloat("CubeHandleColorR", 1f), EditorPrefs.GetFloat("CubeHandleColorG", 1f), EditorPrefs.GetFloat("CubeHandleColorB", 0f));

        Handles.DrawLine(currentHandlePosition - Vector2.left * 0.5f, currentHandlePosition + Vector2.left * 0.5f);
        Handles.DrawLine(currentHandlePosition - Vector2.up * 0.5f, currentHandlePosition + Vector2.up * 0.5f);
    }
}
