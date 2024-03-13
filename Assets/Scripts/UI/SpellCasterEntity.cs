using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCasterEntity : ItemEntity
{
    public Spell spell;
    [SerializeField]
    Transform casting_point;
    public float Utility { get; set; }
    public int Durability { get { return durability; } set { durability = value; } }

    public void CastSpell()
    {
        if (spell != null)
        {
            var s = Instantiate(spell, casting_point);
            s.Utility = Utility;
            Use();
        }
        else
        {
            Debug.Log("I don't know any magic");
        }
    }
}
