using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(LevelBlock))]
public class LevelBlockEditor : Editor {

    LevelBlock inspec;
    int selLayer = 0;

    public void OnEnable() {
        inspec = (LevelBlock)target;
    }

    public override void OnInspectorGUI() {
        DropAreaGUI();
        DrawDefaultInspector();
    }

    public void DropAreaGUI() {
        Event evt = Event.current;
        Rect drop_Area = GUILayoutUtility.GetRect(0.0f, 20.0f, GUILayout.ExpandWidth(true));
        GUI.Box(drop_Area, "New Tile");
        selLayer = EditorGUILayout.LayerField("Layer: ", selLayer);

        switch(evt.type) {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                if(!drop_Area.Contains(evt.mousePosition)) return;

                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                if(evt.type == EventType.DragPerform) {
                    DragAndDrop.AcceptDrag();

                    foreach(Sprite dragObj in DragAndDrop.objectReferences) {
                        CreatePrefab(dragObj);

                        inspec.Blocks.Add(new LevelBlocksData((GameObject)Resources.Load(dragObj.name)));
                    }
                }

                EditorUtility.SetDirty(inspec);
                EditorSceneManager.MarkAllScenesDirty();
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                break;
        }
    }

    public void CreatePrefab(Sprite dragObj) {
        GameObject gO = new GameObject();
        gO.AddComponent<SpriteRenderer>().sprite = dragObj;
        gO.name = dragObj.name;
        gO.AddComponent<BoxCollider2D>();
        gO.layer = selLayer;

        PrefabUtility.CreatePrefab("Assets/Resources/" + dragObj.name + ".prefab", gO);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Object.DestroyImmediate(gO);

    }
}
