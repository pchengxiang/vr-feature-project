using Firebase.Firestore;
using Firebase.Sample.Firestore;
using Mirror;
using Mirror.Discovery;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FirebaseServerRoomContainer : MonoBehaviour
{
    public Item Pattern;
    public Button CreateRoomBtn;
    public Button RefreshRoomsBtn;
    public Button SearchRoomBtn;
    public TextMeshProUGUI ipField;

    public string roomCollectionKey = "rooms";
    bool initialized = false;
    public bool Initialized
    {
        get { return initialized; }
    }


    readonly List<Item> columns = new List<Item>();
    readonly Dictionary<string, Dictionary<string,object>> discoveredServers = new Dictionary<string, Dictionary<string,object>>();
    ListenerRegistration registration;
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
            NetworkConnectionManager.singleton.StartHost();
        });

        SearchRoomBtn?.onClick.AddListener(() => NetworkConnectionManager.singleton.StartClient(ipField.text));
        //RefreshRoomsBtn.onClick.AddListener(() => Refresh());
        //注意這個函數，可能是導致遊戲閃退原因
        Refresh();
        //handler.ReadCollectionData(roomCollectionKey, (snapshot) =>
        //   discoveredServers[snapshot.Id] = snapshot.ToDictionary(),
        //   (snapshot) =>UpdateServerList()
        //);
        

    }

    //Disable all task from the listener of Firestore Collection.
    private void OnDisable()
    {
        registration.Stop();
    }

    private void OnDestroy()
    {
        discoveredServers.Clear();
        foreach(var item in columns)
        {
            Destroy(item.gameObject);
        }
        columns.Clear();
        
    }

    public void Refresh()
    {
        registration = FirebaseUtils.firestore.Collection(roomCollectionKey).Listen((snapshot) =>
        {
            discoveredServers.Clear();
            previousServersCount = 0;
            foreach (var item in snapshot)
            {
                discoveredServers[item.Id] = item.ToDictionary();
            }
            UpdateServerList();
        });
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
        foreach (var key in discoveredServers.Keys)
        {
            UnityAction action = () => { Connect(key); };
            columns[i].uri.text = key;
            columns[i].Button.onClick.RemoveAllListeners();
            columns[i].Button.onClick.AddListener(action);
            i++;  
        }   
        UpdateColumnSize();
        previousServersCount = discoveredServers.Count;
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
   

    void Connect(string roomID)
    {
        NetworkConnectionManager.singleton.StartClient(roomID);
    }
}
