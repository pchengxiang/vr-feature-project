using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCoolDown : Node
{
    public CheckCoolDown() { }

    public override NodeState Evaluate()
    {
        int cd = (int)GetData("CoolDown");
        if (cd == 0)
        {
            return NodeState.SUCCESS;
        }
        else
        {
            SetDataInRoot("CoolDown", cd -1 );
            return NodeState.FAILURE;
        }
    }
}
