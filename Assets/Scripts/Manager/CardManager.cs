using QuickType;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CardManager : MonoBehaviour//SingletonScriptableObject<CardManager>
{
    public static CardManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private List<CardEntity> cards = new List<CardEntity>();
    public List<CardEntity> Cards
    {
        get { return cards; }
    }
    public int Size
    {
        get { return cards.Count; }
    }

    private MonsterPresenter[] presenters;

    public void Add(CardEntity card)
    {
        cards.Add(card);
    }

    public void Remove(CardEntity card)
    {
        Destroy(card.gameObject);
        cards.Remove(card);
    }
    public void Clear()
    {
        foreach (CardEntity card in cards)
            if(card != null)
                DestroyImmediate(card.gameObject);
        cards.Clear();
    }

    public CardEntity Find(string name)
    {
        return cards.Find((x) => x.name == name);
    }
}
