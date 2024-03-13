using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceLoader
{

     Configurations configurations;
    string[] moduleList;
    public string[] ModuleList
    {
        get { return moduleList; }
    }
    public partial class Configurations
    {
        [JsonProperty("modules")]
        public Module[] Modules { get; set; }
    }

    public partial class Module
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("include")]
        public string[] Include { get; set; }
    }

    public class Wrapper<T>
    {
        public T[] content;
    }


    public List<T> Load<T>(string path) where T : class
    {
        if (configurations == null)
        {
            Debug.LogError("error: You have to load resources first.");
            throw new Exception();
        }
        if (moduleList == null)
            moduleList = ScanModulesList();

        int index = ExistModule(typeof(T).Name);
        if (index == -1)
        {
            Debug.LogError($"error: There exists not {typeof(T).Name}.");
            throw new Exception();
        }

        var data = FileLoader.LoadModule(path + configurations.Modules[index].Path, configurations.Modules[index].Include);
        List<T> items = new();
        foreach(var item in data.Values)
        {
            var content = Unserialize.FromJson<T[]>(item);
            items.AddRange(content);
        }
            
        return items;
    }

    /// <summary>
    /// 讀取模塊設定
    /// </summary>
    /// <param name="path"></param>
    public bool LoadConfigurations(string path)
    {
        var data = FileLoader.LoadFile(path);
        configurations = Unserialize.FromJson<Configurations>(data);
        return configurations != null;
    }
    /// <summary>
    /// 獲取模塊清單
    /// </summary>
    /// <returns>回傳模塊清單</returns>
    /// <exception cref="Exception">必須先使用讀取設定函數</exception>
    public string[] ScanModulesList()
    {
        if (configurations == null)
        {
            Debug.LogError("error: you have to load resources first.");
            throw new Exception();
        }
        string[] modulesName = new string[configurations.Modules.Length];
        for (int i = 0; i < configurations.Modules.Length; i++)
        {
            modulesName[i] = configurations.Modules[i].Name;
        }
        
        return modulesName;
    }

    public int ExistModule(string moduleName)
    {
        if (configurations == null)
        {
            Debug.LogError("error: you have to load resources first.");
            throw new Exception();
        }
        int index = -1;
        for(int i=0; i < configurations.Modules.Length; i++) 
        {
            if (configurations.Modules[i].Name == moduleName)
            {
                index = i;
                break;
            }
        }
        return index;
    }
}
