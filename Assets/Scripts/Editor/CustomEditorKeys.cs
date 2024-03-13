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

    [MenuItem("Custom keys/�q���Y����  #%F5")]
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
                    Debug.Log("�^��������");
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
