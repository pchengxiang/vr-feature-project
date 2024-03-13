using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirtualGamePadControl : MonoBehaviour
{
    [Header("Phone Virtual Game Pad Configurations")]

    [SerializeField]
    public GameObject LJoyStick;
    [SerializeField]
    public GameObject RJoyStick;
    [SerializeField]
    public GameObject LHandle;
    [SerializeField]
    public GameObject RHandle;
    [SerializeField]
    public GameObject RushButton;
    [SerializeField]
    public GameObject InteractionButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }
}
