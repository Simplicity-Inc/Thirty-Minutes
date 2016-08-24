﻿using UnityEngine;
using UnityEditor;
using System.Collections;

public class PreviewTime {
    public static float Time {
        get {
            if(Application.isPlaying == true) {
                return UnityEngine.Time.timeSinceLevelLoad;
            }
            return EditorPrefs.GetFloat("PreviewTime", 0f);
        }
        set {
            EditorPrefs.SetFloat("PreviewTime", value);
        }
    }
}