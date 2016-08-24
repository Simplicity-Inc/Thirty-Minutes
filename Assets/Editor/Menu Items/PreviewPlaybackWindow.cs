using UnityEngine;
using UnityEditor;
using System.Collections;

public class PreviewPlaybackWindow : EditorWindow {

    [MenuItem("Window/Preview Playback Window")]
    static void OpenPreviewPlaybackWindow() {
        EditorWindow.GetWindow<PreviewPlaybackWindow>(false, "Playerback");
    }

    float m_PlaybackModifier;
    float m_LastTime;

    void OnEnable() {
        EditorApplication.update -= OnUpdate;
        EditorApplication.update += OnUpdate;
    }

    void OnDisable() {
        EditorApplication.update -= OnUpdate;
    }

    void OnUpdate() {
        if(m_PlaybackModifier != 0f) {
            PreviewTime.Time += ( Time.realtimeSinceStartup - m_LastTime ) * m_PlaybackModifier;
            Repaint();
            SceneView.RepaintAll();
        }
        m_LastTime = Time.realtimeSinceStartup;
    }

    void OnGUI() {
        float seconds = Mathf.Floor(PreviewTime.Time % 60);
        float minutes = Mathf.Floor(PreviewTime.Time / 60);

        GUILayout.Label("Preview Time: " + minutes + ":" + seconds.ToString("00"));
        GUILayout.Label("Playback Speed: " + m_PlaybackModifier);

        GUILayout.BeginHorizontal();
        {
            if(GUILayout.Button("|<", GUILayout.Height(30))) {
                PreviewTime.Time = 0f;
                SceneView.RepaintAll();
            }

            if(GUILayout.Button("<<", GUILayout.Height(30))) {
                m_PlaybackModifier = -5f;
            }

            if(GUILayout.Button("<", GUILayout.Height(30))) {
                m_PlaybackModifier = -1f;
            }

            if(GUILayout.Button("||", GUILayout.Height(30))) {
                m_PlaybackModifier = 0f;
            }

            if(GUILayout.Button(">", GUILayout.Height(30))) {
                m_PlaybackModifier = 1f;
            }

            if(GUILayout.Button(">>", GUILayout.Height(30))) {
                m_PlaybackModifier = 5f;
            }
        }
        GUILayout.EndHorizontal();
    }
}
