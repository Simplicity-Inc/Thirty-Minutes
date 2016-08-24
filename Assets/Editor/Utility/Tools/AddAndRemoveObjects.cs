using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using System.Collections.Generic;

[InitializeOnLoad]
public class AddAndRemoveObjects : Editor {

    static GUIStyle style = new GUIStyle();
    static Vector2 scrollPosition;
    static int blockCount;
    static Transform m_LevelParent;
    public static Transform LevelParent {
        get {
            if(m_LevelParent == null) {
                GameObject go = GameObject.Find("Level");

                if(go != null) m_LevelParent = go.transform;
                else {
                    go = new GameObject("Level");
                    m_LevelParent = go.transform;
                }
            }
            return m_LevelParent;
        }
    }

    public static int SelectedBlock {
        get { return EditorPrefs.GetInt("SelectedEditorBlock", 0); }
        set { EditorPrefs.SetInt("SelectedEditorBlock", value); }
    }

    static LevelBlocks m_LevelBlocks;

    static AddAndRemoveObjects() {
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
        SceneView.onSceneGUIDelegate += OnSceneGUI;

        m_LevelBlocks = AssetDatabase.LoadAssetAtPath<LevelBlocks>("Assets/Prefab/Level Blocks/LevelBlocks.asset");
    }

    void OnDestory() {
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
    }

    static void OnSceneGUI(SceneView sceneView) {
        if(m_LevelBlocks == null) {
            Debug.Log("m_LevelBlocks is null");
            return;
        }

        DrawCustomBlockButtons(sceneView);
        HandleLevelEditorPlacement();
    }

    static void DrawCustomBlockButtons(SceneView sceneView) {
        Handles.BeginGUI();
        GUI.Box(new Rect(0, 0, 110, sceneView.position.height - 35), GUIContent.none, EditorStyles.textArea);

        scrollPosition = GUI.BeginScrollView(new Rect(0, 0, 125, sceneView.position.height - 35),
         scrollPosition, new Rect(0, 0, 110, 117 * ( blockCount + 1 )), false, true);

        for(blockCount = 0; blockCount < m_LevelBlocks.Blocks.Count; ++blockCount) {
            DrawCustomBlockButtons(blockCount, sceneView.position);
        }

        Handles.EndGUI();
        GUI.EndScrollView();
    }

    private static void DrawCustomBlockButtons(int i, Rect position) {
        bool isActive = false;

        if(ToolsMenu.SelectedTool == 2 && i == SelectedBlock) isActive = true;

        Texture2D previewImage = AssetPreview.GetAssetPreview(m_LevelBlocks.Blocks[i].Prefab);
        GUIContent buttonContent = new GUIContent(previewImage);

        style.normal.textColor = Color.black;
        GUI.Label(new Rect(5, i * 128 + 5, 100, 20), m_LevelBlocks.Blocks[i].Name, style);
        bool isToggleDown = GUI.Toggle(new Rect(5, i * 128 + 25, 100, 100), isActive, buttonContent, GUI.skin.button);

        if(isToggleDown == true && isActive == false) {
            SelectedBlock = i;
            ToolsMenu.SelectedTool = 2;
        }
    }

    static void HandleLevelEditorPlacement() {
        if(ToolsMenu.SelectedTool == 0) return;

        int controlId = GUIUtility.GetControlID(FocusType.Passive);
        List<Vector2> added = new List<Vector2>();

        if(( Event.current.type == EventType.mouseDown /*|| Event.current.type == EventType.mouseDrag */)
            && Event.current.button == 0
            && Event.current.alt == false
            && Event.current.shift == false
            && Event.current.control == false) {


            if(LevelEditorHandle.IsMouseInValidArea == true) {
                if(ToolsMenu.SelectedTool == 1) { RemoveBlock(LevelEditorHandle.currentHandlePosition); }
                if(ToolsMenu.SelectedTool == 2) { AddBlock(LevelEditorHandle.currentHandlePosition, m_LevelBlocks.Blocks[SelectedBlock].Prefab, false); }
                if(ToolsMenu.SelectedTool == 3) { AddBlock(LevelEditorHandle.currentHandlePosition, m_LevelBlocks.Blocks[SelectedBlock].Prefab, true); }
            }
        }

        if(Event.current.type == EventType.keyDown
        && Event.current.keyCode == KeyCode.Escape) {
            ToolsMenu.SelectedTool = 0;
        }

        HandleUtility.AddDefaultControl(controlId);
    }

    public static void AddBlock(Vector3 position, GameObject prefab, bool flip) {
        if(prefab == null) return;

        GameObject newCube = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        if(LevelParent.name == "Level") newCube.transform.parent = LevelParent;
        newCube.transform.position = position + prefab.transform.position;

        Undo.RegisterCreatedObjectUndo(newCube, "Add " + prefab.name);

        UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
    }

    public static void RemoveBlock(Vector3 position) {
        for(int i = 0; i < LevelParent.childCount; ++i) {
            float distanceToBlock = Vector3.Distance(LevelParent.GetChild(i).transform.position, position);
            if(distanceToBlock < 0.1f) {
                //Use Undo.DestroyObjectImmediate to destroy the object and create a proper Undo/Redo step for it
                Undo.DestroyObjectImmediate(LevelParent.GetChild(i).gameObject);

                //Mark the scene as dirty so it is being saved the next time the user saves
                UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
                return;
            }
        }
    }
}
