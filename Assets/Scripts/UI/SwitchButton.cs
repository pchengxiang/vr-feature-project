using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SwitchButton : MonoBehaviour
{
    static bool[] states;
    bool state = false;
    [SerializeField]
    List<SwitchButton> repel;
    [SerializeField]
    UnityEvent<SwitchButton> OnRepel;
    Button button;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            state = !state;

        });
    }
}
