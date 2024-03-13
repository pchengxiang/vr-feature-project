using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface DungeonEvent
{
    public void Init(Transform room,Dictionary<string, long> objects);
    public void On_Update();
    public void Activate();
    public void EventEnd();
}

