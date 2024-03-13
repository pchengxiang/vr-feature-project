using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCenter : NetworkBehaviour
{
    Transform startPos;
    static RoomCenter instance;
    public static RoomCenter Instance
    {
        get
        {
            return instance;
        }
    }
    public void Awake()
    {
        instance = this;
    }
    public void SetPlayerPosition(NetworkRoomPlayerExt roomPlayer)
    {
        startPos = NetworkConnectionManager.singleton.GetStartPosition();
        print("Start position set.");
        roomPlayer.transform.position = startPos.position;
        
    }
}
