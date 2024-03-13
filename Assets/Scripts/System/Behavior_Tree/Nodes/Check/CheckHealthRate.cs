using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckHealthRate : Node
{
    Transform transform;
    float limit_rate;
    public CheckHealthRate(Transform transform,float rate)
    {
        this.transform = transform;
        limit_rate = rate;
    }

    public override NodeState Evaluate()
    {
        if(transform.GetComponent<MonsterEntity>().slider.value <= limit_rate)
        {
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }
    }
}
