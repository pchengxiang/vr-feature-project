using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatBT : Tree
{
    protected override Node SetUpTree()
    {
        Node root = new Sequence(new List<Node>
        {
            new IsPlayerAlive(),
            new Selector(new List<Node>
            {
                new Sequence(new List<Node>
                {
                    new CheckTurn(3),
                    new CheckCoolDown(),
                    new Vamprism(transform)
                }),
                new Sequence(new List<Node>
                {
                    new CheckHealthRate(transform, 0.5f),
                    new TasteBlood(transform)
                }),
                new BasicAttack(transform)
            }),
            new EndMove(transform)
        });
        root.SetData("CoolDown", 0);
        root.SetData("Blood", 0f);
        root.SetData("Turn", 0);
        return root;
    }
}
