using Mirror;
using QuickType;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelInstance
{
    [SerializeField]
    List<RoomBehaviour> rooms;
    [SerializeField]
    List<PlayerEntity> playerEntities;
    [SerializeField]
    List<MonsterEntity> monsters;
    [SerializeField]
    List<PortalLink> portalLinks;
    [SerializeField]
    DungeonEvent dungeonEvent;

    [SerializeField]
    RoomBehaviour currentRoom;

    public void Initialize()
    {
        foreach(var room in rooms)
        {

        }
    }

    public void StartLevel() {
        // SetEnable(true); 
        //currentRoom.StartEvent();
    }

    public void SetEnable(bool enable)
    {
        foreach(RoomBehaviour room in rooms) { room.enabled= enable; }
        foreach(PlayerEntity player in playerEntities) {
            player.gameObject.SetActive(enable); 
            player.DiskEntity.enabled = enable;
        }
        
    }

    
}
