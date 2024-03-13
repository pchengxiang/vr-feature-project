using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBT : Tree
{
    protected override Node SetUpTree()
    {
        Node root = new Sequence(new List<Node>
        {
            new IsPlayerAlive(),
            new Healing(transform, 1),
            new Selector(new List<Node>
            {
                new Sequence(new List<Node>
                {
                    new CheckTurn(3),
                    new CheckCoolDown(),
                    //new Selector(new List<Node>
                    //{
                    //    new Sequence(new List<Node>{ 
                    //        new CheckMonstersNum(1),
                    //        new Split(transform)
                    //    }),
                    //    new SelfExplode(transform)
                    //})
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
