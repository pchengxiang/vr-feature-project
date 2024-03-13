using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuickType;

public class ItemGenerator : MonoBehaviour
{
    public List<GameObject> prefabs;
    public List<Spell> spells;
    public static ItemGenerator instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public void Generate(string name,Utility utility,int durability, Vector3 position)
    {
        var prefab = prefabs.Find((x) => x.name == name);
        if (prefab)
        {
            var item = Instantiate(prefab, position, prefab.transform.rotation);
            var weapon = item.GetComponent<WeaponEntity>();
            if(weapon!= null )
            {
                if (utility.Integer.HasValue)
                {
                    weapon.Damage = utility.Integer.Value;
                    weapon.Durability = durability;
                }
                else
                    Debug.LogWarning("Item: " + prefab.name + "cannot generated because its utility isn't valid.");
            }
            var buff = item.GetComponent<BuffEntity>();
            if (buff != null)
            {
                if (utility.Integer.HasValue)
                {
                    weapon.Damage = utility.Integer.Value;
                    weapon.Durability = durability;
                }
                else
                    Debug.LogWarning("Item: " + prefab.name + "cannot generated because its utility isn't valid.");
            }
           
            
            //ItemManager.instance.Add(item.GetComponent<WeaponEntity>());
        }
    }

    public void SpellCasterGenerate(string name, Utility utility, int durability, Vector3 position)
    {
        var prefab = prefabs.Find((x) => x.name == "Staff");
        if (prefab)
        {
            var item = Instantiate(prefab, position, prefab.transform.rotation);
            SpellCasterEntity entity = item.GetComponent<SpellCasterEntity>();
            if (utility.Integer.HasValue)
                entity.Utility = utility.Integer.Value;
            else
                Debug.LogWarning("Item: " + prefab.name + "cannot generated because its utility isn't valid.");
            entity.Durability = durability;
            entity.spell = spells.Find((x) => x.name == name);
            //ItemManager.instance.Add(item.GetComponent<SpellCasterEntity>());
        }
    }

}
