using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEntity: MonoBehaviour
{

    protected int durability = 1;
    protected PlayerEntity owner;
    public PlayerEntity Owner
    {
        get { return owner; }
    }

    public void OnEnable()
    {
        ItemManager.instance.Add(this);
    }

    protected void Use(int cost = 1)
    {
        durability -= cost;
        if (durability <= 0)
        {
            ItemManager.instance.Remove(this);
        }
    }   
}

public class WeaponEntity : ItemEntity
{
    protected float damage = 2;
   
    public float Damage { get { return damage; } set { damage = value; } }
    public int Durability { get { return durability; } set { durability = value; } }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.tag == TagName.Monster && durability > 0)
        {
            other.GetComponentInParent<MonsterEntity>().TakenDamage(damage);
            Use();
        }
    }
}

public class BuffEntity : ItemEntity
{
    protected float strength;
    public float Strength
    {
        get { return strength; }
    }
    //¼È©w¬°ª±®a
    public PlayerEntity target;

    private void Start()
    {
        
    }
}
