using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfExplode : SpecialMove
{
    public SelfExplode(Transform transform) : base(transform) { }

    public override NodeState Evaluate()
    {
        Debug.Log("Explosion!");
        float damage = transform.GetComponent<MonsterEntity>().MonsterInfo.Hp - 1;
        transform.GetComponentInChildren<Animator>().SetTrigger("Attack");
        SetDataInRoot("Damage", damage * 3);
        transform.GetComponent<MonsterEntity>().TakenDamage(damage);
        SetDataInRoot("CoolDown", 3);
        return NodeState.SUCCESS;
    }
}
