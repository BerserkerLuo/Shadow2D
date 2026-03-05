using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Client.UI.UICommon
{
    public delegate bool UIEvent(IXUIObject uiObject);

    public enum EnumTipType
    {
        eTypeNormal,
        eTypeTileContent,
        eTypeContent,
    }

    public class TipData
    {
        public EnumTipType ETipType;
        public string TipTitle;
        public string TipContent;
    }

    public interface IXUIObject
    {
        PointerEventData PointEventData {get; set;}

        GameObject CachedGameObject { get; }
        Transform CachedTransform { get; }
        RectTransform CachedRectTransform { get; }
        IXUIObject parent { get; set; }
        object DlgBehaviour { get; set; }
        CanvasGroup CanvasGroup { get; }
        Vector2 RealSize { get; }
        Vector2 RelativeSize { get; }
        string Tips { get; set; }
        Bounds AbsoluteBounds { get; }
        Bounds RelativeBounds { get; }
        GraphicRaycaster Raycaster { get; }

        bool IsError { get; }
        float Alpha { get; set; }
        float ClickCd { get; set; }
        
        bool IsEnableOpen { get; set; }
        object ClickCustomData { get; set; }

        IXUIObject GetUIObject(string strPath);
        bool IsVisible();
        void SetVisible(bool bVisible);
        bool IsEnable();
        void SetEnable(bool bEnable);
        void SetGray(bool bGray);
        void RegisterClickEventHandler(UIEvent btnClickHandler);
        void RegisterDownEventHandler(UIEvent btnDownHandler);
        void RegisterUpEventHandler(UIEvent btnUpHandler);
        void RegisterEnterEventHandler(UIEvent btnUpHandler);
        void RegisterExitEventHandler(UIEvent btnUpHandler);

        void RegisterDragEventHandler(UIEvent eventHandler);
        void RegisterEndDragEventHandler(UIEvent eventHandler);
        void RegisterBeginDragEventHandler(UIEvent eventHandler);
        void RegisterDropEventHandler(UIEvent eventHandler);

        void RegisterDoubleClickEvent(UIEvent eventHandler,float triggerTime);
        void RegisterPressEvent(UIEvent eventHandler,float triggerTime);
        public void RegisterShowTipsEvent(UIEvent eventHandler, float tipsTime);
        public void RegisterHideTipsEvent(UIEvent eventHandler);

        void SetBeginDragOnDown(bool b);
        void SetAllowDrag(bool b);
        void SetAllowDrop(bool b);
        void SetDragConstraintRect(float xMin, float xMax, float yMin, float yMax);
        void SetDragSensitivity(float f);

        void Init();
    }
}
