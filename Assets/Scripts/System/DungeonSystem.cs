using Firebase.Extensions;
using Mirror;
using Newtonsoft.Json.Schema;
using QuickType;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.AsyncOperations;

public class DungeonSystem : NetworkBehaviour
{
    [Header("Scene Settings")]
    [SerializeField]
    List<AssetReference> maps;
    //選完地圖後抓取物件與房間
    int cntMapIndex = -1;
    RoomBehaviour currentMap;
    public RoomBehaviour CurrentMap
    {
        get
        {
            return currentMap;
        }
    }

    List<GameObject> rooms=new();

    public List<PlayerEntity> players=new();
    [SerializeField]
    DungeonPatternGenerator generator;
    [SerializeField]
    public Vector2Int Size = new Vector2Int(4,4);
    public NetworkConnectionManager Network
    {
        get { return NetworkConnectionManager.singleton; }
    }

    ResourceProvider provider;
    GameObjectPoolManager poolManager;
    PlatformOperationGenerator operationGenerator;
    public UnityAction<RoomBehaviour, RoomBehaviour> OnRoomChanged;
    BoardRoom[] board;

    //data
    [SerializeField] 
    LevelInstance level;
    public enum LoadStatus
    {
        Loading,
        Ready,
        Fail
    }

    bool isReady = false;

    void Awake()
    {
        operationGenerator = PlatformOperationGenerator.instance;
        level.SetEnable(false);
        var checkDependencies = CheckDependenciesStatus(() => { Debug.Log("檢查依賴完成"); });
        checkDependencies.Start();

    }

    void Start()
    {
        //checkDependencies.Start();
        if (cntMapIndex == -1)
        {
            var selectedMap = RandomSelectMap();
            selectedMap.Completed += (handle) =>
            {
                currentMap = handle.Result.GetComponent<RoomBehaviour>();
                SetupMapStatus();
            };
        }
        
        //await selectedMap.Task;

        //provider.PrintDebugInformation();
        //var card = provider.GetItemByName<Card>("Slash");
        
    }

    private void Update()
    {
        if(isReady)
        {
            level.StartLevel();
            isReady = false;    
        }
    }

    #region Initialize Map And Level
    AsyncOperationHandle<GameObject> RandomSelectMap()
    {
        cntMapIndex = Random.Range(0, maps.Count);
        var task = Addressables.InstantiateAsync(maps[cntMapIndex]);
        return task;
    }

    // 已廢棄
    public void SetupMapStatus()
    {
        //generator.size_x = Size.x;
        //generator.size_y = Size.y;
        //foreach (var room in rooms)
        //{
        //    generator.rooms.Add(room);
        //}
        //generator.CreateSimpleDungeonPattern();
        //generator.ApplyDungeonPattern();
        //generator.PrintDungeonPattern();
        //board = generator.GetBoard();
    }


    #endregion
    /// <summary>
    /// 檢查資源提供者與物件池的狀態
    /// </summary>
    public Task CheckDependenciesStatus(UnityAction completeAction)
    {
        int dependencyCount = 0;
        var obj = gameObject;
        provider = GetComponent<ResourceProvider>();
        if (provider == null)
        {
            obj.AddComponent<FirebaseDataBaseHelper>();
            provider = obj.AddComponent<ResourceProvider>();
        }
        poolManager = GetComponent<GameObjectPoolManager>();
        if (poolManager == null)
            poolManager = obj.AddComponent<GameObjectPoolManager>();
        Task checkTask = new Task(() =>
        {
            UnityAction loaded = () =>
            {
                dependencyCount += 1;
                if (dependencyCount == 2)
                {
                    isReady = true;
                    completeAction();
                }
            };

            if (!poolManager.IsReady)
                poolManager.OnGameObjectPoolsLoaded += loaded;
            else
                dependencyCount += 1;

            provider.OnResourcesLoadSuccess += loaded;
            provider.LoadStandardResources();
        });
       
        return checkTask;
    }

    

    void RegisterPlayerEntity(PlayerEntity player)
    {
        players.Add(player);
    }

    void EliminatePlayerEntity(PlayerEntity player)
    {
        players.Remove(player);
    }

    void EliminatePlayerEntities(List<PlayerEntity> players)
    {
        foreach(var player in players)
        {
            EliminatePlayerEntity(player);
        }
    }
    
    void PlayerTeleport(Vector3 position)
    {
        foreach (PlayerEntity player in players)
            player.transform.position = new Vector3(position.x, position.y, position.z + 1);
        Debug.Log("Teleported");
    }
    /// <summary>
    /// 載入所有玩家至地圖的初始房間
    /// </summary>
    public void InitPlayersStatus()
    {
        operationGenerator.Configure();
    }

    public void GetPlayersStatus()
    {

    }
}
