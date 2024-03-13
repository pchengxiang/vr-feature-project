using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.XR.CoreUtils;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Rendering.Universal;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class PlatformOperationGenerator : MonoBehaviour
{
    public UnityEvent TransformIntoPortableUIEvent;
    public UnityEvent InitializedEvent= new();
    //public UnityEvent<bool> switchUIActiveStateEvent = new UnityEvent<bool>();
    static PlatformOperationGenerator m_instance;
    public static PlatformOperationGenerator instance
    {
        get
        {
            return m_instance;
        }
        private set { }
    }
    #region enum Operation Mode
    [System.Flags]
    public enum OperationModes
    {

        VR = 1,
        Computer = 2,
        Phone = 4
    }

    public enum OperationMode
    {
        VR = 1,
        Computer = 2,
        Phone = 4
    }

    public bool IsContainMode(OperationModes modes) {
        return ((int)modes & (int)instance.mode) != 0;
    }

    #endregion

    public bool Initialized;

    [SerializeField]
    private OperationMode mode = OperationMode.Computer;
    public OperationMode Mode
    {
        get { return mode; }
        set { mode = value; }
    }
    [Header("Player Body")]
    public GameObject Player;

    public Transform StartPos;

    [Header("Load Pattern.")]
    public AssetReference PlayerRef;
    public AssetReference XRPlayerRef;

    List<GameObject> prefabs = new();
    VirtualGamePadControl VirtualGamePad;
    XROrigin xr;
    //只有手機與電腦操作平台才有
    PlayerControl control;

    List<UtilityUI> portables = new();
    bool isUsingUI = true;
    public readonly bool IsUsingUI;
    Camera UICamera;


    private void Awake()
    {
        TransformIntoPortableUIEvent = new();
        m_instance = this;
    }

    private void Start()
    {
        InitializeOperationController();
    }

    public void Configure()
    {
        if (!Initialized)
        {
            //Debug.LogError($"{GetType().Name}:Initialize uncompletely yet, pls wait a moment.");
            InitializedEvent.AddListener(() => LoadGameObjects());
            return;
        }
        LoadGameObjects();
    }

    void LoadGameObjects()
    {
        foreach (var prefab in prefabs)
        {
            Instantiate(prefab);
            switch (prefab.name)
            {
                case "VirtualGamePad":
                    VirtualGamePad = gameObject.GetComponent<VirtualGamePadControl>();
                    VirtualGamePad?.gameObject.SetActive(!isUsingUI);
                    break;
                default:
                    break;
            }
        }
    }


    public void InitializeOperationController()
    {
#if !UNITY_EDITOR
        mode = GetCurrentPlatformOperatingMode();
#endif

        StartCoroutine("GetPlayerIfNetworkActive");

    }
    void GetPlayerIfNetworkActive()
    {
        //if (NetworkConnectionManager.singleton != null && NetworkConnectionManager.singleton.isNetworkActive)
        //    if (SceneManager.GetActiveScene().name == NetworkConnectionManager.singleton.GameplayScene)
        //    { 
        //        Debug.Log("Activate");
        //        Player = FindObjectOfType<PlayerEntity>().gameObject;
        //    }
        //var task = GeneratePlayer(mode);
        //await task.Task;
        GenerateMappingObjects(mode);
        if (mode == OperationMode.VR)
            TransformIntoPortableUIEvent.Invoke();
        InitializedEvent.Invoke();
        InitializedEvent.RemoveAllListeners();
        TransformIntoPortableUIEvent.RemoveAllListeners();
        Initialized = true;
    }

    AsyncOperationHandle<GameObject> GeneratePlayer(OperationMode mode)
    {
        AsyncOperationHandle<GameObject> task;
        
        switch (mode)
        {
            case OperationMode.VR:
                task = Addressables.InstantiateAsync(XRPlayerRef);
                
                if (Player != null)
                    task.Completed += (handle) =>
                    {
                        var obj = handle.Result;
                        xr = obj.GetComponentInChildren<XROrigin>();
                        xr.Origin = Player;
                    };
                break; 
            default:
                task = Addressables.InstantiateAsync(PlayerRef);
                break;
        }
        if (Player == null)
            task.Completed += (handle) =>
            {
                Player = handle.Result;
                if (StartPos != null)
                {
                    Player.transform.localPosition = Vector3.zero;
                    Player.transform.rotation = Quaternion.identity;
                    Player.transform.position = StartPos.transform.position;
                }
            };
        return task;
    }

    public OperationMode GetCurrentPlatformOperatingMode()
    {
        if (XRChecker.checkXRDeviceAvaliable())
            return OperationMode.VR;
#if UNITY_ANDROID || UNITY_ANDROID_API
        return OperationMode.Phone;
#elif UNITY_STANDALONE_WIN || PLATFORM_STANDALONE_WIN
        return OperationMode.Computer;
#else
        if (Application.platform == RuntimePlatform.OSXPlayer)
            return OperationMode.Computer;

        throw new ApplicationException("Unknown Platform Error");
#endif
    }
    #region Generate Relation Objects And Configure Settings

    void GenerateMappingObjects(OperationMode mode)
    {
        switch(mode)
        {
            case OperationMode.Phone:
                GeneratePhoneObjects();
                break;
            case OperationMode.VR:
                GenerateVRObjects();
                break;
            default:
                GenerateComputerObjects();
                break;
        }
    }

    void GenerateComputerObjects()
    {
    }

    void GenerateVRObjects()
    {
        LoadPlatformGameObjects();
    }

    void GeneratePhoneObjects()
    {
        LoadPlatformGameObjects();
    }

    public void ConfigurePlayerControl(PlayerControl control)
    {
        Player = control.gameObject;
        var camera = control.GetComponentInChildren<Camera>();
        if (camera == null)
        {
            camera = new GameObject("Perspective").AddComponent<Camera>();
            camera.transform.SetParent(Player.transform, false);
        }
        control = Player.GetComponent<PlayerControl>();
        if (control.Perspective == null)
        {
            control.Perspective = new CameraLook();
            control.PlayerCamera = camera;
        }
    }

    public void SetupUICamera(Camera CameraForUI)
    {
        UICamera = CameraForUI;
        var cameraData = control.PlayerCamera.GetUniversalAdditionalCameraData();
        cameraData.cameraStack.Add(CameraForUI);
    }
    
    /// <summary>
    /// 下載平台相關物件
    /// </summary>
    /// <param name="mode"></param>
    /// <returns></returns>

    AsyncOperationHandle<IList<GameObject>> LoadPlatformGameObjects()
    {
        var task = Addressables.LoadAssetsAsync<GameObject>(mode.ToString(),prefab =>
        {
           prefabs.Add(prefab);
        });
        return task;
    }

    #endregion

    public void RegisterPortableUI(UtilityUI ui)
    {
        TransformIntoPortableUIEvent.AddListener(()=>
        {
            ui.TransformIntoPortable();
        });
        portables.Add(ui);
    }


    public void UnRegisterPortableUI(UtilityUI ui)
    {
        portables.Remove(ui);
        ui.Freedom();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="s"></param>
    public void SwitchUIActiveState(bool s)
    {
        isUsingUI = s;
        OnSwitchUIActiveState(s);
    }

    public void OnSwitchUIActiveState(bool s)
    {
        if (mode == OperationMode.Phone)
            VirtualGamePad.gameObject.SetActive(!s);
    }

}
