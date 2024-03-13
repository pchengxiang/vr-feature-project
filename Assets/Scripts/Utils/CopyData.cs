using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

class CopyData:MonoBehaviour
{
    public string data;
    public void Copy(TextMeshProUGUI text)
    {
        text.CopyToClipboard();
    }
}

public static class CopyDataMethod
{
    public static void CopyToClipboard(this string str)
    {
        GUIUtility.systemCopyBuffer = str;
    }
    public static void CopyToClipboard(this TextMeshProUGUI text)
    {
        GUIUtility.systemCopyBuffer = text.text;
    }



}
