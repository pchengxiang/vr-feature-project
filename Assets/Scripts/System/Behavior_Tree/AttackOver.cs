using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackOver : StateMachineBehaviour
{
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float damage = (float)animator.transform.GetComponentInParent<MonsterEntity>().behavior.GetDataFromRoot("Damage");
        animator.transform.GetComponentInParent<MonsterEntity>().Effect.Invoke();
        PlayerManager.instance.LoseHp(damage);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {        
        animator.transform.GetComponentInParent<MonsterEntity>().StartAttack();
    }
}
