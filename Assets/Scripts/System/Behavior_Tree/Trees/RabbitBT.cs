using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitBT : Tree
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
                    new EarPunch(transform)
                }),
                new Sequence(new List<Node>
                {
                    new CheckHealthRate(transform, 0.5f),
                    new Healing(transform, 3)
                }),
                new BasicAttack(transform)
            }),
            new EndMove(transform)
        }); 
        root.SetData("CoolDown", 0);
        root.SetData("Turn", 0);
        return root;
    }
}
