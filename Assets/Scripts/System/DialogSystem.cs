using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System.Text.RegularExpressions;

public class DialogSystem : MonoBehaviour
{
    Regex getTitleRex = new Regex(@"\[(.*)\]");
    int stopTime = 0;
    public UnityEvent<string, int> stopEvent; 
    
    [Header("UI組件")]
    //用來引用TextMeshPro的文本框
    public TextMeshProUGUI textLabel;
    public Image faceImage;

    [Header("文本文件")]
    public TextAsset textFile;
    string textTitle;
    public int index;
    public float textSpeed;

    bool textFinished;//用來判斷文字是否打完
    bool cancelTyping;//用來取消打字

    List<string> textList = new List<string>();

    float currentTime = 0f;
    public float updateTime = 1f;
    
    void Awake()
    {
        GetTextFromFile(textFile);
    }
    private void OnEnable() 
    {
        textFinished = true;
        StartCoroutine(SetTextUI());    
    }
    void Update()
    {

        if (!textFinished)
            return;

        currentTime += Time.deltaTime;

        if (currentTime > updateTime && !cancelTyping)
        {
            currentTime = 0;
            if (index == textList.Count)
            {
                stopEvent?.Invoke(textTitle,index);
                gameObject.SetActive(false);
                index = 0;
                return;
            }
            Match match = getTitleRex.Match(textList[index]);
            print(match.Captures.Count);
            if (match.Captures.Count == 1 && match.Captures[0].Value != string.Empty)
                OnMark(textList[index], index);
            else
                StartCoroutine(SetTextUI());
        }else if(!textFinished && !cancelTyping)
        {
            cancelTyping = true;
        }
            
        
    }

    void GetTextFromFile(TextAsset file) 
    {
        textList.Clear();
        index = 0;

        var lineDate = file.text.Split("\n");

        foreach(var line in lineDate)
        {
            textList.Add(line);
            print(textList);
        }
        

         
    }

    void OnMark(string mark, int lineNo)
    {
        switch (lineNo)
        {
            case 1:
                textTitle = mark;
                break;
            default:
                if(mark == "stop")
                {
                    ++stopTime;
                    stopEvent?.Invoke(textTitle, stopTime);
                }
                break;
        }
        index++;
    }
    
    IEnumerator SetTextUI()
    {
        //禁止玩家在文字還沒輸入完時就讀取下一行
        textFinished = false;
        //將文字設定成一個一個跑出來
        textLabel.text = "";

        /*
        switch (textList[index])
        {
            case "A":
                index++;
                break;
            case "B":
                index++;
                break;
        }*/


        int letter = 0;

        while (!cancelTyping && letter < textList[index].Length -1)
        {
            textLabel.text += textList[index][letter];
            letter++;
            yield return new WaitForSeconds(textSpeed);
        }
        textLabel.text = textList[index];
        cancelTyping = false;
        textFinished = true;
        index++;
    }

    void OnReadingStopWord()
    {

    }
}
