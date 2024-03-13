using System;
using UnityEngine;
using UnityEngine.Events;

public class PageManager : MonoBehaviour
{
    public enum Page
    {
        TitlePage = 1,
        LoginPage = 2,
        RegisterPage = 4,
        UserInformationPage = 8,
        ResetPasswordPage = 16,
        ServerListPage = 32
    }

    public enum PageGroup {
        Title = 1,
        Auth = 16,
        Network = 64
    }

    public static PageManager instance { get; private set; }

    

    public UnityEvent<Page> JumpToPageEvent = new();

    public Page currentPage;
    public Page mainPage = Page.TitlePage;

    public PageGroup currentGroup;

    public GameObject TitlePage;
    public GameObject LoginPage;
    public GameObject RegisterPage;
    public GameObject UserInformationPage;
    public GameObject ResetPasswordPage;
    public GameObject ServerListPage;

    public GameObject Title;
    public GameObject Network;
    public GameObject Auth;

    public void Awake()
    {
        instance = this;
    }

    public GameObject GetPageObject(Page page)
    {
        switch (page)
        {
            case Page.LoginPage:
                return LoginPage;
            case Page.RegisterPage:
                return RegisterPage;
            case Page.UserInformationPage:
                return UserInformationPage;
            case Page.ResetPasswordPage:
                return ResetPasswordPage;
            case Page.TitlePage:
                return TitlePage;
            case Page.ServerListPage:
                return ServerListPage;
            default:
                return null;
        }
    }

    public PageGroup GetPageGroup(Page page)
    {
        foreach(int t in Enum.GetValues(typeof(PageGroup)))
        {
            if ((int)page <= t)
                return (PageGroup)t;
        }
        throw new Exception("This page not belongs to any page group.");
    }

    public GameObject GetPageGroupObject(PageGroup group)
    {
        switch (group)
        {
            case PageGroup.Title:
                return Title;
            case PageGroup.Auth:
                return Auth;
            case PageGroup.Network:
                return Network;
            default:
                return null;
        }
    }

     


    /// <summary>
    /// 除了指定頁面外都關掉，只打開它。
    /// </summary>
    /// <param name="page">指定要打開的頁面</param>
    public void JumpToPage(Page page)
    {
        if(currentPage != page)
        {
            var pageGroup = GetPageGroup(page);
            if(pageGroup != currentGroup)
            {
                GetPageGroupObject(currentGroup).SetActive(false);
                GetPageGroupObject(pageGroup).SetActive(true);
                currentGroup = pageGroup;
            }
            JumpToPageEvent.Invoke(page);
            GetPageObject(currentPage).SetActive(false);
            GetPageObject(page).SetActive(true);
            currentPage = page;
        }
    }
}
