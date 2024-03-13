using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class GameObjectPoolManager : MonoBehaviour
{
    public UnityAction OnGameObjectPoolsLoaded;
    public UnityAction<GameObjectPool> OnGameObjectPoolLoaded;
    public UnityAction OnGameObjectPoolsLoadFailed;
    public UnityAction<GameObjectPool> OnGameObjectPoolLoadFailed;
    public UnityAction<string, Vector3> GameObjectMoveFromPool;
    public UnityAction GameObjectBackToPool;
    public static GameObjectPoolManager instance;
    List<GameObject> objects = new List<GameObject>();
    List<GameObjectPool> pools = new List<GameObjectPool>();
    public bool IsReady { get { return loadingCoroutine.Count == 0; } }
    //物件數量上限
    [SerializeField] private int elementCount = 8;
    [SerializeField] private List<AssetReference> assetReferenceToInstantiate = new();

    private static Dictionary<object, GameObjectPool> allAvailablePools = new Dictionary<object, GameObjectPool>();
    private Dictionary<object,Coroutine> loadingCoroutine = new();
    public class GameObjectPool
    {
        private int elementCount;
        Stack<GameObject> pool = null;
        GameObject prefab;
        public GameObject Prefab
        {
            get { return prefab; }
        }
        
        private string name;
        bool isReady = false;
        public bool IsReady
        {
            get { return isReady; }
        }

        private Transform ParentTransform = null;

        public GameObject Take(Vector3 position)
        {
            Assert.IsTrue(IsReady, $"Pool {name} is not ready yet");
            if (IsReady == false) return null;
            if (pool.Count > 0)
            {
                var newGameObject = pool.Pop();
                //newGameObject.transform.SetParent(parent, false);
                newGameObject.transform.position = position;
                newGameObject.SetActive(true);
                return newGameObject;
            }
            Debug.LogError($"Pool {name} is free.");
            return null;
        }

        public GameObject Take(Vector3 position, uint assetid)
        {
            return Take(position);
        }

        public void Return(GameObject gameObjectToReturn)
        {
            gameObjectToReturn.SetActive(false);
            gameObjectToReturn.transform.parent = ParentTransform;
            pool.Push(gameObjectToReturn);
        }

        public void Clear()
        {
            pool.Clear();
        }

        public IEnumerator SetupPool(AssetReference assetRef,Transform transform = null, int count = 10,UnityAction<GameObject> onLoaded = default)
        {
            elementCount = count;
            if (transform != null)
                ParentTransform = transform;
            var task = Addressables.LoadAssetAsync<GameObject>(assetRef);
            task.Completed += (handle) =>
            {
                prefab = handle.Result;
                NetworkClient.RegisterPrefab(prefab, Take, Return);
                pool = new Stack<GameObject>(elementCount);
                for (var i = 0; i < elementCount; i++)
                {
                    GameObject product = Instantiate(prefab, ParentTransform);
                    pool.Push(product);
                    product?.SetActive(false);
                }
                isReady = true;
                onLoaded?.Invoke(prefab);
            };
            yield return pool;
        }

        public IEnumerator GetEnumerator()
        {
            return pool.GetEnumerator();
        }


    }

    private void Awake()
    {
        Init();
    }

    void Init()
    {
        if (instance == null) instance = this;
        GameObjectMoveFromPool = (key, position) =>
        {
            GameObjectPool pool = GetPrefabPool(key);
            if (!pools.Contains(pool))
            {
                pools.Add(pool);
            }
            objects.Add(pools[pools.IndexOf(pool)].Take(position));
        };
        GameObjectBackToPool = () =>
        {
            foreach (var pool in pools)                
            {
                foreach (GameObject o in objects)
                {
                    if(o.name == pool.Prefab.name)
                    {
                        pool.Return(o);
                        objects.Remove(o);
                    }
                }
            }
            pools.Clear();
        };
    }

    /// <summary>
    /// 獲取物件池
    /// </summary>
    /// <param name="assetReference">物件的引用</param>
    /// <returns>輸出池子，沒有的話輸出null</returns>
    public static GameObjectPool GetPool(AssetReference assetReference)
    {
        var exists = allAvailablePools
            .TryGetValue(assetReference.RuntimeKey, out GameObjectPool pool);
        if (exists)
        {
            return pool;
        }

        return null;
    }

    public void ReadPools(UnityAction<GameObjectPool> action)
    {
        if (!IsReady)
        {
            OnGameObjectPoolsLoaded += () =>
            {
                foreach (var pool in allAvailablePools.Values)
                {
                    action(pool);
                }
            };
            return;
        }
        foreach(var pool in allAvailablePools.Values)
        {
            action(pool);
        }
    }

    public GameObjectPool GetPrefabPool(string key)
    {
        foreach(var pool in allAvailablePools.Values)
        {
            if(pool.Prefab.name == key)
            {
                return pool;
            }
        }
        return null;
    }

    void OnEnable()
    {
        Assert.IsTrue(elementCount > 0, "Element count must be greater than 0");
        Assert.IsNotNull(assetReferenceToInstantiate, "Prefab to instantiate must be non-null");
        foreach (var assetReference in assetReferenceToInstantiate)
        {
            var gameObjectPool = new GameObjectPool();
            allAvailablePools[assetReference.RuntimeKey] = gameObjectPool;
            loadingCoroutine[assetReference.RuntimeKey] = StartCoroutine(gameObjectPool.SetupPool(assetReference,transform));
        }
    }

    private void Update()
    {
        if (!IsReady)
        {
            bool loadState = true;
            foreach (var assetReference in assetReferenceToInstantiate)
                if (!allAvailablePools[assetReference.RuntimeKey].IsReady)
                {
                    loadState = false;
                    break;
                }
            if (loadState)
            {
                loadingCoroutine.Clear();
                Debug.Log("All game object pools is ready.");
                if(OnGameObjectPoolsLoaded != null)
                    OnGameObjectPoolsLoaded.Invoke();
            }
        }

    }

    void OnDisable()
    {
        foreach (var pool in allAvailablePools)
        {
            foreach (GameObject obj in pool.Value)
            {
                Addressables.ReleaseInstance(obj);
            }
            pool.Value.Clear();
            
        }
    }

}