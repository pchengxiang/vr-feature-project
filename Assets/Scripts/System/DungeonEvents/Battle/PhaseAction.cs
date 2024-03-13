using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using QuickType;

public interface PhaseAction
{
    public void Init(Transform room);
    public void On_Update();

    public void SpecialEvent();
}

public class Battle_Init : PhaseAction
{
    static NativeResourceProvider resourceManager = NativeResourceProvider.instance;
    public void Init(Transform room)
    {
        Debug.Log("Battle Start!");
        Debug.Log(MonsterManager.instance.Size);
        CardManager.instance.Clear();
        MonsterManager.instance.Clear();
        ItemManager.instance.Clear();   
        EnemySpawn(room);
        Debug.Log("生成數量:" + MonsterManager.instance.Size);
        BattleDemoEvent.instance.Change_Phase(BattlePhase.PlayerTurn);
    }

    public void On_Update()
    {

    }

    public void SpecialEvent() { }

    void EnemySpawn(Transform position)
    {
        Vector3 position_center = position.position;
        Vector3 position_left = new Vector3(position_center.x - 3, position_center.y, position_center.z);
        Vector3 position_right = new Vector3(position_center.x + 3, position_center.y, position_center.z);
        MonsterGenerator.instance.Spawn("Slime", resourceManager.GetDictItem<Monster>("Slime"), position_center);
        MonsterGenerator.instance.Spawn("Rabbit", resourceManager.GetDictItem<Monster>("Rabbit"), position_left);
        MonsterGenerator.instance.Spawn("Bat", resourceManager.GetDictItem<Monster>("Bat"), position_right);
    }
}

public class Battle_PlayerTurn : PhaseAction
{
    static NativeResourceProvider resourceManager = NativeResourceProvider.instance;
    public void Init(Transform room)
    {
        BattleDemoEvent.instance.turn++;
        Debug.Log("Turn: " + BattleDemoEvent.instance.turn);
        Debug.Log("Player's Turn!");
        PlayerManager.instance.EnterNextRound();
        BattleDemoEvent.instance.BulletTimeStart.Invoke();
        Debug.Log("Hp: " + PlayerPrefs.GetFloat("Hp")+" Time: "+PlayerPrefs.GetFloat("Time"));
        //CardSpawn();
    }

    public void On_Update()
    {
        if(MonsterManager.instance.Size == 0)
        {
            Debug.Log("Victory!");
            BattleDemoEvent.instance.Change_Phase(BattlePhase.End);
        }else if(PlayerPrefs.GetFloat("Hp") <= 0)
        {
            Debug.Log("Lose!");
            BattleDemoEvent.instance.Change_Phase(BattlePhase.End);
        }//else if(!CostCheck())
        //{
            //BattleDemoEvent.instance.Change_Phase(BattlePhase.EnemyTurn);
        //}
    }

    public void SpecialEvent() { }   

    void CardSpawn()
    {
        //for(int i = 0;i < PlayerPrefs.GetInt("Draw_Count"); i++)
            CardGenerator.instance.Spawn("Rapier", resourceManager.GetDictItem<Card>("Rapier"));
    }

    bool CostCheck()
    {
        float time = PlayerPrefs.GetFloat("Ap");
        if (time == 0) return false;
        foreach (CardEntity card in CardManager.instance.Cards)
        {
            //if(card.cardInfo.CostTime <= time)
            //{
                //return true;
            //}
        }
        return false;
    }
}

public class Battle_EnemyTurn : PhaseAction
{
    public UnityAction NextMonsterMove;
    bool attack_finished;
    int index = 0;
    public void Init(Transform room)
    {       
        Debug.Log("Enemy's Turn!");
        CardManager.instance.Clear();
        BattleDemoEvent.instance.BulletTimeOver.Invoke();
        attack_finished = false;
        NextMonsterMove = () =>
        {
            Attack();
        };
        NextMonsterMove.Invoke();
    }

    public void On_Update()
    {      
        if (MonsterManager.instance.Size == 0)
        {
            Debug.Log("Victory!");
            BattleDemoEvent.instance.Change_Phase(BattlePhase.End);
        }else if (PlayerPrefs.GetFloat("Hp") <= 0)
        {
            Debug.Log("Lose!");
            BattleEvent.instance.Change_Phase(BattlePhase.End);
        }else if (attack_finished)
        {
            BattleDemoEvent.instance.Change_Phase(BattlePhase.PlayerTurn);
        }
    }

    public void SpecialEvent()
    {
        NextMonsterMove.Invoke();
    }

    void Attack()
    {
        if(index < MonsterManager.instance.Size)
        {
            MonsterManager.instance.Monsters[index].StartAttack();
            index++;
        }
        else
        {
            attack_finished = true;
        }
        
    }
}

public class Battle_End : PhaseAction
{
    public void Init(Transform room)
    {
        MonsterManager.instance.Clear();
        CardManager.instance.Clear();
        ItemManager.instance.Clear();
        PlayerManager.instance.EnterNextRound();
        BattleDemoEvent.instance.BulletTimeOver.Invoke();
        BattleDemoEvent.instance.EventEnd();
    }
    public void On_Update()
    {
        
    }

    public void SpecialEvent() { }
}