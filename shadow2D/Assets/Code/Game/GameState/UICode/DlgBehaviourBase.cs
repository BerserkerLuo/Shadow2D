using System;
using System.Collections.Generic;

using System.Text;
using UnityEngine;
using UILib.Export;
using UILib;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.UI;

namespace Client.UI.UICommon
{
    public class DlgBehaviourBase
    {
        public bool IsError
        {
            get { return false; }
        }

        public bool IsOnCanvasRoot2
        {
            get { return m_bOnCanvasRoot2; }
            set { m_bOnCanvasRoot2 = value; }
        }

        public Canvas canvas
        {
            get { return m_canvas; }
        }

        public virtual string AbName { get { return ""; } }

        public virtual string FileName { get { return ""; } }

        public IXUIObject parent
        {
            get { return null; }
            set { }
        }

        public IXUIDlg Dlg
        {
            get { return m_uiDlgInterface; }
            set { m_uiDlgInterface = value; }
        }

        public IXUIObject[] uiChilds
        {
            get { return m_uiChilds; }
        }

        public Vector2 RealSize
        {
            get
            {
                Vector2 vector = Vector2.zero;
                //vector.x = transform.lossyScale.x;
                //vector.y = transform.lossyScale.y;
                return vector;
            }
        }

        public Vector2 RelativeSize
        {
            get
            {
                Vector2 vector = Vector2.zero;
                //vector.x = transform.localScale.x;
                //vector.y = transform.localScale.y;
                return vector;
            }
        }

        public PointerEventData EventData { get; set; }

        public virtual Bounds AbsoluteBounds { get { return new Bounds(Vector3.zero, Vector3.one); } }
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

        public GameObject CachedGameObject { get { return m_Go; } }

        public Transform CachedTransform { get { return m_Trans; } }

        public RectTransform CachedRectTransform { get { if (m_RectTrans == null) m_RectTrans = m_Go.GetComponent<RectTransform>(); return m_RectTrans; } }
        public CanvasGroup CanvasGroup { get { return m_canvasGroup; } }
        public string Tip { get; set; }

        public float Alpha { get; set; }

        public bool IsEnableOpen { get; set; }

        //是否启用引导限制
        public bool EnableFilter 
        { 
            get { return m_bEnableFilter; }
            set { m_bEnableFilter = value; } 
        }

        public void EnableRaycaster(bool bEnable)
        {
            if (null != m_baseRaycast)
            {
                m_baseRaycast.enabled = bEnable;
            }
        }

        public bool IsVisible()
        {
            //return gameObject.activeInHierarchy;// Unity3.5 not Unity4.0
            return true;
        }

        public bool IsMouseIn()
        {
            List<string> keyList = m_dicId2UIObject.Keys.ToList();
            int count = keyList.Count;
            for (int i = 0; i < count; ++i)
            {
                string key = keyList[i];
                XUIObjectBase uiObject = m_dicId2UIObject[key];
                if ((uiObject != null) && uiObject.IsMouseIn())
                {
                    return true;
                }
            }
            return false;
        }

        public void SetVisible(bool bVisible, bool bReal)
        {
            if (null != m_baseRaycast)
            {
                m_baseRaycast.enabled = bVisible;
            }

            if (null != m_canvas)
            {
                m_canvas.enabled = bVisible;
            }

            if (true == bVisible)
            {
                m_RectTrans.localPosition = Vector3.zero;
                //m_RectTrans.SetAsLastSibling();
            }
            else
            {
                if (bReal == false)
                {
                    m_RectTrans.localPosition = Vector3.one * 100000;
                }
                else
                {
                    if (m_Go.activeSelf == true)
                    {
                        m_Go.SetActive(false);
                    }
                }
            }

            if (m_Go.activeSelf == false && bVisible == true)
            {
                m_Go.SetActive(true);
            }
        }

        public void SetSiblingIndex(int index)
        {
            index = CachedTransform.parent.childCount - index - 1;
            (CachedTransform as RectTransform).SetSiblingIndex(index);
        }

        public void SetAsFirstSibling()
        {
            (CachedTransform as RectTransform).SetAsFirstSibling();
        }

        public void SetAsLastSibling()
        {
            (CachedTransform as RectTransform).SetAsLastSibling();
        }

        public void OnPress()
        {
            OnFocus();
        }

        public void OnFocus()
        {
            //UIManager.Singleton.Compositor(m_uiDlgInterface);
        }

        public void Init(GameObject objUI, int nlayer)
        {
            m_Go = objUI;
            m_baseRaycast = m_Go.GetComponent<BaseRaycaster>();
            m_Trans = objUI.transform;
            m_RectTrans = objUI.GetComponent<RectTransform>();

            m_canvas = objUI.GetComponent<Canvas>();
            Transform parent = null;
            if (UnityGameEntry.Instance != null)
            {
                //if(objUI.layer==LayerMask.NameToLayer("BackUI"))
                //    parent = UnityGameEntry.Instance.UIRoot_Back;
                //else
                    parent = UnityGameEntry.Instance.UIRoot;
            }
                

            m_RectTrans.SetParent(parent);

            m_RectTrans.anchorMin = Vector2.zero;
            m_RectTrans.anchorMax = Vector2.one;

            m_RectTrans.offsetMin = Vector2.zero;
            m_RectTrans.offsetMax = Vector2.zero;

            objUI.transform.localPosition = new Vector3(0, 0, 0);
            objUI.transform.localScale = new Vector3(1, 1, 1);

            if (nlayer != 0)
            {
                m_canvas.overrideSorting = true;
                m_canvas.sortingOrder = nlayer;
            }

            m_canvasGroup = objUI.GetComponent<CanvasGroup>();
            Init();
        }

        public virtual void Init()
        {
            WidgetFactory.FindAllUIObjects(m_Trans, null, ref m_dicId2UIObject, this);
        }

        public void _Update()
        {

        }


        public virtual void Highlight(bool bTrue)
        {

        }

        public IXUIObject GetUIObject(string strName)
        {
            if (null == strName)
            {
                return null;
            }
            string strId = strName;
            int nIndex = strName.LastIndexOf('/');
            if (nIndex >= 0)
            {
                strId = strName.Substring(nIndex+1);
            }
            XUIObjectBase uiObject = null;
            if (true == m_dicId2UIObject.TryGetValue(strId, out uiObject))
            {
                return uiObject;
            }
            return null;
        }

        public void Destroy()
        {
            //SafeXUIObject.OnDestoryXUIObject(this);
            List<string> keyList = m_dicId2UIObject.Keys.ToList();
            int count = keyList.Count;
            for (int i = 0; i < count; ++i)
            {
                string key = keyList[i];
                XUIObjectBase uiObject = m_dicId2UIObject[key];
                SafeXUIObject.OnDestoryXUIObject(uiObject);
            }

            m_dicId2UIObject.Clear();
            m_Go = null;
            m_Trans = null;
            m_RectTrans = null;
            m_baseRaycast = null;
            m_canvas = null;
        }

        private void Awake()
        {

        }

        private void Start()
        {
            //m_uiDlgInterface._Init(this);
        }

        public GraphicRaycaster GraphicRaycaster {
            get {
                if (m_baseRaycast is GraphicRaycaster)
                    return (GraphicRaycaster)m_baseRaycast;
                return null;
            }
        }

        public List<GameObject> GetUIUnderPos(Vector2 pos) {
            if (GraphicRaycaster == null)
                return new List<GameObject>();

            PointerEventData pointerData = new PointerEventData(UnityGameEntry.Instance.eventSystem){
                position = pos
            };

            List<RaycastResult> results = new List<RaycastResult>();
            UnityGameEntry.Instance.Raycaster.Raycast(pointerData, results);

            List<GameObject> retList = new List<GameObject>();
            for (int i = 0; i < results.Count; ++i)
                retList.Add(results[i].gameObject);

            return retList;
        }


        private IXUIDlg m_uiDlgInterface = null;
        private IXUIObject[] m_uiChilds = null;
        private GameObject m_Go = null;
        private Transform m_Trans = null;
        private RectTransform m_RectTrans = null;
        private BaseRaycaster m_baseRaycast = null;
        private Canvas m_canvas = null;
        private Dictionary<string, XUIObjectBase> m_dicId2UIObject = new Dictionary<string, XUIObjectBase>();
        private CanvasGroup m_canvasGroup = null;
        private bool m_bOnCanvasRoot2 = false;
        private bool m_bEnableFilter = true;
    }
}
