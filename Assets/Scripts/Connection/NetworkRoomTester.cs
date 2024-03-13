using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkRoomTester : NetworkRoomPlayer
{
    public void Awake()
    {
        for(int i=0; i < 10000;i++)
            print("Awake()");
    }
    public override void OnClientEnterRoom()
    {
        base.OnClientEnterRoom();
        for (int i = 0; i < 10000; i++)
            print("OnClientEnterRoom()");
    }

    public override void OnClientExitRoom()
    {
        base.OnClientExitRoom();
        for (int i = 0; i < 10000; i++)
            print("OnClientExitRoom()");
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        for (int i = 0; i < 10000; i++)
            print("OnStartLocalPlayer");
    }


    public override void OnStartClient()
    {
        base.OnStartClient();
        for (int i = 0; i < 10000; i++)
            print("OnStartClient()");
    }
    public override void OnStopLocalPlayer()
    {
        base.OnStopLocalPlayer();
        for (int i = 0; i < 10000; i++)
            print("OnStopLocalPlayer");
    }
}

