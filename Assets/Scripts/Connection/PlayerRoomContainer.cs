using Mirror;
using QuickType;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerRoomContainer : NetworkBehaviour
{
    public List<Text> names = new();
    public List<Text> readyStates = new();
    CopyData roomKey;

    readonly public int maxPlayerAmount = 4;
    [SerializeField]
    Button startBtn;
    public Button StartBtn
    {
        get { return startBtn; }
    }
    [SerializeField]
    Button leaveBtn;
    public Button LeaveBtn
    {
        get { return leaveBtn; }
    }
    [SerializeField]
    Button readyBtn;
    public Button ReadyBtn
    {
        get { return readyBtn; }
    }
    public TextMeshProUGUI RoomIDLabel;

    public RoomPlayersData data;

    //public RTCFirebaseSignaler Singaler
    //{
    //    get;
    //}
    private void Start()
    {
        data = GetComponent<RoomPlayersData>();
        if(data ==null)
            gameObject.AddComponent<RoomPlayersData>();
        roomKey = GetComponent<CopyData>();
        var id = NetworkConnectionManager.singleton.RoomID;
        SetRoomID(id);
        
    }

    public void SetRoomID(string roomID)
    {
        RoomIDLabel.text = $"RoomID:\n{roomID}";
        roomKey.data = roomID;
    }

    public void SetButtonsOnClickListener(UnityAction StartAct=default,UnityAction LeaveAct = default, UnityAction ReadyAct = default)
    {
        StartBtn.onClick.AddListener(StartAct);
        LeaveBtn.onClick.AddListener(LeaveAct);
        ReadyBtn.onClick.AddListener(ReadyAct);
    }
}
