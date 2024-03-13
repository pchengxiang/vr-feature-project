using Mirror;
using Mirror.Discovery;
using QuickType;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using static PageManager;

public class NetworkConnectionManager : NetworkRoomManager
{

    public static new NetworkConnectionManager singleton
    {
        get; private set;
    }

    bool resourceLoadComplete;
    public bool ResourceLoadComplete
    {
        get { return resourceLoadComplete; }
    }

    ResourceProvider resourceProvider;

    string roomID;
    public string RoomID
    {
        get { return roomID; }
    }

    [Header("UI Part")]

    public ServerRoomContainer container;
    public PlayerRoomContainer playersContainer;
    public List<PlayerEntity> playerEntities;

    public UnityAction<PlayerEntity> OnGamePlayerStart;


    public override void Awake()
    {
        base.Awake();
        //discovery.OnServerFound.AddListener((result)=>container.OnDiscoveredServer(result));
        singleton = this;
        resourceProvider= GetComponent<ResourceProvider>();
    }

    public void StartClient(string address)
    {
        singleton.networkAddress= address;
        StartClient();
    }

    
    public override void OnClientChangeScene(string newSceneName, SceneOperation sceneOperation, bool customHandling)
    {
        base.OnClientChangeScene(newSceneName, sceneOperation, customHandling);
        UnityEngine.Debug.Log($"OnClientChangeScene:{SceneManager.GetActiveScene()}");
    }

    public override void OnServerChangeScene(string newSceneName)
    {
        base.OnServerChangeScene(newSceneName);
        
       
    }

    public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer, GameObject gamePlayer)
    {
        return true;
    }

    public override void OnRoomServerPlayersReady()
    {
        // calling the base method calls ServerChangeScene as soon as all players are in Ready state.
#if UNITY_SERVER
            base.OnRoomServerPlayersReady();
#endif
    }

    //public override GameObject OnRoomServerCreateRoomPlayer(NetworkConnectionToClient conn)
    //{
    //    return OnCreatePlayer(conn, new CreatePlayerMessage());
    //}

    //public override GameObject OnRoomServerCreateGamePlayer(NetworkConnectionToClient conn, GameObject roomPlayer)
    //{
    //    return OnCreateGamePlayer(conn, new CreatePlayerMessage());
    //}

    public override void OnRoomClientConnect()
    {
        base.OnRoomClientConnect();
        if (!clientLoadedScene)
        {
            if (!NetworkClient.ready)
                NetworkClient.Ready();
            NetworkClient.AddPlayer();
        }
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        //if (!clientLoadedScene)
        //{
        //    if (!NetworkClient.ready)
        //        NetworkClient.Ready();
        //    NetworkClient.AddPlayer();
        //}
    }

    GameObject OnCreatePlayer(NetworkConnectionToClient conn, CreatePlayerMessage message)
    {

        GameObject gameobject = Instantiate(roomPlayerPrefab.gameObject);

        PlayerControl control = gameobject.GetComponent<PlayerControl>();
        PlatformOperationGenerator.instance.ConfigurePlayerControl(control);
        return gameobject;
    }

    GameObject OnCreateGamePlayer(NetworkConnectionToClient conn, CreatePlayerMessage message)
    {
        foreach (var player in FindObjectsOfType<NetworkRoomPlayerExt>())
        {
            player.gameObject.SetActive(true);
            player.gameObject.AddComponent<PlayerEntity>();
        }
        GameObject gameobject = Instantiate(playerPrefab.gameObject);
        var playerEntity = gameobject.GetComponent<PlayerEntity>();
        //playerEntity.gameObject.SetActive(false);
        //OnGamePlayerStart?.Invoke(playerEntity);
        PlayerControl control = gameobject.GetComponent<PlayerControl>();
        PlatformOperationGenerator.instance.ConfigurePlayerControl(control);
        return gameobject;
    }


    public void LoadResources()
    {
        resourceProvider.OnResourcesLoadSuccess += ()=>resourceLoadComplete = true;
        resourceProvider.LoadStandardResources();
    }

    public override void OnGUI() { }

}
