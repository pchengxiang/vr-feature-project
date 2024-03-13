using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTimeBattle : MonoBehaviour
{
    public static BulletTimeBattle instance;
    NativeResourceProvider provider;
    bool BulletTime = false;
    bool BattleStart = false;
    bool IsPlayerTurn = false;
    float DefaultTimeScale = 1.0f;
    float BulletTimeScale = 50f;
    //List<string> monsters;
    Transform position;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        provider = NativeResourceProvider.instance;
    }

    private void Update()
    {
        if (BattleStart)
        {
            if (IsPlayerTurn)
            {
                DecreaseTime();
                //Debug.Log(PlayerPrefs.GetFloat(PlayerManager.TimeTerm));
                if (!CheckRemainCost())
                {
                    EndPlayerTurn();
                }
            }
            else
            {
                IncreaseTime();
            }
            if (!CheckPlayerHealth())
            {
                EndBattle("Lose!");
            }else if (!CheckRemainMonsters())
            {
                EndBattle("Win!");
            }
            //for testing
            if (Input.GetKey(KeyCode.P))
            {
                print("test for player turn.");
                StartPlayerTurn();
            }else if (Input.GetKey(KeyCode.O))
            {
                EndPlayerTurn();
            }
            if (Input.GetKey(KeyCode.I))
            {
                EndBattle("Force Stop");
            }
        }
        //Debug.Log(PlayerPrefs.GetFloat("Time"));
    }

    public void StartBattle(Transform position)
    {
        this.position = position;
        //if(monsters.Count != 0 && MonsterManager.instance.Monsters.Count == 0)
            //SpawnMonsters(monsters);
        BattleStart = true;
        print("StartBattle");
        if (CheckRemainMonsters())
        {
            print($"Checked remain monsters:{MonsterManager.instance.Size}");
            MonstersAttack();
        }
    }

    public void StartPlayerTurn()
    {
        if (PlayerPrefs.GetFloat(PlayerManager.APTerm) > 0)
        {
            Debug.Log("Start Player Turn.");
            IsPlayerTurn = true;
            BulletTime = true;
            Time.timeScale = 1 / BulletTimeScale;
            PlayerManager.instance.ChangeMoveSpeed(BulletTimeScale);
            PlayerManager.instance.EnterNextRound();
        }
        else
        {
            Debug.Log("Not Enough AP");
        }
    }

    public void EndPlayerTurn()
    {
        IsPlayerTurn = false;
        BulletTime = false;
        Time.timeScale = DefaultTimeScale;
        PlayerManager.instance.ChangeMoveSpeed(DefaultTimeScale);
    }

    //public void SetMonsters(List<string> monsters)
    //{
        //this.monsters = monsters;
    //}

    void SpawnMonsters(List<string> monsters)
    {
        Vector3 p = position.position;
        foreach(string monster in monsters)
        {
            MonsterGenerator.instance.Spawn(monster, provider.GetDictItem<Monster>(monster), p);
            p = new Vector3(p.x + 3, p.y, p.z);
        }
        MonstersAttack();
    }

    void MonstersAttack()
    {
        foreach(MonsterEntity monster in MonsterManager.instance.Monsters)
        {
            monster.StartAttack();
        }
    }

    void DecreaseTime()
    {
        float remain = PlayerPrefs.GetFloat(PlayerManager.TimeTerm) - Time.deltaTime;
        if (remain <= 0)
        {
            PlayerPrefs.SetFloat(PlayerManager.TimeTerm, 0);
        }
        else
        {
            PlayerPrefs.SetFloat(PlayerManager.TimeTerm, remain);
        }        
    }

    void IncreaseTime()
    {
        float result = PlayerPrefs.GetFloat(PlayerManager.TimeTerm) + (Time.deltaTime / 2);
        if(result >= PlayerManager.instance.Max_AP)
        {
            PlayerPrefs.SetFloat(PlayerManager.TimeTerm, PlayerManager.instance.Max_AP);
        }
        else
        {
            PlayerPrefs.SetFloat(PlayerManager.TimeTerm, result);
        }       
    }
    /// <summary>
    /// �ˬd�Ѿl�ɶ��P����I�ƬO�_�٦�
    /// </summary>
    /// <returns></returns>
    bool CheckRemainCost()
    {
        if(PlayerPrefs.GetFloat(PlayerManager.APTerm) > 0 && PlayerPrefs.GetFloat(PlayerManager.TimeTerm) > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    bool CheckPlayerHealth()
    {
        if(PlayerPrefs.GetFloat(PlayerManager.HPTerm) <= 0)
        {
            return false;
        }
        else
        {
            return true;
        }      
    }

    bool CheckRemainMonsters()
    {
        return MonsterManager.instance.Size > 0;
    }
    public void EndBattle(string message = "")
    {
        Debug.Log(message);
        BulletTime = false;
        BattleStart = false;
        MonsterManager.instance.Clear();
        ItemManager.instance.Clear();
        StopAllCoroutines();
    }
}
