using UnityEngine;
using UnityEditor;
using System.Collections;

//[CustomEditor(typeof(Player))]
public class PlayerInspector : Editor
{
    public override void OnInspectorGUI() {
        Player inspec = (Player)target;

        using (new FixedWidthLabel("Move Speed"))
            inspec.moveSpeed = EditorGUILayout.FloatField(inspec.moveSpeed, GUILayout.MaxWidth(100));

        using (new FixedWidthLabel("Min/Max Jump Height")) {
            EditorGUILayout.BeginVertical();
            inspec.minJumpHeight = EditorGUILayout.FloatField(inspec.minJumpHeight);
            inspec.maxJumpHeight = EditorGUILayout.FloatField(inspec.maxJumpHeight);
            EditorGUILayout.EndVertical();
        }

        using (new FixedWidthLabel("Time to Jump Apex"))
            inspec.timeToJumpApex = EditorGUILayout.FloatField(inspec.timeToJumpApex);

        using (new FixedWidthLabel("Wall Jumping")) {
            EditorGUILayout.BeginVertical();
            using (new FixedWidthLabel("Wall Jumping Climb"))
                inspec.wallJumpClimb = EditorGUILayout.Vector2Field("", inspec.wallJumpClimb);

            using (new FixedWidthLabel("Wall Jumping Off"))
                inspec.wallJumpOff = EditorGUILayout.Vector2Field("", inspec.wallJumpOff);

            using (new FixedWidthLabel("Wall Leap"))
                inspec.wallLeap = EditorGUILayout.Vector2Field("", inspec.wallLeap);

            using (new FixedWidthLabel("Max Wall Slide Speed"))
                inspec.wallSlideSpeedMax = EditorGUILayout.FloatField(inspec.wallSlideSpeedMax);

            using (new FixedWidthLabel("Wall Stick Time"))
                inspec.wallStickTime = EditorGUILayout.FloatField(inspec.wallStickTime);
            EditorGUILayout.EndVertical();
        }
    }
}
