using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UILib;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragEventTriggerListener:MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler{
    public Action<GameObject, PointerEventData> dragStartCallBack;
    public Action<GameObject, PointerEventData> dragCallBack;
    public Action<GameObject, PointerEventData> dragEndCallBack;
    public Action<GameObject, PointerEventData> dropCallBack;

    public bool EnableFilter
    {
        get { return m_bEnableFilter; }
        set { m_bEnableFilter = value; }
    }

    public static DragEventTriggerListener Get(GameObject go) {
        DragEventTriggerListener listener = go.GetComponent<DragEventTriggerListener>();
        if (listener == null) listener = go.AddComponent<DragEventTriggerListener>();
        return listener;
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData){
        if (dragStartCallBack != null)
            dragStartCallBack(gameObject, eventData);
    }

    void IDragHandler.OnDrag(PointerEventData eventData){
        if (dragCallBack != null)
            dragCallBack(gameObject, eventData);
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData){
        if (dragEndCallBack != null)
            dragEndCallBack(gameObject, eventData);
    }

    void IDropHandler.OnDrop(PointerEventData eventData){
        if (dropCallBack != null)
            dropCallBack(gameObject, eventData);
    }


    private bool m_bEnableFilter = true;
}
