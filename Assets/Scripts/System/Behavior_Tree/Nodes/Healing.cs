using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healing : Node
{
    Transform transform;
    float heal_value;
    public Healing(Transform transform,float value)
    {
        this.transform = transform;
        heal_value = value;
    }

    public override NodeState Evaluate()
    {
        Debug.Log("Healing!");
        transform.GetComponentInChildren<Animator>().SetTrigger("Attack");
        transform.GetComponent<MonsterEntity>().Heal(heal_value);
        return NodeState.SUCCESS;
    }
}
