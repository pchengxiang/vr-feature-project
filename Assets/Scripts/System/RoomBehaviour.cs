using QuickType;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoomBehaviour : MonoBehaviour
{
    public GameObject[] walls; // 0 - Up 1 -Down 2 - Right 3- Left
    public PortalLink[] doors;
    [SerializeField]
    Transform[] StartPos;
    public int room_number;
    public int board_number;
    public Room room_style;
    bool event_triggered = false;
    public bool Running
    {
        get { return event_triggered; }
    }
    public UnityAction OnRoomEnter;
    public UnityAction<int,CellDirection> OnRoomLeave;
    public UnityAction StartEvent;
    public UnityAction EndEvent;

    private void Start()
    {
        StartEvent = () =>
        {
            if (!event_triggered)
            {
                room_style = new Room
                {

                };
                DungeonEventManager.instance.OnEventStart.Invoke(transform, room_style);
            }
        };
    }
    public void UpdateRoom()
    {
        for (int i = 0; i < doors.Length; i++)
        {
            doors[i].direction = (CellDirection)i;
            doors[i].gameObject.SetActive(doors[i].hasTarget);
        }           
    }

    public void SetBehaviour(BoardRoom room_info)
    {        
        room_number = room_info.room_number;
        board_number = room_info.board_number;
        for (int i = 0; i < 4; i ++){
            doors[i].hasTarget = room_info.status[i];
        }
        event_triggered = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(board_number);         
    }

    public void CallPlayer(PlayerEntity player)
    {
        var random = Random.Range(0, StartPos.Length);
        player.transform.position = StartPos[random].position;
        player.transform.rotation = StartPos[random].rotation;
        StartEvent.Invoke();
    }

    public void CallPlayers(List<PlayerEntity> players)
    {
        if (players.Count == 0)
        {
            Debug.LogError("Length of list of players are zero.");
            return;
        }
        if (StartPos.Length == 0)
        {
            Debug.LogError("Length of list of startPos are zero.");
            return;
        }
        if (StartPos.Length < players.Count)
        {
            foreach(var player in players) {
                var random = Random.Range(0, StartPos.Length);
                player.transform.SetPositionAndRotation(StartPos[random].position, StartPos[random].rotation);
            }
        }
        else
        {
            for (int i = 0; i < players.Count; i++)
            {
                players[i].transform.SetPositionAndRotation(StartPos[i].position, StartPos[i].rotation);
            }
        }
    }
}
