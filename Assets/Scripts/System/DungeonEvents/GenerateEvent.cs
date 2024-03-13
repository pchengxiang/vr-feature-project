using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateEvent : DungeonEvent
{
    public static GenerateEvent instance = new GenerateEvent();
    Transform room;
    Dictionary<string, long> objects;

    public void Activate()
    {
        int i = 0;
        foreach(string key in objects.Keys)
        {
            for(int j = 0; j < objects[key]; j++)
            {
                Vector3 position = new Vector3(room.position.x + i, room.position.y, room.position.z);
                GameObjectPoolManager.instance.GameObjectMoveFromPool.Invoke(key, position);
                i++;
            }         
        }
    }

    public void EventEnd()
    {
        objects.Clear();
        GameObjectPoolManager.instance.GameObjectBackToPool.Invoke();
        DungeonEventManager.instance.OnEventFinish.Invoke();
    }

    public void Init(Transform room, Dictionary<string, long> objects)
    {
        this.room = room;
        this.objects = objects;
    }

    public void On_Update()
    {
        
    }
}
