

using UnityEngine;
using Game;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UILib;

public class UnityGameEntry : MonoBehaviour
{
    public static UnityGameEntry Instance = null;

    public Camera MainCamera;
    public Camera UICamera;
    public Transform UIRoot;
    public Transform CanvasRoot;
    public EventSystem eventSystem; 
    public GraphicRaycaster Raycaster;

    void Start()
    {
        Application.targetFrameRate = 180;

        Instance = this;

        DontDestroyOnLoad(this);

        UIGlobal.UICamera = UICamera;
        UIGlobal.MainCamera = MainCamera;

        GameMgr.Singleton.Init();
        GameMgr.Singleton.EnterWorld();

    }

    void Update()
    {
        GameMgr.Singleton.Update();
    }

    private void FixedUpdate()
    {
        GameMgr.Singleton.FixedUpdate();
    }

    void LateUpdate()
    {
        GameMgr.Singleton.LateUpdate();
    }

    private void OnApplicationQuit()
    {
        GameMgr.Singleton.OnApplicationQuit();
    }
}
