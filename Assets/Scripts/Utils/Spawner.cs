using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

internal class Spawner
{
    [ServerCallback]
    internal static void InitialSpawn(int count, Vector3 position)
    {
        for (int i = 0; i < count; i++)
            SpawnReward(position);
    }

    [ServerCallback]
    internal static void SpawnReward(Vector3 position)
    {
        NetworkServer.Spawn(Object.Instantiate(NetworkRoomManagerExt.singleton.rewardPrefab, position, Quaternion.identity));
    }
}
