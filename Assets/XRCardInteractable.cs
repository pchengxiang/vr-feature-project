using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRCardInteractable : XRGrabInteractable
{
    public CardEntity drivedCard;
    CardEffectHandler handler;
    // Start is called before the first frame update
    void Start()
    {
        GetCard(drivedCard);
    }

    public void GetCard(CardEntity card)
    {
        drivedCard = card;

        activated.AddListener((args) =>
        {
            drivedCard.EffectActivate();
        });

    }

}
