using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static PageManager;

public class HomePage : MonoBehaviour
{
    public Button Button1;
    public Button Button2;
    public Button Button3;
    public List<Button> BackToHome;

    // Start is called before the first frame update
    void Start()
    {
        OnLoginBeforeLoad();
//#if DEBUG
//        FirebaseAuthManager.instance.emailLoginField.text = "1234@gmail.com";
//        FirebaseAuthManager.instance.passwordLoginField.text = "123456";
//        FirebaseAuthManager.instance.Login();
//#endif
    }

    public void OnLoginBeforeLoad()
    {
        Button1.onClick.RemoveAllListeners();
        Button2.onClick.RemoveAllListeners();
        Button1.GetComponentInChildren<Text>().text = "SignUp";
        Button2.GetComponentInChildren<Text>().text = "SignIn";
        Button1.onClick.AddListener(() => GoToAuth(Page.RegisterPage));
        Button2.onClick.AddListener(() => GoToAuth(Page.LoginPage));
        foreach(Button b in BackToHome)
        {
            b.onClick.AddListener(() =>
            {
                instance.JumpToPage(instance.mainPage);
            });
        }

        if(FirebaseAuthManager.instance != null)
            FirebaseAuthManager.instance.LoginInSuccessEvent.AddListener(OnLoginAfterLoad);
    }

    public void OnLoginAfterLoad()
    {
        BackToTitle();
        
        Button1.onClick.RemoveAllListeners();
        Button2.onClick.RemoveAllListeners();
        Button3.gameObject.SetActive(true);

        Button1.GetComponentInChildren<Text>().text = "Multiplayer";
        Button2.GetComponentInChildren<Text>().text = "Account Information";
        Button1.onClick.AddListener(() => instance.JumpToPage(Page.ServerListPage));
        Button2.onClick.AddListener(() => instance.JumpToPage(Page.UserInformationPage));
        AddressableManager.Instance.ClearCache();
    }

    public void GoToAuth(Page page)
    {
        instance.JumpToPage(page);
    }

    public void BackToTitle()
    {
        instance.JumpToPage(instance.mainPage);
    }

}
