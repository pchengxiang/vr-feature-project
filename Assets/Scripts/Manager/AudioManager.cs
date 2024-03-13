using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool CheckExistMicrophone()
    {
        if (Microphone.devices.Length == 0)
        {
            Debug.Log("No Microphone");
            return false;
        }
        return true;
    }

    void printMicrophoneInfo()
    {
        
        foreach (string _DeviceName in Microphone.devices)
        {
            Debug.Log("Microphone Name: " +_DeviceName);
        }
    }
}
