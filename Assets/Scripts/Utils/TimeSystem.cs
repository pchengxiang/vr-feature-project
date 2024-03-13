using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSystem:MonoBehaviour
{
    [SerializeField]
    float initializeSpeed;

    public void Start()
    {
        ChangeProcessTimeScale(initializeSpeed);
    }
    public static void changeFullGameScale(AudioSource[] audio, float scale)
    {
        foreach (AudioSource audioSource in audio) { ChangeAudioTime(audioSource, scale); }
        ChangeProcessTimeScale(scale);
    }
    static void ChangeAudioTime(AudioSource audio, float scale)
    {
        audio.pitch= scale;
    }
    
   static void ChangeProcessTimeScale(float scale)
    {
        Time.timeScale= scale;
    }

}
