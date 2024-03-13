using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Recorder;
using UnityEngine;
using UnityEngine.Profiling;

public class SpinRecording : MonoBehaviour
{
    RecorderWindow recorderWindow;
    bool recording;
    // Start is called before the first frame update
    void Start()
    {
        RecorderWindow recorderWindow = EditorWindow.GetWindow<RecorderWindow>();
    }
    //拍攝開始與結束
    void Update()
    {
        if (Input.GetKey(KeyCode.I) && !recorderWindow.IsRecording())
        {
            recorderWindow.StartRecording();
        }
        else
        {
            recorderWindow.StopRecording();

        }
    }
}
#endif