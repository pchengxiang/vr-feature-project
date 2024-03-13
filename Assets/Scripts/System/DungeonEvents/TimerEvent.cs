using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerEvent : DungeonEvent
{
    public static TimerEvent instance = new TimerEvent();
    Transform room;
    Dictionary<string, long> target;
    float timer;
    bool timer_start;
    public void Activate()
    {
        foreach(PortalLink portal in room.GetComponent<RoomBehaviour>().doors)
        {
            portal.gameObject.SetActive(false);
        }
        timer_start = true;
    }

    public void EventEnd()
    {
        timer_start = false;
        timer = 0;
        room.GetComponent<RoomBehaviour>().UpdateRoom();
        DungeonEventManager.instance.OnEventFinish.Invoke();
    }

    public void Init(Transform room, Dictionary<string, long> objects)
    {
        this.room = room;
        target = objects;
        timer_start = false;
        timer = 0;
    }

    public void On_Update()
    {
        if (timer_start)
        {
            timer += Time.deltaTime;
            if (timer >= target["Timer"])
            {
                EventEnd();
            }
        }
    }
}
