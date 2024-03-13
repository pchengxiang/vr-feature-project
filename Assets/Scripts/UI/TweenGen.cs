using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class TweenGen : MonoBehaviour
{
    [SerializeField]
    EasyTween tween;
    [SerializeField]
    List<RectTransform> targets;
    [SerializeField]
    Transform parent;
    Dictionary<RectTransform, EasyTween> target_dict = new();
    // Start is called before the first frame update
    void Start()
    {
        foreach(var t in targets)
        {
            Generate(t);
        }
    }

    public void Generate(RectTransform obj)
    {
        tween.rectTransform = obj;
        var newTween = Instantiate(tween,parent);
        target_dict[obj] = newTween;
    }
    public void Use(RectTransform obj)
    {
        target_dict[obj].OpenCloseObjectAnimation();
    }

}
