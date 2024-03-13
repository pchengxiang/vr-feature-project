using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class NavMeshAgentControl : MonoBehaviour
{
    NavMeshAgent agent;
    public bool moving = false;
    Transform target;
    UnityAction action;
    Animator animator;

    void Start()
    {
        agent = GetComponentInChildren<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    
    void Update()
    {
        if (moving)
        {
            animator.SetBool("Move", true);
            agent.SetDestination(target.position);
            if (ReachedDestinationOrGaveUp())
            {
                animator.SetBool("Move", false);
                action.Invoke();
                action = null;
                moving = false;
            }
        }
    }

    public void MoveToEvent(Transform target, UnityAction action)
    {
        print("MoveToEvent active");
        this.target = target;
        this.action = action;
        moving = true;
    }

    bool ReachedDestinationOrGaveUp()
    {
        //print($"{agent.remainingDistance}:{agent.stoppingDistance}");
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance || !agent.hasPath)
            {
               //if (agent.velocity.sqrMagnitude == 0f)
              //  {
                    return true;
              //  }
            }
        }
        return false;
    }
}
