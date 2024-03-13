using QuickType;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CardEffectHandler : MonoBehaviour
{
    public static CardEffectHandler instance;
    public UnityAction<CardEntity,Card> EffectActive;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        EffectActive = (entity, card) =>
        {
            EffectHandle(entity, card);
        };
    }
    public bool EffectHandle(CardEntity entity,Card card)
    {

        if (card.CostTime <= PlayerPrefs.GetFloat(PlayerManager.APTerm))
        {
            float remain = PlayerPrefs.GetFloat(PlayerManager.APTerm) - card.CostTime;
            PlayerPrefs.SetFloat(PlayerManager.APTerm, remain);
            for(int i = 0;i< card.Commands.Length; i++)
            {
                Command command = card.Commands[i];
                switch (card.Commands[i].Item)
                {
                    case "Attack":
                        ItemGenerator.instance.Generate(command.Prefab, command.Utility, command.Durability, entity.transform.position);
                        break;
                    case "Spell":
                        ItemGenerator.instance.SpellCasterGenerate(command.Prefab, command.Utility, command.Durability, entity.transform.position);
                        break;
                    case "Defend":
                        break;
                    case "Relic":
                        break;
                }
            }
            return true;
            //CardManager.instance.Remove(entity);
        }
        else
        {
            Debug.Log("Not Enough AP");
            return false;
        }
    }
}
