using Newtonsoft.Json.Schema;
using QuickType;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    NativeResourceProvider resourceManager;
    public Transform position;

    // Start is called before the first frame update
    void Awake()
    {       
        OnInit();
    }

    public void OnInit()
    {
        resourceManager = NativeResourceProvider.instance;
    }
    private void Start()
    {
        //ItemGenerator.instance.SpellCasterGenerate("Fireball",2,5,position.position);
        BulletTimeBattle.instance.StartBattle(transform);
    }

    private bool TryGetComponent<T>()
    {
        return GetComponent<T>() != null;
    }
}
