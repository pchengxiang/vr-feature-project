using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndMove : Node
{
    Transform transform;
    public EndMove(Transform transform)
    {
        this.transform = transform;
    }

    public override NodeState Evaluate()
    {
        transform.GetComponent<MonsterEntity>().EndAttack();
        return NodeState.SUCCESS;
    }
}
