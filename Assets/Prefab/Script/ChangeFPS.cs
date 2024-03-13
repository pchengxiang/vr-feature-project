using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFPS : MonoBehaviour
{
    public int targetFrameRate = 60;

    void Awake() {
        //修改当前的FPS
        Application.targetFrameRate = targetFrameRate;
    }
}
