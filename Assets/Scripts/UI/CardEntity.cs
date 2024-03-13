using QuickType;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CardEntity: MonoBehaviour
{
    public Text Title;    
    public Text Ability;
    public Text Cost;
    public Text Description;
    public Text Rarity;
    //利用預製體，未用
    public GameObject Graph;
    [SerializeField]
    Card cardInfo;
    public Card CardInfo
    {
        get { return cardInfo; }
    }
    bool allowActivate;
    public bool AllowActivate
    {
        get; set;
    }

    DuelDiskEntity deck;
    public DuelDiskEntity Deck
    {
        get
        {
            return deck; 
        }
        set
        {
            deck = value;
        }
    }

    public void Set(Card info)
    {
        cardInfo = info;
        Title.text = info.Name;
        Cost.text = info.CostTime.ToString();
        Description.text = info.Description;
        Rarity.text = "普通";

        foreach(Command cmd in cardInfo.Commands)
        {
            switch(cmd.Item)
            {
                case "Attack":
                    if (cmd.Utility.Integer != null && Ability.text == "")
                        Ability.text = cmd.Utility.Integer.ToString();
                    else if(Ability.text != "")
                    {
                        Ability.text += $",{cmd.Utility.Integer}";
                    }
                    else
                        Ability.text = "0";
                    break;
            }
        }
    }

    public void EffectActivate()
    {
        //cardInfo = ResourceProvider.Singleton.GetItemByName<Card>("Rapier I");
        deck.OnCardActivated(this);
        
    }
    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
    }
}
