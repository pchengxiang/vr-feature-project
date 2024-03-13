using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NetworkRoomPlayerExt : NetworkRoomPlayer
{
    [Header("User Interface Control")]
    public UIController UI;
    private static readonly List<string> playerSeatObjName = new List<string>{ "PlayerSeat1","PlayerSeat2","PlayerSeat3","PlayerSeat4" };
    PlayerRoomContainer container;
    
    
    [SyncVar(hook=nameof(OnEnterOrLeaveRoom))]
    public bool inRoom;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        SetupUI();
    }

    public override void OnClientEnterRoom()
    {
        base.OnClientEnterRoom();
        OnEnterRoom();
    }

    public override void OnClientExitRoom()
    {
        base.OnClientExitRoom();
        inRoom = false;
    }



    public override void OnStartClient()
    {
        base.OnStartClient();
        print(123456);
        container = FindObjectOfType<PlayerRoomContainer>();
        RoomCenter.Instance.SetPlayerPosition(this);
        container.data.nameData.Callback += SyncName;
        container.data.readyStateData.Callback += SyncReadyState;
        UpdateAllData();
    }

    void SetupUI()
    {
        UnityAction readyAction = () => { CmdChangeReadyState(!readyToBegin); };
        UnityAction leaveAction = () => { OnLeaveRoom(); };
        UnityAction StartAction = () => { OnStartGame(); };
        container.SetButtonsOnClickListener(StartAction, leaveAction,readyAction);
    }

    [Command]
    public void OnEnterRoom()
    {
        inRoom = true;
    }

    [Command]
    void OnLeaveRoom()
    {
        inRoom = false;
        if (isServer)
            NetworkManager.singleton.StopHost();
        else
            NetworkManager.singleton.StopClient();
    }

    /// <summary>
    /// 當玩家加入或離開時勾起此函數
    /// </summary>
    /// <param name="oldState"></param>
    /// <param name="newState"></param>
    public void OnEnterOrLeaveRoom(bool oldState, bool newState)
    {
        if (newState)
            ApplyPlayerInfo();
        else
            RemovePlayerInfo();
    }

    public void ApplyPlayerInfo()
    {
        container.data.nameData[index] = $"Player {index + 1}";
        container.data.readyStateData[index] = "Not Ready";
    }

    public void RemovePlayerInfo()
    {
        container.data.nameData[index] = "";
        container.data.readyStateData[index] = "";
    }

    public override void IndexChanged(int oldIndex, int newIndex)
    {
        base.IndexChanged(oldIndex, newIndex);
    }

    void OnStartGame()
    {
        enabled = false;
        NetworkConnectionManager.singleton.ServerChangeScene(NetworkConnectionManager.singleton.GameplayScene);
    }

    public override void ReadyStateChanged(bool oldReadyState, bool newReadyState)
    {
        base.ReadyStateChanged(oldReadyState, newReadyState);
        if (newReadyState)
            container.data.readyStateData[index] = "Ready";
        else
            container.data.readyStateData[index] = "Not Ready";
    }

    public override void OnGUI()
    {
        if(!inRoom) return;
        if (isServer && NetworkConnectionManager.singleton.allPlayersReady)
            container.StartBtn.gameObject.SetActive(true);
        else
            container.StartBtn.gameObject.SetActive(false);
    }

    #region Sync Data
    public void SyncName(SyncList<string>.Operation op, int index, string oldItem, string newItem)
    {
        switch (op)
        {
            case SyncList<string>.Operation.OP_SET:
                if(isServer)
                    UpdateName(index, newItem);
                break;
            default:
                Debug.LogError("[NetworkRoomPlayerExt]:happened error from SyncName(...).");
                break;
        }
    }

    public void SyncReadyState(SyncList<string>.Operation op, int index, string oldItem, string newItem)
    {
        switch (op)
        {
            case SyncList<string>.Operation.OP_SET:
                if(isServer)
                    UpdateState(index, newItem);
                break;
            default:
                Debug.LogError("[NetworkRoomPlayerExt]:happened error from SyncReadyState(...).");
                break;
        }
    }
    #endregion
    #region Update Book Information

    [ClientRpc]
    public void UpdateName(int index,string newText)
    {
        container.names[index].text = newText;
    }
    [ClientRpc]
    public void UpdateState(int index, string newText)
    {
        container.readyStates[index].text = newText;
    }

    public void UpdateAllData()
    {
        for (int i = 0; i < container.maxPlayerAmount; i++)
        {
            container.names[i].text = container.data.nameData[i];
            container.readyStates[i].text = container.data.readyStateData[i];
        }
    }
    #endregion
}
