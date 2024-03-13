using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.Events;

public class MonsterEntity : MonoBehaviour
{
    Monster monster_info;
    Animator animator;
    public Transform target;
    NavMeshAgentControl agent;
    public Monster MonsterInfo
    {
        get { return monster_info; }
        set { monster_info = value; }
    }
    public GameObject healthbarUI;
    public Slider slider;
    float max_health;
    public float health = 10;
    public Tree behavior;
    public UnityAction Attack;
    public UnityAction Effect;
    public GameObject effect;
    public Transform place;
    bool waitForAttackAnimeFinish = false;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgentControl>();
        //if (target == null) target = GameObject.FindGameObjectWithTag("Player").transform;
        //MonsterManager.instance.Add(this);
        
        if (monster_info == null)
        {
            SetMonsterData(new Monster
            {
                Name = gameObject.name,
                BaseDamage = 1,
                Hp = 10
            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (waitForAttackAnimeFinish && animator.GetCurrentAnimatorStateInfo(0).IsName("AttackOver"))
        {
            waitForAttackAnimeFinish = false;
        }
    }

    public void SetMonsterData(Monster data)
    {
        monster_info = data;
        max_health = data.Hp;
        health = max_health;
        HealthUI_Check();
    }

    void HealthUI_Check()
    {
        float current = monster_info.Hp;
        slider.value = current/max_health;
        if (current <= 0)
        {
            animator.SetTrigger("Die");
            Debug.Log(monster_info.Name + " is defeated!");
            MonsterManager.instance.Remove(this);
        }else if(current>max_health)
        {
            monster_info.Hp = max_health;
        }
    }

    public void TakenDamage(float damage)
    {
        if (healthbarUI != null)
        {
            animator.SetTrigger("Damage");
            MonsterInfo.Hp -= damage;
            Debug.Log(MonsterInfo.Name+"'s hp -"+damage);
            HealthUI_Check();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision != null && collision.gameObject.tag == "bullet")
        {
            TakenDamage(2.0f);   
        }
    }
    public void Heal(float value)
    {
        MonsterInfo.Hp += value;
        Debug.Log(MonsterInfo.Name+"'s hp +"+value);
        HealthUI_Check();
    }

    public void StartAttack()
    {
        Debug.Log(MonsterInfo.Name + "'s attack!");
        Attack = () => behavior.StartAttack();
        Effect = () => {
            Instantiate(effect,place);
            Debug.Log("Bleeding");
            };
        if (target == null) target = FindObjectOfType<PlayerEntity>().transform;
        agent.MoveToEvent(target, Attack);
    }

    public void EndAttack()
    {
        waitForAttackAnimeFinish = true;
    }

    public void SetTargetTag(string tag)
    {
        target = GameObject.FindGameObjectWithTag(tag).transform;
    }
}
