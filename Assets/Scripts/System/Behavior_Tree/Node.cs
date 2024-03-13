using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    protected NodeState state;
    public Node parent;
    protected List<Node> children = new List<Node>();
    Dictionary<string, object> dataDic = new Dictionary<string, object>();
    public Node()
    {
        parent = null;
    }

    public Node(List<Node> children)
    {
        foreach(Node child in children)
        {
            Attach(child);
        }
    }

    void Attach(Node node)
    {
        node.parent = this;
        children.Add(node);
    }

    public virtual NodeState Evaluate() => NodeState.FAILURE;

    public void SetData(string key, object value)
    {
        if (dataDic.ContainsKey(key))
        {
            dataDic[key] = value;
        }
        else
        {
            dataDic.Add(key, value);
        }
    }

    public void SetDataInRoot(string key, object value)
    {
        Node node = parent;
        if(node == null)
        {
            SetData(key, value);
        }
        else
        {
            node.SetDataInRoot(key, value);
        }
    }

    public object GetData(string key)
    {
        object value;
        if (dataDic.TryGetValue(key, out value)) return value;
        Node node = parent;
        while(node != null)
        {
            value = node.GetData(key);
            if (value != null) return value;
            node = node.parent;
        }
        return null;
    }

    public bool ClearData(string key)
    {
        if (dataDic.ContainsKey(key))
        {
            dataDic.Remove(key);
            return true;
        }
        Node node = parent;
        while (node != null)
        {
            bool cleared = node.ClearData(key);
            if (cleared) return true;
            node = node.parent;
        }
        return false;
    }
}

public enum NodeState
{
    RUNNING,
    SUCCESS,
    FAILURE
}
