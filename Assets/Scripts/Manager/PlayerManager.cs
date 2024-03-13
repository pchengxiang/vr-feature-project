using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    float max_hp;
    public float Max_Hp { get { return max_hp; } set { max_hp = value; } }
    float max_ap;
    public float Max_AP { get { return max_ap; } set { max_ap = value; } }
    float damage;
    public float Damage { get { return damage; } set { damage = value;} }

    float remainTime;
    public float RemainTime
    {
        get { return remainTime; }
        set { remainTime = value; }
    }
    public List<PlayerEntity> players = new();
    static int playerNo = 0;
    #region µü·J
    public static string HPTerm = "Hp";
    public static string APTerm = "Ap";
    public static string DamageTerm = "Damage";
    public static string DeckTerm = "Deck";
    public static string DrawCountTerm = "DrawCount";
    public static string TimeTerm = "Time";
    public static string DisplayTerm = "DisplayName";
    
    #endregion
    private void Awake()
    {
        Init();
    }
    void Init()
    {
        if (instance == null) instance = this;
        PlayerPrefs.SetFloat(HPTerm, 10);
        PlayerPrefs.SetFloat(APTerm, 5);
        PlayerPrefs.SetInt(DrawCountTerm, 3);
        PlayerPrefs.SetFloat(DamageTerm, 1);
        damage = PlayerPrefs.GetFloat(DamageTerm);
        max_hp = PlayerPrefs.GetFloat(HPTerm);
        max_ap = PlayerPrefs.GetFloat(APTerm);
        PlayerPrefs.SetFloat(TimeTerm,max_ap);
        PlayerPrefs.SetString(DeckTerm, "beginner");
        PlayerPrefs.SetString(DisplayTerm, "À°§Ú¼µ¤Q¬í");
    } 

    public void RegisterPlayer(PlayerEntity player)
    {
        players.Add(player);
    }

    public void EnterNextRound()
    {
        //PlayerPrefs.SetFloat("Time", max_ap);
        PlayerPrefs.SetFloat(APTerm, instance.Max_AP);
        PlayerPrefs.SetFloat(DamageTerm, instance.Damage);
        foreach (var player in players)
        {
            player.StartTurn();
        }
    }

    public void LoseHp(float value)
    {
        Debug.Log($"{PlayerPrefs.GetString(DisplayTerm)}'s hp -" + value);
        PlayerPrefs.SetFloat(HPTerm, PlayerPrefs.GetFloat(HPTerm) - value);
        players[playerNo].ResetUIData();
    }

    public void ChangeMoveSpeed(float time)
    {
        foreach(PlayerEntity playerEntity in players)
        {
            Assert.IsNotNull(playerEntity.MoveProvider);
            playerEntity.MoveProvider.MoveSpeed = time;
            playerEntity.TurnProvider.turnSpeed = time * 60;
        }
    }

}
