using RobinBird.FirebaseTools.Storage.Addressables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Tests loading scenes and making sure they clean up correctly when being unloaded
/// </summary>
public class SceneNavigator : MonoBehaviour
{
    UnityEvent<SceneInstance> LoadedCompleteSceneEvent = new();
    [SerializeField]
    private List<AssetReference> sceneReferenceSequence = new();

    private int index = 0;

    [SerializeField]
    private SceneInstance sceneInstance;

    [SerializeField]
    bool loadFromFirebaseStorage;

    void Awake()
    {
        
        if (sceneReferenceSequence.Count == 0 || sceneReferenceSequence[0] == null)
            return;
    }

    private void LoadSceneFromFirebase()
    {
        FirebaseAddressablesManager.FirebaseSetupFinished += LoadScene;
    }

    private void LoadScene()
    {
        Addressables.LoadSceneAsync(sceneReferenceSequence[index], LoadSceneMode.Additive).Completed += handle =>
        {
            sceneInstance = handle.Result;
            LoadedCompleteSceneEvent.Invoke(sceneInstance);
            Debug.Log("Scene loaded successfully");
        };
    }

    private void PrintInformation()
    {
        Debug.Log($"Load {sceneReferenceSequence[index].RuntimeKey}");
    }

    private void UnloadScene()
    {
        if (sceneInstance.Scene.IsValid())
        {
            Addressables.UnloadSceneAsync(sceneInstance).Completed += handle =>
            {
                Debug.Log("Scene unloaded successfully");
            };
        }
    }

    private void SetupUnloadButton()
    {
        Debug.Log($"Unload {sceneReferenceSequence[index].RuntimeKey}");
    }

    public void LoadNextScene()
    {
        if (index != sceneReferenceSequence.Count - 1 || sceneReferenceSequence.Count != 0) 
            index += 1;
        else
            index = 0;
        if(loadFromFirebaseStorage)
            LoadSceneFromFirebase();
        else
            LoadScene();
    }

    public void LoadPreScene()
    {
        if (index != 0)
            index -= 1;
        else
            index = sceneReferenceSequence.Count - 1;
        if (loadFromFirebaseStorage)
            LoadSceneFromFirebase();
        else
            LoadScene();
    }

    public void OnLoadedCompleteScene(SceneInstance scene)
    {
            
        if(scene.Scene == SceneManager.GetSceneByName(StringResource.LobbySceneName))
        {

        }
    }


}
