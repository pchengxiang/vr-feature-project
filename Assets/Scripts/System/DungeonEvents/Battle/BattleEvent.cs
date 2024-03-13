using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEvent : DungeonEvent
{
    public static BattleEvent instance = new BattleEvent();
    public PhaseAction action;
    Transform position;
    public Dictionary<string, long> objects;
    public int turn;
    public void Init(Transform room, Dictionary<string, long> objects)
    {
        position = room;
        turn = 0;
        this.objects = objects;
    }
    public void Activate()
    {       
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
        action.Init(position);
    }
    public void On_Update()
    {
        if(action != null) action.On_Update();
    }

    public void EventEnd()
    {
        action = null;
        DungeonEventManager.instance.OnEventFinish.Invoke();
    }
}
public enum BattlePhase
{
    Init,
    PlayerTurn,
    EnemyTurn,
    End
}
