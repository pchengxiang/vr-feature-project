using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ItemManager : MonoBehaviour//SingletonScriptableObject<ItemManager>
{
    public static ItemManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    List<ItemEntity> items = new List<ItemEntity>();
    public List<ItemEntity> Items { get { return items; } }
    public int Size { get { return items.Count; } }
    
    public void Add(ItemEntity ui)
    {
        items.Add(ui);
    }

    public void Remove(ItemEntity ui)
    {
        Destroy(ui.gameObject, 0.01f);
        items.Remove(ui);
    }

    public void Clear()
    {
        foreach(ItemEntity ui in items)
        {
            if(ui != null) Destroy(ui.gameObject);
        }
        items.Clear();
    }
}
