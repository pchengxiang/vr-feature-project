using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VRInputField : InputField
{

    public override void OnSelect(BaseEventData eventData)
    {
        Debug.Log("Onselect");
        TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
        base.OnSelect(eventData);
    }
}
