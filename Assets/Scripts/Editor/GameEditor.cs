using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
[CustomEditor(typeof(GameManager))]
public class GameEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var game = (GameManager)target;
        if (GUILayout.Button("Initialize"))
        { 
            
            game.OnInit();
            

        }
        if(GUILayout.Button("Print Loading Status"))
        {
            foreach(var item in NativeResourceProvider.instance.Loader.ModuleList)
                Debug.Log(item);
        }
        if(GUILayout.Button("Clear"))
        {
            MonsterManager.instance.Clear();
            CardManager.instance.Clear();
        }

    }
}
#endif
