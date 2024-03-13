using QuickType;
using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[Serializable]
public class Register
{
    [Serializable]
    class NamedReference
    {
        [SerializeField]
        string alias;
        public string Alias
        {
            get { return alias; }
        }
        [SerializeField]
        AssetReference reference;
        public object GetKey()
        {
            return reference.RuntimeKey;
        }
    }
    [SerializeField]
    List<NamedReference> references = new();
    Dictionary<string, NamedReference> keys;

    public object GetRuntimeKey(string key)
    {
        if(keys == null)
        {
            ToDictionary();
        }
        return keys[key];
    }

    private Dictionary<string,NamedReference> ToDictionary()
    {
        foreach(NamedReference reference in references)
        {
            keys[reference.Alias] = reference;
        }
        return keys;
    }

    string MappingEntityAndModel<T>()
    {
        
        switch (typeof(T).Name)
        {
            case "Card":
                return "CardEntity";
            default:
                throw new NotImplementedException();
        }
    }

    
}
