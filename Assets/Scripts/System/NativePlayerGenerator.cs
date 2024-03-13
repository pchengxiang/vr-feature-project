using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using static PlatformOperationGenerator;

public class NativePlayerGenerator : MonoBehaviour
{
    [SerializeField]
    UnityEvent Generated;
    [SerializeField]
    GameObject player;
    [SerializeField]
    GameObject XRPlayer;

    GameObject currentPlayer;

    [SerializeField]
    Transform standBy;
    void Start()
    {
        var mode = PlatformOperationGenerator.instance.Mode;
        if (mode == OperationMode.VR)
            currentPlayer = Instantiate(XRPlayer);
        else
            currentPlayer = Instantiate(player);
        Debug.Log(mode.ToString());
        currentPlayer.transform.SetPositionAndRotation(standBy.position, standBy.rotation);
        Generated.Invoke();
    }
}
