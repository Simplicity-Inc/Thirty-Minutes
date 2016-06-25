using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(LayerMapController))]
public class LayerMapControllerInspec : Editor {

    public override void OnInspectorGUI() {
        LayerMapController inspec = (LayerMapController)target;

        inspec.isPlayer = EditorGUILayout.Toggle("Is Player", inspec.isPlayer);
        EditorGUI.BeginDisabledGroup(inspec.isPlayer == false);
        inspec.host = EditorGUILayout.ObjectField("Host", inspec.host, typeof(Player), true) as Player;
        EditorGUI.EndDisabledGroup();

        inspec.currentLayer = EditorGUILayout.IntField("Current Layer", inspec.currentLayer);
    }
}
