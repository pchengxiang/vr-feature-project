using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTurn : Node
{
    int turn;
    public CheckTurn(int turn)
    {
        this.turn = turn;
    }

    public override NodeState Evaluate()
    {
        //if(BattleDemoEvent.instance.turn >= turn)
        if((int)GetData("Turn") >= turn)
        {
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }
    }
}
