using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPlayersData : NetworkBehaviour
{
    readonly public SyncList<string> nameData = new SyncList<string> { "", "", "", "" };
    readonly public SyncList<string> readyStateData = new SyncList<string> { "", "", "", "" };
}
