using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MonsterManager : MonoBehaviour //SingletonScriptableObject<MonsterManager>
{
    public static MonsterManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [SerializeField]
    private List<MonsterEntity> monsters = new List<MonsterEntity>();
    public List<MonsterEntity> Monsters
    {
        get { return monsters; }
    }
    public int Size {
        get { return monsters.Count; }
    }

    private MonsterPresenter[] presenters;

    public void Add(MonsterEntity monster)
    {
        monsters.Add(monster);
    }

    public void Remove(MonsterEntity monster)
    {
        Debug.Log("A monster vanish.");
        monsters.Remove(monster);
        Destroy(monster.gameObject,0.1f);
    }
    public void Clear()
    {
        foreach (MonsterEntity monster in monsters) 
            if(monster!=null)
                Destroy(monster.gameObject);
        monsters.Clear();
    }

    public MonsterEntity Find(string name) {
        return monsters.Find((x)=>x.name == name);
    }
    
   
}
