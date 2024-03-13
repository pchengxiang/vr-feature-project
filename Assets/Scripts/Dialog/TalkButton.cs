using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkButton : MonoBehaviour
{
    public GameObject Button;
    public GameObject talkUI;
    
    private void OnTriggerEnter(Collider other) 
    {
        Button.SetActive(true);    
    }

    private void OnTriggerExit(Collider other) 
    {
        Button.SetActive(false);    
    }

    private void Update()
    {
        //這邊判斷是用鍵盤輸入，可以改成滑鼠
        if(Button.activeSelf && Input.GetKeyDown(KeyCode.Mouse0))
        {
            talkUI.SetActive(true);
        }
    }
}
