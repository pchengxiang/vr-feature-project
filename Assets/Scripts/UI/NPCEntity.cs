using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPCEntity : MonoBehaviour
{
    float damage = 1000000000f;
    Animator animator;
    NavMeshAgentControl agent;
    public Transform target;
    public GameObject TalkUI;
    UnityAction Chat;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgentControl>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        Chat = () =>
        {
            StartTalking();
            TalkUI.SetActive(true);
            TalkUI.GetComponent<DialogSystem>().stopEvent.AddListener((s, i) => StopTalking());
        };
    }

    private void Start()
    {
        agent.MoveToEvent(target, Chat);
    }
    public void StartTalking()
    {
        animator.SetBool("Talk",true);
    }

    public void StopTalking()
    {
        animator.SetBool("Talk", false);
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
        PlayerManager.instance.LoseHp(damage);
    }
}
