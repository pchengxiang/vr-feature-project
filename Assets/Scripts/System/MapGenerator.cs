using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{
    public int LevelAmount;
    //路線最大與最小數量
    public int minWayAmount;
    public int maxWayAmount;
    //節點物件
    //List<Connection> RoomNodeObjects = new();
    //起點(樹根)
    NTree<RoomNode> rootNode;
    public float width;
    public float height;

    public List<HorizontalLayoutGroup> LevelLayoutGroups;
    public GameObject Board;
    public RectTransform nodeObject;

    class RoomNode
    {
        public RoomNode parent;
        public int level;
        public RoomType type;
    }

    enum RoomType
    {
        Monster,
        Merchant,
        Rest
    }

    public void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {

        DivideIntoLevels();
        GenerateConnection();
    }

    void SplitBoard()
    {
       
    }

    void DivideIntoLevels()
    {
        var types = Enum.GetValues(typeof(RoomType));
        rootNode = new NTree<RoomNode>(
            new RoomNode
            {
                parent = null,
                type = RoomType.Rest,
                level = 1
            }
        );
        var levelNodes = new List<NTree<RoomNode>>
        {
            rootNode
        };
        for (int i = 1; i < LevelAmount; i++)
        {
            var nextLevelNodes = new List<NTree<RoomNode>>();
            for (int j = 0; j < levelNodes.Count; j++)
            {
                var randomWayAmount = Random.Range(minWayAmount, maxWayAmount);
                for (int k = 0; k < randomWayAmount; k++)
                {
                    var randomType = (RoomType)types.GetValue(Random.Range(0, types.Length));

                    var node = new RoomNode
                    {
                        type = randomType,
                        level = i,
                        parent = levelNodes[k].Data
                    };

                    var treeNode = levelNodes[k].AddChild(node);
                    nextLevelNodes.Add(treeNode);
                }
            }
            levelNodes = nextLevelNodes;
        }
    }

    void GenerateConnection()
    {
        int i = 1;
        NTree<RoomNode>.Traverse(rootNode, (node) =>
        {
            var nodeData = node;
            Debug.Log($"{node.type} {node.level}");
            var obj = new GameObject($"RoomNode {i}");
            //var connection = obj.AddComponent<Connection>();
           // RoomNodeObjects.Add(connection);
            
            ++i;
        });
       
    }

    delegate void TreeVisitor<T>(T node);

    class NTree<T>
    {
        private T data;
        public T Data 
        {
            get { return data; } 
        }
        private LinkedList<NTree<T>> children;
        public LinkedList<NTree<T>> Children
        {
            get { return children; }
        }

        public NTree(T data)
        {
            this.data = data;
            children = new LinkedList<NTree<T>>();
        }

        public NTree<T> AddChild(T data)
        {
            var childNode = new NTree<T>(data);
            children.AddFirst(childNode);
            return childNode;
        }

        public NTree<T> GetChild(int i)
        {
            foreach (NTree<T> n in children)
                if (--i == 0)
                    return n;
            return null;
        }

        public static void Traverse(NTree<T> node, TreeVisitor<T> visitor, bool recursive = true)
        {
            visitor(node.data);
            foreach (NTree<T> kid in node.children)
                if (recursive)
                    Traverse(kid, visitor);
                else
                    visitor(kid.Data);
        }
    }


}
