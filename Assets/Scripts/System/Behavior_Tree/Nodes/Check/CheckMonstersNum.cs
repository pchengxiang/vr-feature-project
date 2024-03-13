using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckMonstersNum : Node
{
    int value;
    public CheckMonstersNum(int value)
    {
        this.value = value;
    }

    public override NodeState Evaluate()
    {
        if(MonsterManager.instance.Size <= value)
        {
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }
    }
}
