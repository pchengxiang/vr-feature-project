using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRChecker
{
    public static bool checkXRDeviceAvaliable()
    {
        var inputDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevices(inputDevices);

        return inputDevices.Count > 0;

    }
}
