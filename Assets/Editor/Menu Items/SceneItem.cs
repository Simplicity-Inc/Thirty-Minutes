using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class SceneItem : Editor {
    [MenuItem("Open Scene/Dev Zone")]
    public static void OpenDevZone() { OpenScene("DevScene"); }

    static void OpenScene(string sceneName) {
        if(EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            EditorSceneManager.OpenScene("Assets/Scenes/" + sceneName + ".unity");
    }
}