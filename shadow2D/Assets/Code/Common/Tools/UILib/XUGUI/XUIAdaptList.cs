using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using Client.UI.UICommon;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UILib
{
    public class XUIAdaptList: XUIObject, IXUIAdaptList
    {
        protected LoopScrollRect m_scrollRect;
        public int TotalCount { get { return m_scrollRect.totalCount; } }
        public int StartIndex { get { return m_scrollRect.itemTypeStart; } }
        public int EndIndex { get { return m_scrollRect.itemTypeEnd; } }
        public bool IsDragging { get { return m_scrollRect.m_Dragging; } }

        public GameObject prefabObj;
        public int poolSize = 5;

        public XUIListItem GetObject()
        {
            GameObject goObj = null;
            XUIListItem listItem = null;
            if (!inited)
            {
                //SG.ResourceManager.Instance.InitPool(prefabName, poolSize);
                for (int i = 0; i < poolSize; ++i)
                {
                    goObj = GameObject.Instantiate<GameObject>(prefabObj);
                    GameObject.DontDestroyOnLoad(goObj);
                    listItem = goObj.GetComponent<XUIListItem>();
                    if (null == listItem)
                    {
                        Debug.LogError("null == listItem");
                    }
                    listItem.Init();
                    //go.parent = this;
                    //go.Init();
                    //go.transform.SetParent(m_itemsRoot);
                    //go.transform.localScale = Vector3.one;
                    //go.transform.localRotation = Quaternion.identity;
                    //RectTransform rect = go.GetComponent<RectTransform>();
                    //rect.pivot = new Vector2(0, 1);
                    //rect.anchorMin = new Vector2(0, 1);
                    //rect.anchorMax = new Vector2(0, 1);
                    m_stack.Push(listItem);
                }
                inited = true;
            }

            if (m_stack.Count > 0)
            {
                return m_stack.Pop();
            }
            goObj = GameObject.Instantiate<GameObject>(prefabObj);
            GameObject.DontDestroyOnLoad(goObj);
            listItem = goObj.GetComponent<XUIListItem>();
            listItem.Init();
            return listItem;
        }

        public void ReturnObject(XUIListItem listItem)
        {
            //go.SendMessage("ScrollCellReturn", SendMessageOptions.DontRequireReceiver);
            //SG.ResourceManager.Instance.ReturnObjectToPool(go.gameObject);
            if (null == listItem)
            {
                Debug.LogError("null == listItem");
            }
            listItem.SetVisible(false);
            listItem.CachedTransform.SetParent(m_scrollRect.transform);
            m_stack.Push(listItem);
        }

        private void Awake()
        {
            m_scrollRect = GetComponentInParent<LoopScrollRect>();
            if (m_scrollRect == null)
            {
                Debug.LogError("m_scrollRect == null");
            }
        }

        public void Init(int dataCount, UIEvent updateCellCB,int nOffset = 0, bool bFromEnd=false)
        {
            Debug.Log(string.Format("XUIAdaptList.Init:dataCount={0}, nDataOffset={1}, bViewFromEnd={2}", dataCount, nOffset,bFromEnd));
            if(m_scrollRect == null)
            {
                LoopScrollRect[] loopScrollRects = GetComponentsInParent<LoopScrollRect>(true);
                m_scrollRect = loopScrollRects.Length > 0 ? loopScrollRects[0] : null;
            }
            if (null != m_scrollRect)
            {
                //bool init = (m_scrollRect.totalCount == 0 && dataCount > 0);
                m_scrollRect.totalCount = dataCount;
                m_scrollRect.OnUpdateItem = updateCellCB;
                m_scrollRect.content = this.CachedRectTransform;
                m_scrollRect.UpdateBounds();

                if (false == bFromEnd)
                {
                    m_scrollRect.RefillCells(nOffset);
                }
                else
                {
                    m_scrollRect.RefillCellsFromEnd(nOffset);
                }
                RectTransform rectTrans = m_scrollRect.GetComponent<RectTransform>();
                m_scrollRectSize = rectTrans.rect.size;
                //DragEventTriggerListener.Get(gameObject).dragEndCallBack = OnDragEnd;
            }
            else
            {
                Debug.LogError(null == m_scrollRect);
            }
        }
        public void AddListItem(int nCount = 1, bool bAddToEnd = true)
        {
            if(m_scrollRect != null && nCount > 0)
            {
                m_scrollRect.totalCount += nCount;
                if (m_scrollRect.m_Dragging)
                {
                    m_scrollRect.UpdateBounds(true);
                }
                else
                {
                    m_scrollRect.RefillCellsFromEnd();
                }
            }
        }

        private void Update()
        {
            //Debug.Log(m_scrollRect.normalizedPosition);
        }

        public void MoveToLast()
        {
            if (null != m_scrollRect)
            {
                Vector2 pos = m_scrollRect.normalizedPosition;
                pos.y = 1f;
                m_scrollRect.normalizedPosition = pos;
            }
        }

        public Vector2 NormalizedPosition
        {
            get
            {
                if (null != m_scrollRect)
                {
                    return m_scrollRect.normalizedPosition;
                }
                return Vector2.zero;
            }
        }    

        public void RegisterScrollValueChange(UIEvent uiEvent)
        {
            m_eventScrollValueChange = uiEvent;
            m_scrollRect.onValueChanged.RemoveAllListeners();
            m_scrollRect.onValueChanged.AddListener(OnValueChange);
        }

        public void RegisterScrollDragEnd(UIEvent uiEvent)
        {
            if(m_scrollRect != null)
            {
                m_scrollRect.OnDragEnd = uiEvent;
            }
            //m_eventScrollDragEnd = uiEvent;
        }

        /// <summary>
        /// 获取content拖拽到最下面,离下视口边界的像素值
        /// </summary>
        /// <returns></returns>
        public float GetDragBottomPixel()
        {
            if (m_scrollRect.horizontal)
            {
                return Math.Max(0, Math.Abs((CachedRectTransform.anchoredPosition.x) - (CachedRectTransform.rect.size.x - m_scrollRectSize.x)));
            }
            else
            {
                return Math.Max(0, CachedRectTransform.anchoredPosition.y - (CachedRectTransform.rect.size.y - m_scrollRectSize.y));
            }
        }

        public float GetDragTopPixel()
        {
            if (m_scrollRect.horizontal)
            {
                if (CachedRectTransform.anchoredPosition.x < 0)
                    return 0;
                return Math.Abs(CachedRectTransform.anchoredPosition.x);
            }
            else
            {
                if (CachedRectTransform.anchoredPosition.y > 0)
                    return 0;
                return Math.Abs(CachedRectTransform.anchoredPosition.y);
            }
        }

        private void OnValueChange(Vector2 value)
        {
            if(m_eventScrollValueChange != null)
            {
                m_eventScrollValueChange(this);
            }
        }

        //private void OnDragEnd(GameObject obj, PointerEventData evt)
        //{
        //    if (m_scrollRect != null)
        //    {
        //        m_scrollRect.OnEndDrag(evt);
        //    }

        //    if (m_eventScrollDragEnd != null)
        //    {
        //        Debug.Log("Unity。。。OnScrollEnd");
        //        m_eventScrollDragEnd(this);
        //    }

        //}


        private UIEvent m_eventScrollValueChange = null;
        //private UIEvent m_eventScrollDragEnd = null;
        private Vector2 m_scrollRectSize;
        private Stack<XUIListItem> m_stack = new Stack<XUIListItem>();
        private bool inited = false;
    }
}
