using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragoBT : Tree
{
    protected override Node SetUpTree()
    {
        Node root = new Sequence(new List<Node>
        {
            new IsPlayerAlive(),
            new BasicAttack(transform),
            new EndMove(transform)
        });
        root.SetData("Turn", 0);
        return root;
    }
}
