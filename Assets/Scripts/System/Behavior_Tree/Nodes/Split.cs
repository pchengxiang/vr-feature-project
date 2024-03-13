using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Split : SpecialMove
{
    public Split(Transform transform) : base(transform) { }

    public override NodeState Evaluate()
    {
        Debug.Log("Split into two!");
        transform.GetComponentInChildren<Animator>().SetTrigger("Attack");
        Vector3 position_left = new Vector3(transform.position.x - 2, transform.position.y, transform.position.z);
        Vector3 position_right = new Vector3(transform.position.x + 2, transform.position.y, transform.position.z);
        var token1 = MonsterGenerator.instance.Spawn("TokenSlime", NativeResourceProvider.instance.GetDictItem<Monster>("TokenSlime"), position_left);
        var token2 = MonsterGenerator.instance.Spawn("TokenSlime", NativeResourceProvider.instance.GetDictItem<Monster>("TokenSlime"), position_right);
        token1.StartAttack();
        token2.StartAttack();
        SetDataInRoot("CoolDown", 3);
        return NodeState.SUCCESS;
    }
}
