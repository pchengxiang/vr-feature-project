using QuickType;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static FirebaseDataBaseHelper;

[RequireComponent(typeof(FirebaseDataBaseHelper))]
public class ResourceProvider:MonoBehaviour
{
    private static ResourceProvider singleton;
    public static ResourceProvider Singleton
    {
        get { return singleton; }
    }
    public UnityAction OnResourceLoadSuccess;
    public UnityAction OnResourcesLoadSuccess;
    public UnityAction OnResourceLoadFailed;
    public UnityAction OnResourcesLoadFailed;
    [SerializeField]
    bool isNative;
    [SerializeField]
    bool printDebugInfo;
    static Dictionary<Type, Dictionary<string, IResource>> resources = new();
    static Dictionary<string,LoadStatus> loadStatus = new();

    public enum LoadStatus
    {
        Loading,
        Ready,
        Fail
    }

    public LoadStatus status;

    private void Awake()
    {
        singleton = this;
    }

    public void LoadStandardResources()
    {
        status = LoadStatus.Loading;
        LoadResource<Card>();
        LoadResource<Monster>();
        LoadResource<Room>();
        LoadResource<Level>();
        LoadResource<Deck>();
        foreach (LoadStatus s in loadStatus.Values)
            if(s == LoadStatus.Fail)
                status = LoadStatus.Fail;
        if (status == LoadStatus.Fail)
            OnResourcesLoadFailed?.DynamicInvoke();
        else
        {
            status = LoadStatus.Ready;
            OnResourcesLoadSuccess?.DynamicInvoke();
        }
    }

    public void LoadResource<T>() where T : IResource
    {
        LoadResource<T>(typeof(T).Name);
    }

    public void LoadResource<T>(string type)
    {
        loadStatus.TryAdd(type, LoadStatus.Loading)
;       Instance?.GetJsonFrom(type, (data) =>
        {
            var content = Unserialize.FromJson<T[]>(data);
            
            var items = new Dictionary<string, IResource>();
            foreach (IResource resource in content)
            {
                if(printDebugInfo)
                    Debug.Log(resource.Name);
                if (!items.TryAdd(resource.Name, resource))
                    loadStatus[type] = LoadStatus.Fail; 
                    //throw new Exception($"Loading error for the {resource.Name} of {typeof(T)}");
            }
            if (!resources.TryAdd(typeof(T), items))
                loadStatus[type] = LoadStatus.Fail;
            //throw new Exception($"Loading error for the {typeof(T)}");
            if (loadStatus[type] != LoadStatus.Fail)
            {
                Singleton.OnResourceLoadSuccess?.Invoke();
                loadStatus[type] = LoadStatus.Ready;
            }
            else
                Singleton.OnResourceLoadFailed?.Invoke();
        });
    }
    public void PrintDebugInformation()
    {
        foreach(var items in GetItems<Card>())
        {
            Debug.Log(items.Value.Name);
        }
    }

    public Dictionary<string,IResource> GetItems<T>() where T: IResource
    {
        if (resources.ContainsKey(typeof(T)))
            return resources[typeof(T)];
        return default;
    }
    public T GetItemByName<T>(string name) where T : IResource
    {
        if (resources[typeof(T)].ContainsKey(name))
            return (T)resources[typeof(T)][name];
        return default;
    }
}
