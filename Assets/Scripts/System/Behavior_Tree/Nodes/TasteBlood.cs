using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TasteBlood : Node
{
    Transform transform;
    public TasteBlood(Transform transform)
    {
        this.transform = transform;
    }

    public override NodeState Evaluate()
    {
        float blood = (float)GetData("Blood");
        if (blood == 0) return NodeState.FAILURE;
        MonsterEntity ui = transform.GetComponent<MonsterEntity>();
        transform.GetComponentInChildren<Animator>().SetTrigger("Attack");
        Debug.Log(ui.MonsterInfo.Name +" tastes the stored blood");
        float remain_blood = blood - (ui.health - ui.MonsterInfo.Hp);
        if (remain_blood <= 0)
        {
            ui.Heal(blood);
        }
        else
        {
            ui.Heal(ui.health - ui.MonsterInfo.Hp);
            ui.MonsterInfo.BaseDamage += remain_blood;
            Debug.Log(ui.MonsterInfo.Name + " uses the remain blood to power up!");
            Debug.Log(ui.MonsterInfo.Name + "'s BaseDamage + " + remain_blood);
        }
        SetDataInRoot("Blood", 0f);
        return NodeState.SUCCESS;
    }
}
