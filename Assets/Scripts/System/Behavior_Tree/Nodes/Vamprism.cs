using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vamprism : SpecialMove
{
    public Vamprism(Transform transform) : base(transform) { }

    public override NodeState Evaluate()
    {
        int time = Random.Range(0, 4);
        float value = transform.GetComponent<MonsterEntity>().MonsterInfo.BaseDamage;
        transform.GetComponentInChildren<Animator>().SetTrigger("Attack");
        Debug.Log(transform.GetComponent<MonsterEntity>().MonsterInfo.Name + " sucks blood for "+time+" times!");
        SetDataInRoot("Damage", value * time);
        SetDataInRoot("Blood", (float)value * time);
        SetDataInRoot("CoolDown", 3);
        return NodeState.SUCCESS;
    }
}
