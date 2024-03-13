using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchMethods
{
    public static List<T> GetComponentsInLayer<T>(GameObject root, int layer)
    {
        var ret = new List<T>();
        foreach (var t in root.transform.GetComponentsInChildren(typeof(T), true))
        {
            if (t.gameObject.layer == layer)
            {
                ret.Add(t.gameObject.GetComponent<T>());
            }
        }
        return ret;
    }

    public static List<T> GetComponents<T>(GameObject root)
    {
        var ret = new List<T>();
        foreach (var t in root.transform.GetComponentsInChildren(typeof(T), true))
        {
            ret.Add(t.gameObject.GetComponent<T>());
        }
        return ret;
    }
    /// <summary>
    /// 記得只能查名字都不一樣的元素，否則會回傳結果不完整
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="root"></param>
    /// <returns></returns>
    public static Dictionary<string,T> GetComponentsByNames<T>(GameObject root)
    {
        var ret = new Dictionary<string,T>();
        foreach (var t in root.transform.GetComponentsInChildren(typeof(T), true))
        {
            ret[t.name] = t.gameObject.GetComponent<T>();
        }
        return ret;
    }

    public static List<T> GetComponentsByName<T>(string name, GameObject root)
    {
        var ret = new List<T>();
        foreach (var t in root.transform.GetComponentsInChildren(typeof(T), true))
        {
            if(t.name == name)
                ret.Add(t.gameObject.GetComponent<T>());
        }
        return ret;
    }
}
