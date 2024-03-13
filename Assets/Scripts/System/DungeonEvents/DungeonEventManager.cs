using QuickType;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DungeonEventManager : MonoBehaviour
{
    public static DungeonEventManager instance;
    public DungeonEvent current_event;
    Transform room;
    Dictionary<string, long> event_objects = new Dictionary<string, long>();
    bool event_continue;
    public UnityEvent<Transform, Room> OnEventStart = new UnityEvent<Transform, Room>(); 
    public UnityEvent OnEventFinish = new UnityEvent();
    private void Awake()
    {
        Init();
    }
    
    void Init()
    {
        if (instance == null) instance = this;
        OnEventStart.AddListener((room, type) =>
        {
            SetEvent(room, type);
            StartEvent();
        });
        OnEventFinish.AddListener(() => {
            if (current_event != null)
                ForceEnd();
            else
                EndEvent();
        });
    }

    private void Update()
    {
        if(current_event != null)
            current_event.On_Update();
    }

    public void StartEvent()
    {
        current_event.Init(room, event_objects);
        event_continue = true;
        current_event.Activate();
    }

    public void SetEvent(Transform room ,Room room_type)
    {
        this.room = room;
        if(room_type.Generate.Length != 0)
            foreach(Generate obj in room_type.Generate)
                event_objects.Add(obj.Name, obj.CardinalAmount);
        switch (room_type.Type)
        {
            case RoomEventType.Battle:
                //current_event = BattleEvent.instance;
                current_event = GenerateEvent.instance;
                if(BulletTimeBattle.instance == null)
                {
                   var battle = gameObject.AddComponent<BulletTimeBattle>();
                    battle.StartBattle(room.transform);
                }
                break;
            case RoomEventType.Shop:
                current_event = GenerateEvent.instance;
                break;
            case RoomEventType.Timer:
                current_event = TimerEvent.instance;
                break;
        }
    }

    public void EndEvent()
    {
        if (event_continue)
        {
            event_continue = false;
            current_event = null;
            event_objects.Clear();
            room.GetComponent<RoomBehaviour>().EndEvent.Invoke();
            StopAllCoroutines();
        }
    }

    public void ForceEnd()
    {
        current_event.EventEnd();
    }
}
