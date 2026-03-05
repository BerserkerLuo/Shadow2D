using Client.UI.UICommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

namespace UILib
{
    public class XUILoopList : XUIObject, IXUILoopList
    {
        public GameObject m_prefabListItem = null;
        public Transform m_itemsRoot = null;
        public bool m_IsLoop = false;

        public int TotalCount
        {
            get { return m_nDataCount; }
        }

        public int StartIndex { get { return m_nStartIndex; } }
        public int EndIndex
        {
            get
            {
                int nEndIndex = m_nStartIndex + m_nCreateCount - 1;
                nEndIndex = Math.Min(nEndIndex, m_nDataCount - 1);
                return nEndIndex;
            }
        }

        public Transform ItemsRoot { get { return m_itemsRoot; } }

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
            set
            {
                if (null != m_scrollRect)
                {
                    m_scrollRect.normalizedPosition = value;
                }
            }
        }

        public void RegisterScrollValueChange(UIEvent uiEvent)
        {
            m_eventScrollValueChange = uiEvent;
        }

        public void RegisterScrollDragEnd(UIEvent uiEvent)
        {
            m_eventScrollDragEnd = uiEvent;
        }

        public override void Init()
        {
            base.Init();
            CheckIncompatibleComponents();
            if (m_itemsRoot == null)
            {
                m_itemsRoot = CachedTransform;
            }

            for (int i = 0; i < m_itemsRoot.childCount; ++i)
            {
                Transform trans = m_itemsRoot.GetChild(i);
                XUIListItem uiListItem = trans.GetComponent<XUIListItem>();
                uiListItem.Init();
                ReleaseItem(uiListItem);
            }
        }

        //检测不兼容的组件
        private void CheckIncompatibleComponents()
        {
            if (CachedTransform.GetComponent<ContentSizeFitter>())
                Debug.LogWarning("<ContentSizeFitter>组件可能导致循环使用的子节点坐标错误!!!");

            if (CachedTransform.GetComponent<HorizontalLayoutGroup>())
                Debug.LogWarning("<HorizontalLayoutGroup>组件可能导致循环使用的子节点坐标错误!!!");

            if (CachedTransform.GetComponent<VerticalLayoutGroup>())
                Debug.LogWarning("<VerticalLayoutGroup>组件可能导致循环使用的子节点坐标错误!!!");
        }

        public void Init(int dataCount, UIEvent updateCell, float cellPadding = 0f)
        {
            CheckIncompatibleComponents();
            if (m_IsLoop && dataCount > 1)
            {
                dataCount = dataCount * m_ExtendMultiple;
            }
           
            //数据和组件初始化
            this.m_nDataCount = dataCount;
            this.m_updateCellCB = updateCell;
            this.m_fCellPadding = cellPadding;

            m_scrollRect =  transform.GetComponentInParent<ScrollRect>();
            if (null == m_scrollRect)
            {
                Debug.LogError("null == m_scrollRect");
                return;
            }
            RectTransform rectTrans = m_scrollRect.GetComponent<RectTransform>();
            m_scrollRectSize = rectTrans.rect.size;
            m_cellSize = m_prefabListItem.GetComponent<RectTransform>().rect.size;
            m_nStartIndex = 0;
            m_nMaxCount = GetMaxCount();
            m_nCreateCount = 0;

            if (m_scrollRect.horizontal)
            {
                CachedRectTransform.anchorMin = new Vector2(0, 0);
                CachedRectTransform.anchorMax = new Vector2(0, 1);
            }
            else
            {
                CachedRectTransform.anchorMin = new Vector2(0, 1);
                CachedRectTransform.anchorMax = new Vector2(1, 1);
            }
            m_scrollRect.onValueChanged.RemoveAllListeners();
            m_scrollRect.onValueChanged.AddListener(OnValueChanged);
            DragEventTriggerListener.Get(gameObject).dragEndCallBack = OnDragEnd;
            DragEventTriggerListener.Get(gameObject).dragStartCallBack = OnDragStart;
            DragEventTriggerListener.Get(gameObject).dragCallBack = OnDrag;
            ResetSize(dataCount);
            Debug.Log("m_nStartIndex:" + m_nStartIndex);
            Vector3 pos = GetPosition(m_nStartIndex);
            CachedRectTransform.anchoredPosition = -pos;
            UpdateList();
        }


        public void Reposition(int nIndex=0)
        {
            if (null == m_updateCellCB)
            {
                Debug.LogError("null == m_updateCellCB");
                return;
            }
            //刷新数据
            int nShowMaxCount = m_nMaxCount - m_nCacheCount;
            if (m_nDataCount - nIndex < nShowMaxCount)
            {
                nIndex = m_nDataCount - nShowMaxCount;
                nIndex = Math.Max(nIndex,0);
            }
            Vector3 pos = GetPosition(nIndex);
            m_nStartIndex = nIndex;
            CachedRectTransform.anchoredPosition = -pos;
            ReArrange();

        }

        //重置数量
        public void ResetSize(int dataCount)
        {
            if (null == m_updateCellCB)
            {
                Debug.LogError("null == m_updateCellCB");
                return;
            }
            this.m_nDataCount = dataCount;
            CachedRectTransform.sizeDelta = GetContentSize();
            //创建或显示需要的go
            m_nCreateCount = Mathf.Min(m_nDataCount, m_nMaxCount);
            ReArrange();
        }

        public void AddListItem(int nCount = 1)
        {
            if (null == m_updateCellCB)
            {
                Debug.LogError("null == m_updateCellCB");
                return;
            }

            m_nDataCount += nCount;
            CachedRectTransform.sizeDelta = GetContentSize();
            //创建或显示需要的go
            m_nCreateCount = Mathf.Min(m_nDataCount, m_nMaxCount);
            ReArrange();

        }

        public void MoveToIndex(int index)
        {
            if (null == m_updateCellCB)
            {
                Debug.LogError("null == m_updateCellCB");
                return;
            }
            if (m_scrollRect != null && m_itemsRoot != null)
            {
                int count = index + 1;
                if(count <= m_nDataCount)
                {
                    m_scrollRect.velocity = Vector2.zero;
                    float localY = m_cellSize.y * count + m_fCellPadding * (count - 1);
                    Vector2 contentSize = (m_itemsRoot as RectTransform).sizeDelta;
                    Vector2 rectSize = m_scrollRect.GetComponent<RectTransform>().rect.size;
                    float normalizedPosY = Math.Max(0.0f, ((contentSize.y - rectSize.y) - Math.Abs(localY))) / (contentSize.y - rectSize.y);
                    normalizedPosY = Math.Min(1, normalizedPosY);
                    float normalizedSizeY = contentSize.y - rectSize.y;
                    float contentY = normalizedSizeY * (1 - normalizedPosY);
                    m_itemsRoot.transform.DOLocalMoveY(contentY, 0.4f);
                }
            }
        }

        //更新当前显示的列表
        public void UpdateList(int index = -1)
        {
            if (null == m_updateCellCB)
            {
                Debug.LogError("null == m_updateCellCB");
                return;
            }
            if (m_dicListItem.ContainsKey(index))
            {
                m_updateCellCB(m_dicListItem[index]);
            }
            else
            {
                Dictionary<int, XUIListItem>.Enumerator iter = m_dicListItem.GetEnumerator();
                while (iter.MoveNext())
                {
                    XUIListItem go = iter.Current.Value;
                    m_updateCellCB(go);
                }
            }
        }

        public void StopMovement()
        {
            m_scrollRect.StopMovement();
        }

        //创建或显示一个item
        private void CreateItem(int index)
        {
            XUIListItem go;
            if (m_freeGoQueue.Count > 0)//使用原来的
            {
                go = m_freeGoQueue.Dequeue();
                go.SetVisible(true);
            }
            else//创建新的
            {
                GameObject goObj = Instantiate<GameObject>(m_prefabListItem);
                go = goObj.GetComponent<XUIListItem>();
                go.parent = this;
                go.DlgBehaviour = this.DlgBehaviour;
                go.Init();
                go.transform.SetParent(m_itemsRoot);
                go.transform.localScale = Vector3.one;
                go.transform.localRotation = Quaternion.identity;
                RectTransform rect = go.GetComponent<RectTransform>();
                rect.pivot = new Vector2(0, 1);
                rect.anchorMin = new Vector2(0, 1);
                rect.anchorMax = new Vector2(0, 1);
            }
            //m_goList.Add(go);
            go.transform.localPosition = GetPosition(index);
            go.Index = index;
            m_dicListItem[index] = go;
            m_updateCellCB(go);
        }

        //回收一个item
        private void ReleaseItem(XUIListItem go)
        {
            go.SetVisible(false);
            //m_goList.Remove(go);
            m_freeGoQueue.Enqueue(go);
            //m_goIndexDic[go] = m_nInvalidStartIndex;
            int nIndex = go.Index;
            go.Index = m_nInvalidStartIndex;
            m_dicListItem.Remove(nIndex);
        }

        //滑动回调
        private static List<int> s_listNeedDelIndex = new List<int>();
        private void OnValueChanged(Vector2 vec)
        {
            int curStartIndex = GetStartIndex();
            //Debug.LogWarning(curStartIndex);

            if ((m_nStartIndex != curStartIndex))
            {
                m_nStartIndex = Math.Max(0, curStartIndex);


                //收集被移出去的go
                //索引的范围:[startIndex, startIndex + createCount - 1]
                ReArrange();
            }
        }

        private void OnDragStart(GameObject obj, PointerEventData evt)
        {
            if(m_scrollRect != null)
            {
                m_scrollRect.OnBeginDrag(evt);
            }
        }

        private void OnDrag(GameObject obj, PointerEventData evt)
        {
            if (m_scrollRect != null)
            {
                m_scrollRect.OnDrag(evt);
            }

            if (m_eventScrollValueChange != null)
                m_eventScrollValueChange(this);
        }

        private void OnDragEnd(GameObject obj, PointerEventData evt)
        {
            if (m_scrollRect != null)
            {
                m_scrollRect.OnEndDrag(evt);
            }

            if (m_eventScrollDragEnd != null)
            {
                //Debug.Log("Unity。。。OnScrollEnd");
                m_eventScrollDragEnd(this);
            }
              
        }

        private void ReArrange()
        {
            s_listNeedDelIndex.Clear();
            Dictionary<int, XUIListItem>.Enumerator iter = m_dicListItem.GetEnumerator();
            while (iter.MoveNext())
            {
                int nIndex = iter.Current.Value.Index;
                if (nIndex < m_nStartIndex || nIndex > (m_nStartIndex + m_nCreateCount - 1))
                {
                    s_listNeedDelIndex.Add(nIndex);
                }
            }

            for (int i = 0; i < s_listNeedDelIndex.Count; ++i)
            {
                int nIndex = s_listNeedDelIndex[i];
                XUIListItem go = m_dicListItem[nIndex];
                ReleaseItem(go);
            }
            s_listNeedDelIndex.Clear();

            //对移除出的go进行重新排列
            for (int i = m_nStartIndex; i < m_nStartIndex + m_nCreateCount; i++)
            {
                if (i >= m_nDataCount)
                {
                    break;
                }

                if (m_dicListItem.ContainsKey(i) == false)
                {
                    CreateItem(i);
                }
            }
        }

        //获取需要创建的最大prefab数目
        private int GetMaxCount()
        {
            if (m_scrollRect.horizontal)
            {
                return Mathf.CeilToInt(m_scrollRectSize.x / (m_cellSize.x + m_fCellPadding)) + m_nCacheCount;
            }
            else
            {
                return Mathf.CeilToInt(m_scrollRectSize.y / (m_cellSize.y + m_fCellPadding)) + m_nCacheCount;
            }
        }

        //获取起始索引
        private int GetStartIndex()
        {
            if (m_scrollRect.horizontal)
            {
                return Mathf.FloorToInt(-CachedRectTransform.anchoredPosition.x / (m_cellSize.x + m_fCellPadding));
            }
            else
            {
                return Mathf.FloorToInt(CachedRectTransform.anchoredPosition.y / (m_cellSize.y + m_fCellPadding));
            }
        }

        //获取索引所在位置
        private Vector3 GetPosition(int index)
        {
            Vector3 pos = Vector3.zero;
            if (m_scrollRect.horizontal)
            {
                pos = new Vector3(index * (m_cellSize.x + m_fCellPadding), 0, 0);
                return pos;
            }
            else
            {
                pos = new Vector3(0, index * -(m_cellSize.y + m_fCellPadding), 0);
                return pos;
            }
        }

        //获取内容长宽
        private Vector2 GetContentSize()
        {
            Vector2 size = Vector2.zero;
            bool bHorizontal = false;
            if (null != m_scrollRect)
            {
                bHorizontal = m_scrollRect.horizontal;
            }

            if (bHorizontal)
            {
                size = new Vector2(m_cellSize.x * m_nDataCount + m_fCellPadding * (m_nDataCount - 1), CachedRectTransform.sizeDelta.y);
                return size;
            }
            else
            {
                size = new Vector2(CachedRectTransform.sizeDelta.x, m_cellSize.y * m_nDataCount + m_fCellPadding * (m_nDataCount - 1));
                return size;
            }
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

        private UIEvent m_eventScrollValueChange = null;

        private UIEvent m_eventScrollDragEnd = null;

        private ScrollRect m_scrollRect = null;

        private Dictionary<int, XUIListItem> m_dicListItem = new Dictionary<int, XUIListItem>();
        private Queue<XUIListItem> m_freeGoQueue = new Queue<XUIListItem>();

        private Vector2 m_scrollRectSize;
        private Vector2 m_cellSize;
        public int m_nStartIndex;//起始索引

        private int m_nMaxCount;//创建的最大数量
        public int m_nCreateCount;//当前显示的数量

        private const int m_nCacheCount = 2;//缓存数目
        private const int m_nInvalidStartIndex = -1;//非法的起始索引

        public int m_nDataCount;
        private UIEvent m_updateCellCB;
        private float m_fCellPadding;
        private int m_ExtendMultiple = 100; //无限循环扩展倍数
    }
}
