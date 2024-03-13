using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;
using static PlayerManager;

//public class PlayerEntity : NetworkBehaviour
public class PlayerEntity : NetworkBehaviour
{
    float health;
    float time;
    int round = 0;

    public Text Name;
    public Text OriginHP;
    public Text OriginDamage;
    public Text OriginAP;

    public Text NowHP;
    public Text NowDamage;
    public Text NowAP;
    public Text NowRound;

    public Button inDeckBtn;
    public Button handCardsBtn;
    public Button discardsBtn;

    public Text CurrentModeHint;

    public Text HandCardAmountHint;
    public Text DiscardAmountHint;
    public Text InDeckAmountHint;

    public Text RemainTimeHint;

    [SerializeField]
    DuelDiskEntity diskEntity;

    [SerializeField]
    DynamicMoveProvider moveProvider;
    public DynamicMoveProvider MoveProvider
    {
        get
        {
            return moveProvider;
        }
    }
    [SerializeField]
    ActionBasedContinuousTurnProvider turnProvider;
    public ActionBasedContinuousTurnProvider TurnProvider
    {
        get
        {
            return turnProvider;
        }
    }
    public DuelDiskEntity DiskEntity 
    { get { return diskEntity; } }
    private void State_Update(float hp, float time)
    {
        health = hp;
        this.time = time;
    }
    public void Awake()
    {
        health = PlayerPrefs.GetFloat(HPTerm);
        time = PlayerPrefs.GetFloat(APTerm);
        diskEntity.BindPlayer(this);
        instance.RegisterPlayer(this);
    }

    public void ResetUIData()
    {
        Name.text = PlayerPrefs.GetString(DisplayTerm);
        NowHP.text = $"{PlayerPrefs.GetFloat(HPTerm)}";
        NowAP.text = $"{PlayerPrefs.GetFloat(APTerm)}";
        NowDamage.text = $"{PlayerPrefs.GetFloat(DamageTerm)}";
        OriginHP.text = $"{instance.Max_Hp}";
        OriginAP.text = $"{instance.Max_AP}";
        OriginDamage.text = $"{instance.Damage}";
        InDeckAmountHint.text = $"{diskEntity.InDeck.Count} 眎";
        DiscardAmountHint.text = $"{diskEntity.Discards.Count} 眎";
        HandCardAmountHint.text = $"{diskEntity.HandCards.Count} 眎";
    }

    private void Update()
    {
        RemainTimeHint.text = $"逞緇丁: {Math.Round(PlayerPrefs.GetFloat(TimeTerm),3)}s";
        CurrentModeHint.text = $"ヘ玡陪ボ:{diskEntity.GetDeckModeExpress()}";
        NowRound.text = $"ヘ玡近计:{round}";
    }

    public void StartTurn()
    { 
        diskEntity.FillHandCards();
        round++;
        NowRound.text = $"ヘ玡近计:材{round}近";
        ResetUIData();
    }

    public void EndTurn()
    {
        BattleDemoEvent.instance.PlayerEndTurn.Invoke();
    }
}
