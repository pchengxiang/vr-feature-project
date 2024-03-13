using Firebase.Database;
using Firebase.Extensions;
using Mirror;
using QuickType;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseDataBaseHelper : MonoBehaviour
{
    private static FirebaseDataBaseHelper instance;
    public static FirebaseDataBaseHelper Instance
    {
        get { return instance; }
    }
    public static DatabaseReference root;
    public static DatabaseReference GameData;
    public static string CardData = typeof(Card).Name;
    public static string MonsterData = typeof(Monster).Name;
    public static string RoomData = typeof(Room).Name;
    public static string LevelData = typeof(Level).Name;

    public delegate void OnGetJson(string jsonData);

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        root = FirebaseDatabase.DefaultInstance.RootReference;
        GameData = root.Child("GameData");
    }
#if UNITY_EDITOR
    void ForDebug()
    {
        GetJsonFrom(CardData, (data) =>
        {
            //Debug.Log(data);
            var content = Unserialize.FromJson<Card[]>(data);
            IResource item = content[0];
            Debug.Log(item);
            Card item2 = (Card)item;
            Debug.Log(item2.Description);
        });
    }
#endif

    public void GetJsonFrom(string block,OnGetJson doSomething)
    {
        GameData.Child(block).KeepSynced(true);
        GameData.Child(block).GetValueAsync().ContinueWithOnMainThread(task => {
           if (task.IsFaulted)
           {
               Debug.LogError($"Database Exception:{typeof(Card)} is null");
           }
           else if (task.IsCompleted)
           {
               DataSnapshot snapshot = task.Result;
               doSomething(snapshot.GetRawJsonValue());
               GameData.Child(block).KeepSynced(false);
           }
   });
    }
}
