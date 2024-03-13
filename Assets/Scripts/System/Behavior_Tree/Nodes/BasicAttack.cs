using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : Node
{
    Transform transform;
    public BasicAttack(Transform transform)
    {
        this.transform = transform;
    }

    public override NodeState Evaluate()
    {
        float damage = transform.GetComponent<MonsterEntity>().MonsterInfo.BaseDamage;
        transform.GetComponentInChildren<Animator>().SetTrigger("Attack");
        SetDataInRoot("Damage", damage);
        SetDataInRoot("Turn", (int)GetData("Turn") + 1);
        return NodeState.SUCCESS;
    }
}
