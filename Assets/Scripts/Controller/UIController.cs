using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public enum UI
{
    [Description("None")]
    None,
    [Description("Desktop UI")]
    DesktopUI,
    [Description("VR UI")]
    VRUI
}


public class UIController:MonoBehaviour
{
    UnityEvent OpenUIEvent;
    UnityEvent CloseUIEvent;
    [SerializeField]
    UI currentUI;
    public UI CurrentUI
    {
        get { return currentUI; }
        set { 
            ui = GameObject.Find(value.ToString());
            currentUI= value;
            Debug.Log(value.ToString());
        }
    }
    [SerializeField]
    GameObject ui;
    public GameObject UI
    {
        get { return ui; }
        set { ui = value; }
    }

    public bool Use
    {
        get
        {
            if(this == null)
                return false;
            return UI.activeSelf;
        }
        set
        {
            UI.SetActive(value);
        }
    }

    private void Start()
    {
        
    }

    public void Awake()
    {
        if (ui == null || currentUI != global::UI.None)
            ui = GameObject.Find(CurrentUI.ToString());

        OpenUIEvent = new UnityEvent();
        CloseUIEvent = new UnityEvent();
        OpenUIEvent.AddListener(OnOpenUI);
        CloseUIEvent.AddListener(OnCloseUI);
    }

    [Serializable]
    public class MarkedGameObject
    {
        public string name;
        public GameObject gameObject;
    }


    void OnOpenUI()
    {
        UI.SetActive(true);
    }

    void OnCloseUI()
    {
        UI.SetActive(false);
    }

    public void OpenUI()
    {
        OpenUIEvent.Invoke();
    }

    public void CloseUI()
    {
        CloseUIEvent.Invoke();
    }
}
