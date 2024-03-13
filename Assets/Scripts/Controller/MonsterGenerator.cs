using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGenerator : MonoBehaviour
{
    public List<GameObject> prefabs;
    public static MonsterGenerator instance;

    private void Awake()
    {
        if (instance == null) instance = this;
    }
    public MonsterEntity Spawn(string name, Monster monsterData, Vector3 position)
    {
        var prefab = prefabs.Find(x => x.name == name);
        if (prefab)
        {
            var monster = Instantiate(prefab, position, prefab.transform.rotation);
            MonsterEntity ui = monster.GetComponent<MonsterEntity>();
            ui.SetMonsterData(monsterData);
            //MonsterManager.instance.Add(ui);
            Debug.Log($"{name} is spawned!");
            return ui;
        }

        Debug.LogErrorFormat($"{name} is not in the prefabs");
        return null;
    }
}
