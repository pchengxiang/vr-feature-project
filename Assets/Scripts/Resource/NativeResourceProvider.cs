using QuickType;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NativeResourceProvider: SingletonScriptableObject<NativeResourceProvider>
{
    static string resourcePath = Application.streamingAssetsPath;
    string configurePath = resourcePath + "/Configurations.json";

    public string ConfigurePath { get; set; }
    readonly ResourceLoader loader = new ResourceLoader();
    public ResourceLoader Loader { get { return loader; } }
    bool attachData = false;
    public bool AttachData
    {
        get { return attachData; }
    }
    readonly List<string> resources = new List<string>();
    List<Card> cards;
    public List<Card> CardResources
    {
        get { return cards; }
    }

    List<Monster> monsters;
    public List<Monster> MonsterResources
    {
        get { return monsters; }
    }

    List<Room> rooms;
    public List<Room> RoomResources
    {
        get { return rooms; }
    }

    List<Level> levels;
    public List<Level> LevelResources
    {
        get { return levels; }
    }

    List<Deck> decks;
    public List<Deck> DeckResources
    {
        get { return decks; }
    }

    readonly Dictionary<string, Card> cardDict= new Dictionary<string, Card>();
    readonly Dictionary<string, Monster> monsterDict = new Dictionary<string, Monster>();
    readonly Dictionary<string,Room> roomDict = new Dictionary<string, Room>();
    readonly Dictionary<string,Level> levelDict = new Dictionary<string, Level>();
    readonly Dictionary<string, Deck> deckDict = new Dictionary<string, Deck>();
    
    // Start is called before the first frame update
    public bool LoadResources<T>()
    {
        if (loader.LoadConfigurations(configurePath))
        {
            switch (typeof(T).Name)
            {
                case nameof(Card):
                    cards = loader.Load<Card>(resourcePath);
                    foreach(Card card in cards)
                        if(!cardDict.TryAdd(card.Name, card))
                            Debug.LogWarning("Loading error the card of " + card.Name);
                    break;
                case nameof(Monster):
                    monsters = loader.Load<Monster>(resourcePath);
                    foreach (Monster monster in monsters)
                        if (!monsterDict.TryAdd(monster.Name, monster))
                            Debug.LogWarning("Loading error the monster of " + monster.Name);
                    break;
                case nameof(Room):
                    rooms = loader.Load<Room>(resourcePath);
                    foreach (Room room in rooms)
                        if (!roomDict.TryAdd(room.Name, room))
                            Debug.LogWarning("Loading error the room of " + room.Name);
                    break;
                case nameof(Level):
                    levels = loader.Load<Level>(resourcePath);
                    foreach (Level level in levels)
                        if (!levelDict.TryAdd(level.Name, level))
                            Debug.LogWarning("Loading error the level of " + level.Name);
                    break;
                case nameof(Deck):
                    decks = loader.Load<Deck>(resourcePath);
                    foreach (Deck deck in decks)
                        if (!deckDict.TryAdd(deck.Name, deck))
                            Debug.LogWarning("Loading error the deck of " + deck.Name);
                    break;

            }
            resources.Add(typeof(T).Name);
            Debug.Log("Loaded Resources.");
        }
        else
            attachData = false;
        return attachData;
    }

    public Dictionary<string,T> GetDict<T>() where T : class
    {
        if (!resources.Contains(typeof(T).Name))
            LoadResources<T>();
        switch (typeof(T).Name)
        {
            case nameof(Card):
                return cardDict as Dictionary<string, T>;
            case nameof(Monster):
                return monsterDict as Dictionary<string, T>;
            case nameof(Room):
                return roomDict as Dictionary<string, T>;
            case nameof(Level):
                return levelDict as Dictionary<string, T>;
            case nameof(Deck):
                return deckDict as Dictionary<string, T>;
            default:
                Debug.LogWarning($"{typeof(T).Name} could not found.");
                return null;
        }
    }

    public T GetDictItem<T>(string item) where T : class
    {
        if (!resources.Contains(typeof(T).Name))
            LoadResources<T>();
        switch (typeof(T).Name)
        {
            case nameof(Card):
                return cardDict[item] as T;
            case nameof(Monster):
                return monsterDict[item] as T;
            case nameof(Room):
                return roomDict[item] as T;
            case nameof(Level):
                return levelDict[item] as T;
            case nameof(Deck):
                return deckDict[item] as T;
            default:
                Debug.LogWarning($"{item} could not found.");
                return null;
        }
    }
}
