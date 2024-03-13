using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(ResourceProvider))]
public class ResourceProviderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var provider = (ResourceProvider)target;
        if (GUILayout.Button("Load Resources"))
        {
            provider.LoadStandardResources();
            provider.OnResourcesLoadSuccess+=()=>
                provider.PrintDebugInformation();
        }
    }
}
