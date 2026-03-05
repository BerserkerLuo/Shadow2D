using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using UILib;
using UnityEngine.Events;

public class EventTriggerListener : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public delegate void VoidDelegate(GameObject go, PointerEventData pointEventData);
    public Action<GameObject, PointerEventData> onClick;
    public Action<GameObject, PointerEventData> onDown;
    public Action<GameObject, PointerEventData> onEnter;
    public Action<GameObject, PointerEventData> onExit;
    public Action<GameObject, PointerEventData> onUp;
    public Action<GameObject, BaseEventData> onSelect;
    public Action<GameObject, BaseEventData> onDSelect;


    //进入标记
    private bool IsEnter { set; get; } = false;
    //按下标记
    private bool IsDown { set; get; } = false;



    //是否在长按
    private bool IsPress { set; get; } = false;
    //按下的时长
    private float DownTime { set; get; } = 0f;
    //按压多久触发Press回调
    public float PressDurationTime { set; get; } = 999f;
    //长按回调
    public Action<GameObject> onPress = null;


    //点击次数
    private int ClickTimes { set; get; } = 0;
    //按下抬起之后 过去的时间
    private float ClickElapseTime { set; get; } = 0f;
    //在多少秒内双击触发回调
    public float DoubleClickTriggerTime { set; get; } = 0f;
    //双击回调
    public Action<GameObject> onDoubleClick = null;



    //是否显示Tips
    private bool IsShowTips { set; get; } = false;
    //进入了多久
    public float EnterTime { set; get; } = 0f;
    //悬停多久触发Tips
    public float TipsTime { set; get; } = 999f;
    //显示提示
    public Action<GameObject> onTips = null;
    //隐藏提示
    public Action<GameObject> onHideTips = null;

    public bool EnableFilter
    {
        get { return m_bEnableFilter; }
        set { m_bEnableFilter = value; }
    }

    static public EventTriggerListener Get(GameObject go){
        EventTriggerListener listener = go.GetComponent<EventTriggerListener>();
        if (listener == null) listener = go.AddComponent<EventTriggerListener>();
        return listener;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Vector2.Distance(eventData.position, eventData.pressPosition) > Screen.width * 0.05f)
            return;
        
        //播放按钮点击音效
        //MusicMgr.Singleton.PlayShortMusic("BtnClick");
        //MusicMgr.Singleton.PlayMusicByXmlID(1003);

        if (onClick != null)
            onClick(gameObject, eventData);

        if (!IsPress)
            ClickTimes += 1;
        else
            IsPress = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (onDown != null)
            onDown(gameObject, eventData);

        IsDown = true;
        DownTime = 0;
    }

    public void OnPointerEnter(PointerEventData eventData){
        if (onEnter != null) 
            onEnter(gameObject, eventData);

        IsEnter = true;
    }

    public void OnPointerExit(PointerEventData eventData){
        if (onExit != null)
            onExit(gameObject, eventData);

        if (IsShowTips && onHideTips != null)
            onHideTips(gameObject);

        IsDown = false;
        IsPress = false;
        IsShowTips = false;
        IsEnter = false;
        EnterTime = 0;
    }

    public void OnPointerUp(PointerEventData eventData){
        if (onUp != null)
            onUp(gameObject, eventData);

        IsDown = false;
    }

    void ISelectHandler.OnSelect(BaseEventData eventData){
        if (onSelect != null) 
            onSelect(gameObject, eventData);
    }

    void IDeselectHandler.OnDeselect(BaseEventData eventData){
        if (onDSelect != null) 
            onDSelect(gameObject, eventData);
    }

    private void Update(){
        CheckPress();
        CheckDoubleClick();
        CheckTips();
    }

    //判断长按
    private void CheckPress() {
        if (onPress == null || !IsDown || IsPress)
            return;

        DownTime += Time.deltaTime;
        if (DownTime < PressDurationTime)
            return;
            
        IsPress = true;
        onPress(gameObject);  
    }

    //判断双击
    private void CheckDoubleClick() {
        if (onDoubleClick == null || ClickTimes < 1)
            return;

        bool bReset = false;
        ClickElapseTime += Time.deltaTime;
        if (ClickElapseTime <= DoubleClickTriggerTime && ClickTimes >= 2){
            onDoubleClick(gameObject);
            bReset = true;
        }
        else if (ClickElapseTime > DoubleClickTriggerTime){
            bReset = true;
        }

        if (bReset){
            ClickTimes = 0;
            ClickElapseTime = 0;
        }
    }

    //Tips
    private void CheckTips() {

        if (onHideTips == null || onTips == null)
            return;
        if (!IsEnter || IsShowTips)
            return;

        EnterTime += Time.deltaTime;
        if (EnterTime < TipsTime)
            return;

        onTips(gameObject);
        IsShowTips = true;
    }

    private bool m_bEnableFilter = true;
}