using Mono.Cecil;
using QuickType;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DuelDiskEntity : MonoBehaviour
{
    public Transform[] positions;
    //public Transform[] deck_positions;
    //public Transform[] discard_positions;
    //public List<string> deck = new List<string>();
    //List<string> cardsRef = new List<string>();
    //List<string> discard = new List<string>();

    [SerializeField]
    List<CardEntity> handCards = new List<CardEntity>();

    public List<CardEntity> HandCards
    {
        get { return handCards; }
    }
    [SerializeField]
    List<CardEntity> inDeck = new List<CardEntity>();
    public List<CardEntity> InDeck
    {
        get { return inDeck; }
    }
    [SerializeField]
    List<CardEntity> discards = new List<CardEntity>();

    public List<CardEntity> Discards
    {
        get { return discards; }
    }


    [System.Flags]
    public enum Mode
    {
        None = 0,
        inHand = 1,
        inDeck = 2,
        inDiscards = 4
    }
    Mode currentMode;
    public Mode CurrentMode
    {
        get { return currentMode; } 
    }

    //int positon_index = 0;
    //int deck_index = 0;
    //int discard_index = 0;
    //手牌上限
    int handCardsLimit = 5;
    // 每次補充手牌數量
    int oncePerFillingHandCards = 3;
    // 卡牌之間空出的大小
    public Vector3 Space = new Vector3(1, 0, 0);
    // 卡牌堆出現的中心點
    [SerializeField]
    Transform SpawnPoint;
    NativeResourceProvider provider;
    [SerializeField]
    PlayerEntity player;
    

    private void Awake()
    {
        provider = NativeResourceProvider.instance;
    }

    public void BindPlayer(PlayerEntity player)
    {
        this.player = player;
        LoadDeck(provider.GetDictItem<Deck>(PlayerPrefs.GetString("Deck")));
    }

    void LoadDeck(Deck d)
    {
        inDeck.Clear();
        discards.Clear();
        handCards.Clear();
        foreach (string card in d.Cards)
        {
            var entity = CardGenerator.instance.Spawn(card, provider.GetDictItem<Card>(card),SpawnPoint);
            entity.gameObject.SetActive(false);
            inDeck.Add(entity);
            entity.Deck = this;
        }
    }

    public void FillHandCards()
    {

        var time = 0;
        while(time < oncePerFillingHandCards && handCards.Count <= handCardsLimit)
        {
            if(inDeck.Count == 0) Shuffle();
            try
            {
                var next = inDeck[0];
                handCards.Add(next);
                inDeck.Remove(next);
            }
            catch (Exception e)
            {
                throw e;
            };
            time++;
        }
    }

    void Shuffle()
    {
        int random = 0;
        CardEntity tmp;
        inDeck.AddRange(discards);
        discards.Clear();
        for(int i=0; i < inDeck.Count;i++)
        {
            random = Random.Range(i + 1 , inDeck.Count);
            tmp = inDeck[i];
            inDeck[i] = inDeck[random];
            inDeck[random] = tmp;
        }
    }

    void ChangeMode(Mode mode)
    {
        currentMode = mode;
        switch (mode)
        {
            case Mode.inHand:
                foreach (CardEntity entity in discards)
                { entity.gameObject.SetActive(false); }
                foreach (CardEntity entity in inDeck)
                { entity.gameObject.SetActive(false); }
                break;
            case Mode.inDeck:
                foreach (CardEntity entity in discards)
                { entity.gameObject.SetActive(false); }
                foreach (CardEntity entity in handCards)
                { entity.gameObject.SetActive(false); }
                break;
            case Mode.inDiscards:
                foreach (CardEntity entity in inDeck)
                { entity.gameObject.SetActive(false); }
                foreach (CardEntity entity in handCards)
                { entity.gameObject.SetActive(false); }
                break;
            case Mode.None:
                foreach (CardEntity entity in discards)
                { entity.gameObject.SetActive(false); }
                foreach (CardEntity entity in inDeck)
                { entity.gameObject.SetActive(false); }
                foreach (CardEntity entity in handCards)
                { entity.gameObject.SetActive(false); }
                break;
        }
    }

    public void ShowDeck()
    {
        int count = inDeck.Count;
        if (currentMode == Mode.inDeck)
        {
            ChangeMode(Mode.None);
            return;
        }
        else
            ChangeMode(Mode.inDeck);
        int mid = count / 2 + 1;

        Vector3 currentPos = SpawnPoint.transform.localPosition - (mid - 1) * Space;
        foreach (CardEntity entity in inDeck)
        {
            entity.gameObject.SetActive(true);
            entity.gameObject.transform.position = currentPos;
            entity.AllowActivate = false;
            currentPos += Space;
        }
        currentMode = Mode.inDeck;
        //for(int i = 0; i < 5; i++)
        //{
        //    inDeck[i].gameObject.transform.position = transform.position + Space;
        //    inDeck[i].gameObject.SetActive(true);

        //    //CardGenerator.instance.Spawn(cardsRef[deck_index], provider.GetDictItem<Card>(cardsRef[deck_index]),deck_positions[positon_index]);
        //    deck_index = deck_index + 1;
        //    positon_index = (positon_index + 1) % deck_positions.Length;
        //    if(deck_index == cardsRef.Count)
        //    {
        //        break;
        //    }
        //}
    }

    public void ShowDiscard()
    {
        int count = discards.Count;
        int mid = count / 2 + 1;
        if (currentMode == Mode.inDiscards)
        {
            ChangeMode(Mode.None);
            return;
        }
        else
            ChangeMode(Mode.inDiscards);
        Vector3 currentPos = SpawnPoint.transform.position - (mid - 1) * Space;

        foreach (CardEntity entity in discards)
        {
            entity.gameObject.SetActive(true);
            entity.gameObject.transform.position = currentPos;
            entity.AllowActivate = false;
            currentPos += Space;
        }
        currentMode = Mode.inDiscards;
    }

    public void ShowHandCards()
    {
        int count = handCards.Count;
        int mid = count / 2 + 1;
        if (currentMode == Mode.inHand)
        {
            ChangeMode(Mode.None);
            return;
        }
        else
            ChangeMode(Mode.inHand);
        Vector3 currentPos = SpawnPoint.transform.position - (mid - 1) * Space;

        foreach (CardEntity entity in handCards)
        {
            entity.gameObject.SetActive(true);
            entity.gameObject.transform.position = currentPos;
            entity.AllowActivate = true;
            currentPos += Space;
        }
        currentMode = Mode.inHand;
    }

    public string GetDeckModeExpress()
    {
        switch (currentMode)
        {
            case Mode.inHand:
                return "手牌";
            case Mode.inDeck:
                return "牌組";
            case Mode.inDiscards:
                return "棄牌堆";
            default:
                return "無";
        }
    }

    public void OnCardActivated(CardEntity entity)
    {
        if (currentMode != Mode.inHand)
            return;
        CardEffectHandler.instance.EffectActive.Invoke(entity, entity.CardInfo);
        ChangeMode(Mode.None);
        handCards.Remove(entity);
        discards.Add(entity);
        player.ResetUIData();
    }

    #region 舊的卡牌系統部分

    //public void Next_Deck()
    //{
    //    if(deck_index != cardsRef.Count)
    //    {
    //        foreach(Transform slot in deck_positions)
    //        {
    //            Destroy(slot.GetChild(0).gameObject);
    //        }
    //        ShowDeck();
    //    }
    //}

    //public void Previous_Deck()
    //{
    //    if(deck_index >= frame)
    //    {
    //        foreach (Transform slot in deck_positions)
    //        {
    //            Destroy(slot.GetChild(0).gameObject);
    //        }
    //        if(deck_index == cardsRef.Count && cardsRef.Count % frame != 0)
    //        {
    //            deck_index -= (deck_index % frame + frame);
    //        }
    //        else
    //        {
    //            deck_index -= frame * 2;
    //        }
    //        ShowDeck();
    //    }
    //}

    //public void Next_Discard()
    //{
    //    if (discard_index != discard.Count)
    //    {
    //        foreach (Transform slot in discard_positions)
    //        {
    //            Destroy(slot.GetChild(0).gameObject);
    //        }
    //        ShowDiscard();
    //    }
    //}

    //public void Previous_Discard()
    //{
    //    if (discard_index >= frame)
    //    {
    //        foreach (Transform slot in discard_positions)
    //        {
    //            Destroy(slot.GetChild(0).gameObject);
    //        }
    //        if (discard_index == discard.Count && discard.Count % frame != 0)
    //        {
    //            discard_index -= (discard_index % frame + frame);
    //        }
    //        else
    //        {
    //            discard_index -= frame * 2;
    //        }
    //        ShowDiscard();
    //    }
    //}

    #endregion


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Card")
        {
            //other.GetComponentInParent<CardEffectHandler>().Invoke();
            CardManager.instance.Remove(other.transform.parent.GetComponent<CardEntity>());
        }
    }
}
