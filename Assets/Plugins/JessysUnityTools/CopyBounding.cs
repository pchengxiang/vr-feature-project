using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
public class CopyBoundingWindow : EditorWindow
{
    private static SkinnedMeshRenderer SourceMeshRendererContext;

    public SkinnedMeshRenderer SourceMeshRenderer;
    public GameObject RootGameObject;
    public bool CopyToAllChildren;
    
    private bool done = false;
    private MessageType result = MessageType.None;
    private string resultTxt = "";

    [MenuItem("Tools/JessysUnityTools/CopyBounding")]
    public static void ShowWindow()
    {
        GetWindow<CopyBoundingWindow>(false, "Copy Bounding", true);
    }

    // Add a menu item called "Double Mass" to a Rigidbody's context menu.
   
    void OnGUI()
    {

        SourceMeshRenderer = SourceMeshRendererContext = (SkinnedMeshRenderer) EditorGUILayout.ObjectField("Source", SourceMeshRendererContext, typeof(SkinnedMeshRenderer), true);
        RootGameObject = (GameObject) EditorGUILayout.ObjectField("Root", RootGameObject, typeof(GameObject), true);
        CopyToAllChildren = (bool)EditorGUILayout.Toggle("Copy to all Children?", true);


        if (GUILayout.Button("Copy Boundings from Source to all Objects in Root"))
        {
            if (!SourceMeshRenderer)
            {
                resultTxt = "FAILED: No Source Skinned Mesh Renderer set.";
                result = MessageType.Error;
            }
            else if (!RootGameObject)
            {
                resultTxt = "FAILED: No Root GameObject set.";
                result = MessageType.Error;
            }
            else
            {
                ApplyBounding(SourceMeshRenderer, RootGameObject, CopyToAllChildren);

                resultTxt = "SUCCESS: Copy successfully finished.";
                result = MessageType.Info;
            }

            done = true;
        }

        if (done)
        {
            EditorGUILayout.HelpBox(resultTxt, result);
        }
    }
    
    static bool ApplyBounding(SkinnedMeshRenderer source, GameObject root, bool toChildren)
    {
        Bounds _sourceBounds = source.localBounds;

        SkinnedMeshRenderer[] meshRenderers = root.transform.GetComponentsInChildren<SkinnedMeshRenderer>(true);
        SkinnedMeshRenderer localMeshRenderer = root.transform.GetComponent<SkinnedMeshRenderer>();

        if (localMeshRenderer)
        {
            localMeshRenderer.localBounds = _sourceBounds;
        }

        if (toChildren)
        {
            foreach (SkinnedMeshRenderer skinnedMeshRenderer in meshRenderers)
            {
                skinnedMeshRenderer.localBounds = _sourceBounds;
            }
        }

        return true;
    }

    static private bool isMeshRendererSet = false;

    [MenuItem("GameObject/JessysUnityTools/CopyBounding/Copy", false, 0)]
    static void SetBoundingSource(MenuCommand command)
    {
        SourceMeshRendererContext = ((GameObject)command.context).transform.GetComponent<SkinnedMeshRenderer>();

        if (SourceMeshRendererContext)
        {
            isMeshRendererSet = true;
        }
        else
        {
            EditorUtility.DisplayDialog("Error", "Could not copy bounding - no SkinnedMeshRenderer found.", "Ok");
        }

    }

    [MenuItem("GameObject/JessysUnityTools/CopyBounding/Paste To all Children", true, 1)]
    [MenuItem("GameObject/JessysUnityTools/CopyBounding/Paste", true, 1)]
    static bool validatePaste(MenuCommand command)
    {
        return isMeshRendererSet;
    }

    [MenuItem("GameObject/JessysUnityTools/CopyBounding/Paste", false, 2)]
    static void Paste(MenuCommand command)
    {
        GameObject root = (GameObject)command.context;
        ApplyBounding(SourceMeshRendererContext, root, false);
    }
    
    [MenuItem("GameObject/JessysUnityTools/CopyBounding/Paste To all Children", false, 2)]
    static void PasteToTree(MenuCommand command)
    {
        GameObject root = (GameObject)command.context;
        ApplyBounding(SourceMeshRendererContext, root, true);
        EditorUtility.DisplayDialog(
            "Success",
            $"Copy Bounding from {SourceMeshRendererContext.name} to all SkinnedMeshRenderers in tree of {root.name} successfully done.", "Ok");
    }
}

#endif
