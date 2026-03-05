using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.UI.UICommon;
using UnityEngine;
using Client.UI;
using UILib;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using ProjectX;
using System.Collections;

namespace UILib
{
    public enum EnumUIEventType
    {
        eClick,
        eClickDown,
        eClickUp,
        eBeginDrag,
        eDrag,
        eEndDrag,
        eDrop,
        eEnter,
        eExit,
    }

    public abstract class XUIObjectBase : MonoBehaviour, IXUIObject
    {
        public PointerEventData PointEventData { get; set; }

        public TipData Tip
        {
            get
            {
                return m_tipData;
            }
            set
            {
                if (null != value && null == m_tipData)
                {
                    EventTriggerListener.Get(CachedGameObject).onUp = OnPointUp;
                    EventTriggerListener.Get(CachedGameObject).onDown = OnPointDown;
                }
                m_tipData = value;
            }
        }

        public IXUIObject parent
        {
            get { return m_parent; }
            set { m_parent = value; }
        }

        public object DlgBehaviour
        {
            get { return m_dlgBehaviour; }
            set { m_dlgBehaviour = value; }
        }

        public CanvasGroup CanvasGroup
        {
            get { return m_canvasGroup; }
        }

        public virtual Vector2 RealSize
        {
            get
            {
                Vector2 vector = Vector2.zero;
                vector.x = transform.lossyScale.x;
                vector.y = transform.lossyScale.y;
                return vector;
            }
        }

        public virtual Vector2 RelativeSize
        {
            get
            {
                Vector2 vector = Vector2.zero;
                vector.x = transform.localScale.x;
                vector.y = transform.localScale.y;
                return vector;
            }
        }

        public virtual Bounds AbsoluteBounds
        {
            get
            {
                return m_AbsoluteBounds;
            }
        }

        public Bounds RelativeBounds
        {
            get
            {
                Vector3 min = CachedTransform.InverseTransformPoint(AbsoluteBounds.min);
                Vector3 max = CachedTransform.InverseTransformPoint(AbsoluteBounds.max);
                Bounds b = new Bounds(min, Vector3.zero);
                b.Encapsulate(max);
                return b;
            }
        }


        GraphicRaycaster _Raycaster = null;
        public GraphicRaycaster Raycaster {
            get {
                if (_Raycaster != null)
                    return _Raycaster;
                _Raycaster = CachedRectTransform.GetComponent<GraphicRaycaster>();
                return _Raycaster;
            }
        }

        public bool IsError
        {
            get { return false; }
        }

        public bool IsInited { get { return m_bInited; } }

        public GameObject CachedGameObject { get { 
                if (m_Go == null)
                    m_Go = gameObject; 
                return m_Go; 
            }
        }

        public Transform CachedTransform { get { if (m_Trans == null) m_Trans = transform; return m_Trans; } }

        public RectTransform CachedRectTransform { get { if (m_RectTrans == null) m_RectTrans = GetComponent<RectTransform>(); return m_RectTrans; } }

        public virtual float Alpha
        {
            get { return m_fAlpha; }
            set { m_fAlpha = value; }
        }

        public float ClickCd
        {
            get { return m_nClickCd; }
            set { m_nClickCd = value; }
        }

        public virtual bool IsEnableOpen { get; set; }

        public PointerEventData EventData { get; set; }

        public object ClickCustomData { get; set; }
        public string Tips { get { return m_strTipText; } set { m_strTipText = value; } }

        //public static FilterUIEventHandler ActionUIEventFilter { get { return s_actionUIEventFilter; } }

        public virtual IXUIObject GetUIObject(string strPath) { return null; }

        public bool IsVisible() { return gameObject.activeInHierarchy; }

        public bool IsEnable()
        {
            return m_bEnable;
        }

        public virtual void SetEnable(bool bEnable)
        {
            m_bEnable = bEnable;
            if (null != m_button)
            {
                m_button.interactable = bEnable;
            }

            SetGray(!bEnable);
        }

        public virtual void SetGray(bool bGray)
        {
            //Graphic[] imgAry = GetComponentsInChildren<Graphic>(true);
            //for (int i = 0; i < imgAry.Length; i++)
            //{
            //    Graphic img = imgAry[i];
            //    if (img != null)
            //        img.material = bGray ? GetGrayMat() : null;
            //}
            //else
            //{
            //    Graphic self = GetComponent<Graphic>();
            //    if (self != null)
            //        self.material = bGray ? GetGrayMat() : null;
            //}
        }

        //事件 eventTrigger
        //============================================================================
        private void TryInitEventTrigger() {
            if (null == m_eventTriggerListener)
                m_eventTriggerListener = EventTriggerListener.Get(CachedGameObject);
        }

        public virtual void RegisterClickEventHandler(UIEvent eventHandler){
            TryInitEventTrigger();
            this.m_eventHandlerOnClick = eventHandler;
            m_eventTriggerListener.onClick = this.OnPointClicked;
        }

        public void RegisterDownEventHandler(UIEvent eventHandler){
            TryInitEventTrigger();
            this.m_eventHandlerOnDown = eventHandler;
            m_eventTriggerListener.onDown = OnPointDown;
        }
        public void RegisterDoubleClickEvent(UIEvent eventHandler, float triggerTime){
            TryInitEventTrigger();
            m_eventDoubleClick = eventHandler;
            m_eventTriggerListener.onDoubleClick = OnDoubleClick;
            m_eventTriggerListener.DoubleClickTriggerTime = triggerTime;
        }
        public void RegisterUpEventHandler(UIEvent eventHandler){
            TryInitEventTrigger();
            this.m_eventHandlerOnUp = eventHandler;
            m_eventTriggerListener.onUp = OnPointUp;
        }

        public void RegisterEnterEventHandler(UIEvent eventHandler){
            TryInitEventTrigger();
            this.m_eventHandlerOnEnter = eventHandler;
            m_eventTriggerListener.onEnter = OnPointEnter;
        }

        public void RegisterExitEventHandler(UIEvent eventHandler) {
            TryInitEventTrigger();
            this.m_eventHandlerOnExit = eventHandler;
            m_eventTriggerListener.onExit = OnPointExit;
        }

        public void RegisterPressEvent(UIEvent eventHandler, float triggerTime){
            TryInitEventTrigger();
            m_eventPress = eventHandler;
            m_eventTriggerListener.onPress = OnPress;
            m_eventTriggerListener.PressDurationTime = triggerTime;
        }

        public void RegisterShowTipsEvent(UIEvent eventHandler, float tipsTime){
            TryInitEventTrigger();
            m_eventTips = eventHandler;
            m_eventTriggerListener.onTips = OnTips;
            m_eventTriggerListener.TipsTime = tipsTime;
        }

        public void RegisterHideTipsEvent(UIEvent eventHandler){
            TryInitEventTrigger();
            m_eventHideTips = eventHandler;
            m_eventTriggerListener.onHideTips = OnHideTips;
        }

        //事件回调
        //============================================================================

        private void OnPointClicked(GameObject go, PointerEventData data)
        {
            PointEventData = data;
            if (null != m_eventHandlerOnClick){
                if (m_nClickCd > 0)
                {
                    float curTime = Time.time;
                    if (curTime - m_nLastClickTime >= m_nClickCd)
                    {
                        if (null != m_eventHandlerOnClick)
                            m_eventHandlerOnClick(this);

                        m_nLastClickTime = curTime;
                    }
                }
                else
                {
                    if (null != m_eventHandlerOnClick)
                        m_eventHandlerOnClick(this);
                }
            }
            PointEventData = null;
        }

        private void OnDoubleClick(GameObject go)
        {
            if (this.m_eventDoubleClick != null)
                this.m_eventDoubleClick(this);

            PointEventData = null;
        }

        private void OnPointDown(GameObject go, PointerEventData data)
        {
            PointEventData = data;
            //XUITool.OnTip(false, this);
            if (this.m_drag != null)
            {
                if (this.m_drag.m_bAllowDrag && this.m_drag.m_bBeginDragOnDown)
                    SetDraggedPosition(data);
            }

            //点击检测cd
            if (m_nClickCd > 0)
            {
                float curTime = Time.time;
                if (curTime - m_nLastClickTime >= m_nClickCd)
                {
                    m_eventHandlerOnDown(this);
                    m_nLastClickTime = curTime;
                }
            }
            else
            {
                if (m_eventHandlerOnDown != null)
                    m_eventHandlerOnDown(this);
            }

            PointEventData = null;
        }

        private void OnPointUp(GameObject go, PointerEventData data)
        {
            PointEventData = data;

            if (m_nClickCd > 0)
            {
                float curTime = Time.time;
                if (curTime - m_nLastClickTime >= m_nClickCd)
                {
                    if (null != m_eventHandlerOnUp)
                        m_eventHandlerOnUp(this);

                    m_nLastClickTime = curTime;
                }
            }
            else
            {
                if (m_eventHandlerOnUp != null)
                    m_eventHandlerOnUp(this);
            }

            PointEventData = null;
        }

        private void OnPointEnter(GameObject go, PointerEventData data)
        {
            PointEventData = data;
            if (m_eventHandlerOnEnter != null)
                m_eventHandlerOnEnter(this);
            PointEventData = null;
        }
        private void OnPointExit(GameObject go, PointerEventData data)
        {
            PointEventData = data;
            if (m_eventHandlerOnExit != null)
                m_eventHandlerOnExit(this);
            PointEventData = null;
        }

        private void OnPress(GameObject go){
            if (this.m_eventPress != null)
                this.m_eventPress(this);
        }

        private void OnTips(GameObject go){
            if (this.m_eventTips != null)
                this.m_eventTips(this);
        }

        private void OnHideTips(GameObject go){
            if (this.m_eventHideTips != null)
                this.m_eventHideTips(this);
        }


        //////拖拽事件
        //======================================================================
        private void TryInitDrag()
        {
            if (m_drag == null) { m_drag = new CDrag(); }
            if (null == m_dragEventTriggerListener)
                m_dragEventTriggerListener = DragEventTriggerListener.Get(CachedGameObject);
        }
        public void RegisterBeginDragEventHandler(UIEvent eventHandler)
        {
            TryInitDrag();
            this.m_drag.m_eventHandlerOnBeginDrag = eventHandler;
            m_dragEventTriggerListener.dragStartCallBack = OnBeginDrag;
        }
        public void RegisterDragEventHandler(UIEvent eventHandler)
        {
            TryInitDrag();
            this.m_drag.m_eventHandlerOnDrag = eventHandler;
            m_dragEventTriggerListener.dragCallBack = OnDrag;
        }
        public void RegisterDropEventHandler(UIEvent eventHandler)
        {
            TryInitDrag();
            this.m_drag.m_eventHandlerOnDrop = eventHandler;
            m_dragEventTriggerListener.dropCallBack = OnDrop;
        }
        public void RegisterEndDragEventHandler(UIEvent eventHandler)
        {
            TryInitDrag();
            this.m_drag.m_eventHandlerOnEndDrag = eventHandler;
            m_dragEventTriggerListener.dragEndCallBack = OnEndDrag;
        }

        //拖拽回调
        //======================================================================
        private void OnBeginDrag(GameObject go, PointerEventData data)
        {
            PointEventData = data;
            if (this.m_drag.m_bBeginDragOnDown) return;
            this.m_drag.m_bStartDrag = true;

            if (this.m_drag.m_bAllowDrag)
            {
                Vector3 globalMousePos;
                if (RectTransformUtility.ScreenPointToWorldPointInRectangle(CachedRectTransform, data.position, data.enterEventCamera, out globalMousePos))
                {
                    m_drag.m_vOffset = CachedRectTransform.position - globalMousePos;
                }
            }

            if (this.m_drag.m_eventHandlerOnBeginDrag != null)
                this.m_drag.m_eventHandlerOnBeginDrag(this);
            PointEventData = null;
        }
        private void OnDrag(GameObject go, PointerEventData data)
        {
            PointEventData = data;
            if (this.m_drag.m_bAllowDrag){
                SetDraggedPosition(data);
            }
            if (this.m_drag.m_eventHandlerOnDrag != null)
                this.m_drag.m_eventHandlerOnDrag(this);
            PointEventData = null;
        }
        private void OnEndDrag(GameObject go, PointerEventData data)
        {
            PointEventData = data;
            if (!this.m_drag.m_bStartDrag) return;
            //SetDraggedPosition(data);
            m_drag.m_vLastPos = Vector3.zero;
            this.m_drag.m_bStartDrag = false;

            if (this.m_drag.m_eventHandlerOnEndDrag != null)
                this.m_drag.m_eventHandlerOnEndDrag(this);
            PointEventData = null;
        }

        private void OnDrop(GameObject go, PointerEventData data)
        {
            PointEventData = data;
            if (!this.m_drag.m_bAllowDrop) return;
            if (this.m_drag.m_eventHandlerOnDrop != null)
                this.m_drag.m_eventHandlerOnDrop(this);
            PointEventData = null;
        }

        //======================================================================

        public void SetBeginDragOnDown(bool b)
        {
            if (m_drag == null) { m_drag = new CDrag(); }
            this.m_drag.m_bBeginDragOnDown = b;
        }

        public void SetAllowDrag(bool b)
        {
            if (m_drag == null) { m_drag = new CDrag(); }
            this.m_drag.m_bAllowDrag = b;
        }

        public void SetAllowDrop(bool b)
        {
            if (m_drag == null) { m_drag = new CDrag(); }
            this.m_drag.m_bAllowDrop = b;
        }

        public void SetDragConstraintRect(float xMin, float xMax, float yMin, float yMax)
        {
            if (m_drag == null) { m_drag = new CDrag(); }
            this.m_drag.m_ConstraintRect.Set(xMin, xMax, yMin, yMax);
        }

        public void SetDragSensitivity(float f)
        {
            if (m_drag == null) { m_drag = new CDrag(); }
            this.m_drag.m_fSensitivity = f;
        }
        private void SetDraggedPosition(PointerEventData eventData)
        {
            // transform the screen point to world point int rectangle
            Vector3 globalMousePos;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(CachedRectTransform, eventData.position, eventData.pressEventCamera, out globalMousePos))
            {
                //if (m_drag.m_vLastPos == Vector3.zero)
                //{
                //    m_drag.m_vLastPos = globalMousePos;
                //}
                //else
                //{
                //    //Debug.Log(String.Format("DeltaPos x: {0} , y: {1}", vPosDelta.x, vPosDelta.y));
                //    globalMousePos = m_drag.m_vLastPos + (globalMousePos - m_drag.m_vLastPos) * m_drag.m_fSensitivity;
                //    m_drag.m_vLastPos = globalMousePos;
                //}
                //globalMousePos += m_drag.m_vOffset;//加上偏移量

                //xMin
                if (this.m_drag.m_ConstraintRect.x != float.NaN && globalMousePos.x <= this.m_drag.m_ConstraintRect.x)
                {
                    globalMousePos.x = this.m_drag.m_ConstraintRect.x;
                }

                //xMax
                if (this.m_drag.m_ConstraintRect.y != float.NaN && globalMousePos.x >= this.m_drag.m_ConstraintRect.y)
                {
                    globalMousePos.x = this.m_drag.m_ConstraintRect.y;
                }

                //yMin
                if (this.m_drag.m_ConstraintRect.width != float.NaN && globalMousePos.y <= this.m_drag.m_ConstraintRect.width)
                {
                    globalMousePos.y = this.m_drag.m_ConstraintRect.width;
                }

                //yMax
                if (this.m_drag.m_ConstraintRect.height != float.NaN && globalMousePos.y >= this.m_drag.m_ConstraintRect.height)
                {
                    globalMousePos.y = this.m_drag.m_ConstraintRect.height;
                }
                CachedRectTransform.position = globalMousePos;
            }
        }
        //////

        public virtual void SetVisible(bool bVisible)
        {
            //UIManager.singleton.SetVisible(gameObject, bVisible);
            if (CachedGameObject == null)
                return;

            CachedGameObject.SetActive(bVisible);
        }

        public virtual bool IsMouseIn()
        {
            return m_bIsMouseIn;
        }

        public virtual void Highlight(bool bTrue){

        }

        public virtual void Init()
        {
            m_bInited = true;
            m_button = GetComponent<Button>();
            m_canvasGroup = GetComponent<CanvasGroup>();
            if (string.IsNullOrEmpty(m_strTipText) == false)
            {
                m_tipData = new TipData
                {
                    ETipType = EnumTipType.eTypeTileContent,
                    TipContent = m_strTipText
                };
            }
        }

        protected virtual void OnAwake()
        {

        }

        protected virtual void OnStart()
        {

        }

        void Awake()
        {
            OnAwake();
        }

        void Start()
        {
            if (false == m_bInited)
            {
                Init();
            }
            OnStart();
        }

        void OnEnable()
        {

        }

        void OnDisable()
        {
            //m_bIsMouseIn = false;
            //OnMouseLeave();
        }

        private static Material GetGrayMat()
        {
            return ResourceMgr2.Singleton.MatGray;
        }

        public string m_strTipText = "";

        private IXUIObject m_parent = null;
        private object m_dlgBehaviour = null;
        private bool m_bInited = false;
        private GameObject m_Go;
        private Transform m_Trans;
        private RectTransform m_RectTrans;
        private bool m_bIsMouseIn = false;
        protected Bounds m_AbsoluteBounds = default(Bounds);
        protected bool m_bSizeChanged = true;
        protected float m_fAlpha = 1.0f;

        protected UIEvent m_eventHandlerOnClick = null;
        private UIEvent m_eventHandlerOnDown = null;
        private UIEvent m_eventHandlerOnUp = null;
        private UIEvent m_eventHandlerOnEnter = null;
        private UIEvent m_eventHandlerOnExit = null;

        //双击
        private UIEvent m_eventDoubleClick = null;

        //长按
        private UIEvent m_eventPress = null;

        //tips
        private UIEvent m_eventTips = null;
        private UIEvent m_eventHideTips = null;


        /// <summary>
        /// 拖拽事件
        /// </summary>
        private class CDrag
        {
            public UIEvent m_eventHandlerOnBeginDrag = null;
            public UIEvent m_eventHandlerOnDrag = null;
            public UIEvent m_eventHandlerOnEndDrag = null;
            public UIEvent m_eventHandlerOnDrop = null;
            public bool m_bAllowDrag = true;      //是否允许拖拽
            public bool m_bAllowDrop = true;      //是否允许drop
            public bool m_bBeginDragOnDown = false;//是否在按下以后就触发BeginDrag事件
            public bool m_bStartDrag = false; //是否已经开始拖拽
            public Rect m_ConstraintRect; //约束拖动范围，四个参数分别为 xMin, xMax , yMin , yMax
            public Vector3 m_vLastPos = Vector3.zero;
            public Vector3 m_vOffset = Vector3.zero;
            public float m_fSensitivity = 1.0f; //灵敏度
            public CDrag(){
                m_ConstraintRect.Set(float.NaN, float.NaN, float.NaN, float.NaN);
            }
        }
        private CDrag m_drag = null;
        /// 

        private Button m_button = null;
        private CanvasGroup m_canvasGroup = null;
        private bool m_bEnable = true;
        private float m_nClickCd = 0.0f;
        private float m_nLastClickTime = 0;
        private TipData m_tipData = null;
        private EventTriggerListener m_eventTriggerListener = null;
        private DragEventTriggerListener m_dragEventTriggerListener = null;
    }
}
