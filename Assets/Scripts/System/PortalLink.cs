using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalLink : MonoBehaviour
{

    public PortalLink target;
    public bool hasTarget;
    public CellDirection direction;
    private void OnTriggerEnter(Collider other)
    {
        if (target != null)
        {
            //target.target = null;
            //RoomBehaviour behaviour = GetComponentInParent<RoomBehaviour>();
            //behaviour.OnRoomLeave += (num, dir) =>
            //{
            //    DungeonEventManager.instance.OnEventFinish.Invoke();
            //};
            //behaviour.OnRoomLeave.Invoke(behaviour.board_number,direction);
            //target = null;
            //hasTarget = false;
            other.transform.position = target.transform.position;
            target = null;
        }
    }

}
