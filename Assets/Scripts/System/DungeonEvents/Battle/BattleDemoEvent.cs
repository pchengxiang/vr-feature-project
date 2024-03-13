using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BattleDemoEvent : MonoBehaviour
{
    public static BattleDemoEvent instance;
    public int turn;
    float DefaultTimeScale;
    bool BulletTime = false; 
    BattlePhase current_phase;
    public PhaseAction action;
    Transform position;
    public UnityAction AttackEnd;
    public UnityAction PlayerEndTurn;
    public UnityAction BulletTimeStart;
    public UnityAction BulletTimeOver;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        AttackEnd = () => { 
            if(current_phase == BattlePhase.EnemyTurn)
            {
                action.SpecialEvent();
            }
        };
        PlayerEndTurn = () =>
        {
            if (current_phase == BattlePhase.PlayerTurn)
            {
                Change_Phase(BattlePhase.EnemyTurn);
            }
        };
        DefaultTimeScale = Time.timeScale;
        BulletTimeStart = () => BulletTime = true;
        BulletTimeOver = () => BulletTime = false;
    }

    public void StartEvent(Transform position)
    {
        turn = 0;
        this.position = position;
        Change_Phase(BattlePhase.Init);
        
    }

    public void Change_Phase(BattlePhase phase)
    {
        switch (phase)
        {
            case BattlePhase.Init:
                action = new Battle_Init();
                break;
            case BattlePhase.PlayerTurn:
                action = new Battle_PlayerTurn();
                break;
            case BattlePhase.EnemyTurn:
                action = new Battle_EnemyTurn();
                break;
            case BattlePhase.End:
                action = new Battle_End();
                break;
        }
        current_phase = phase;
        action.Init(position);
    }

    private void Update()
    {
        if (action != null)
        {
            action.On_Update();
        }
        if (BulletTime)
        {
            Time.timeScale = 0.1f;
        }
        else
        {
            Time.timeScale = DefaultTimeScale;
        }
    }

    public void EventEnd()
    {
        action = null;
        StopAllCoroutines();
    }
}
