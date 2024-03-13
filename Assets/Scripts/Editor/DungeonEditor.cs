using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
[CustomEditor(typeof(DungeonPatternGenerator))]
public class DungeonEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var dungeon = (DungeonPatternGenerator)target;
        if (GUILayout.Button("Update"))
        {
            dungeon.CreateSimpleDungeonPattern();
            dungeon.PrintDungeonPattern();
            dungeon.ApplyDungeonPattern();
        }
    }
}
#endif