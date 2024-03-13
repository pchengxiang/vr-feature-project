using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsPlayerAlive : Node
{
    public IsPlayerAlive() { }

    public override NodeState Evaluate()
    {
        if(PlayerPrefs.GetFloat("Hp") > 0)
        {
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }
    }
}
