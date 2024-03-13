using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tree : MonoBehaviour
{
    Node root = null;
    public void StartAttack()
    {
        if(root == null) root = SetUpTree();
        root.Evaluate();
    }
    protected abstract Node SetUpTree();

    public object GetDataFromRoot(string key)
    {
        return root.GetData(key);
    }
}