using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonGroup : MonoBehaviour
{

    [SerializeField]
    List<Button> buttons;
    List<bool> states;
    public bool[] States
    {
        get; 
        set;
    }
    int currentCheck = -1;
    int preCheck = -1;
    [SerializeField]
    bool single = true;
    int checkNum = 0;

    public enum ButtonGroupState
    {
        UncheckAll,
        Toggle,
        ToggleOther,
        CheckAll
    }

    ButtonGroupState currentState;

    [SerializeField]
    UnityEvent CheckAll;
    [SerializeField]
    UnityEvent UncheckAll;
    [SerializeField]
    UnityEvent<RectTransform> Toggle;
    [SerializeField]
    UnityEvent<RectTransform> ToggleOther;
    [SerializeField]
    UnityEvent<RectTransform> ToggleFirst;

    private void Awake()
    {
        states = new List<bool>(buttons.Count);
    }
    // Start is called before the first frame update
    void Start()
    {
        for(int i=0; i<buttons.Count; i++)
        {
            states[i] = false;
            buttons[i].onClick.AddListener(() =>
            {
                ButtonGroupState newState;
                //States[i] = !States[i];
                newState = ButtonGroupState.Toggle;
                preCheck = currentCheck;
                if (single)
                {
                    if (i == currentCheck)
                    {
                        currentCheck = -1;
                        newState = ButtonGroupState.UncheckAll;
                        ChangeState(newState,null);
                    }
                    else if (currentCheck == -1)
                    {
                        currentCheck = i;
                        ChangeState(newState, buttons[i]);
                    }
                    else if (buttons[i].gameObject.activeSelf)
                    {
                        buttons[currentCheck].onClick.Invoke();
                        newState = ButtonGroupState.ToggleOther;
                        ChangeState(newState, buttons[currentCheck]);
                    }
                }
                Debug.Log(currentCheck);
            }
            );
        }

    }

    void OnClick(int i)
    {
        ButtonGroupState newState;
        //States[i] = !States[i];
        newState = ButtonGroupState.Toggle;
        preCheck = currentCheck;
        Debug.Log(currentCheck);
        if (single)
        {
            if(i == currentCheck)
            {
                currentCheck = -1;
                newState = ButtonGroupState.UncheckAll;
            }
            else if (currentCheck == -1)
            {
                currentCheck = i;
            }
            else if (buttons[i].gameObject.activeSelf)
            {
                buttons[currentCheck].onClick.Invoke();
                newState = ButtonGroupState.ToggleOther;
            }
        }
        else
        {
            if (states[i])
                checkNum++;
            else
                checkNum--;
            if(checkNum == buttons.Count)
                newState = ButtonGroupState.CheckAll;
            //TODO:Continue...
        }

    }

    public void ChangeState(ButtonGroupState newStates,Button button)
    {
        switch(newStates)
        {
            case ButtonGroupState.UncheckAll:
                UncheckAll.Invoke();
                break;
            case ButtonGroupState.CheckAll:
                CheckAll.Invoke();
                break;
            case ButtonGroupState.ToggleOther:
                ToggleOther.Invoke(button.gameObject.GetComponent<RectTransform>());
                break;
            default:
                if (currentState == ButtonGroupState.UncheckAll)
                    ToggleFirst.Invoke(button.gameObject.GetComponent<RectTransform>());
                Toggle.Invoke(button.gameObject.GetComponent<RectTransform>());
                break;
        }
        currentState = newStates;
    }
}
