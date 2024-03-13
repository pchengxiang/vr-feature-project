using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarPunch : SpecialMove
{
    public EarPunch(Transform transform) : base(transform) { }

    public override NodeState Evaluate()
    {
        Debug.Log("Ear Punch!!");
        float damage = transform.GetComponent<MonsterEntity>().MonsterInfo.BaseDamage;
        transform.GetComponentInChildren<Animator>().SetTrigger("Attack");
        SetDataInRoot("Damage", damage * 2);
        SetDataInRoot("CoolDown", 3);
        return NodeState.SUCCESS;
    }
}
