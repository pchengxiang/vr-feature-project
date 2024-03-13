using Firebase.Firestore;
using Firebase.Sample.Firestore;
using Mirror;
using Mirror.Discovery;
using QuickType;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ServerRoomContainer : MonoBehaviour
{
    public Item Pattern;
    public Button CreateRoomBtn;
    public Button RefreshRoomsBtn;
    public Button SearchRoomBtn;
    public TextMeshProUGUI ipField;
    readonly Dictionary<long, ServerResponse> discoveredServers = new Dictionary<long, ServerResponse>();
    public NetworkDiscovery discovery;

    //public string roomCollectionKey = "rooms";
    bool initialized = false;
    public bool Initialized
    {
        get { return initialized; }
    }


    readonly List<Item> columns = new List<Item>();
    public UIHandler handler;
    private int columnCount
    {
        get { return columns.Count; }
    }
    private int previousServersCount = 0;
    public int ApplyLimitPerOnce = 20;

    public void Start()
    {
        CreateRoomBtn.onClick.AddListener(() =>
        {
            discovery.AdvertiseServer();
            NetworkConnectionManager.singleton.StartHost();
        });

        SearchRoomBtn?.onClick.AddListener(() => NetworkConnectionManager.singleton.StartClient(ipField.text));
        RefreshRoomsBtn.onClick.AddListener(() => Refresh());
        

    }

    public void Refresh()
    {
        discoveredServers.Clear();
        for (int i = 0; i < previousServersCount; i++)
            columns[i].gameObject.SetActive(false);
        previousServersCount = 0;
        discovery.StartDiscovery();
    }


    public void UpdateServerList()
    {
        int i = 0;
        if(discoveredServers.Count >= columnCount)
        {
            for(i = 0;i< ApplyLimitPerOnce;i++)
            {
                Item item = Instantiate(Pattern, transform);
                item.gameObject.SetActive(false);
                columns.Add(item);
            }
        }
        i = 0;
        foreach (ServerResponse info in discoveredServers.Values)
        {
            UnityAction action = () => { Connect(info); };
            columns[i].uri.text = info.uri.ToString();
            columns[i].Button.onClick.RemoveAllListeners();
            columns[i].Button.onClick.AddListener(action);
            i++;  
        }   
        UpdateColumnSize();
        previousServersCount = discoveredServers.Count;
    }

    public void OnDiscoveredServer(ServerResponse info)
    {
        // Note that you can check the versioning to decide if you can connect to the server or not using this method
        discoveredServers[info.serverId] = info;
        UpdateServerList();
    }


    public void UpdateColumnSize()
    {
        var count = discoveredServers.Count;
        if(previousServersCount > count)
        {
            for(int i = count; i< previousServersCount;i++)
                columns[i].gameObject.SetActive(false);
        }else if(previousServersCount < count)
            for(int i = previousServersCount;i<count;i++)
                columns[i].gameObject.SetActive(true);
    }


    void Connect(ServerResponse info)
    {
        discovery.StopDiscovery();
        NetworkManager.singleton.StartClient(info.uri);
    }
}
