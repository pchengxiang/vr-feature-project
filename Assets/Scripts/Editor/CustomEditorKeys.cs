using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomEditorKeys : Editor
{
    public static string EditorSceneBeginning = "Assets/Scenes/AuthenticationScene.unity";
    public static Scene CurrentScene;
    public static bool BackOriginScene;

    [MenuItem("Custom keys/從源頭執行  #%F5")]
    static void EditorRunKey()
    {
        CurrentScene = EditorSceneManager.GetActiveScene();
        EditorSceneManager.OpenScene(EditorSceneBeginning);
        
        BackOriginScene= true;
        EditorApplication.playModeStateChanged += (mode) =>
        {
            switch (mode)
            {
                case PlayModeStateChange.EnteredEditMode:
                    Debug.Log("回到原先場景");
                    if (BackOriginScene)
                    {
                        EditorSceneManager.OpenScene(CurrentScene.path);
                        BackOriginScene = false;
                        
                    }
                        
                    
                    break;
            }
        };
        EditorApplication.isPlaying = true;
    }

}
