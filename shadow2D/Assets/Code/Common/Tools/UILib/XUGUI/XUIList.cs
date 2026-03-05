using UnityEngine;
using UnityEngine.UI;
using Client.UI.UICommon;
using System;
using System.Collections.Generic;
namespace UILib
{
    public enum EnumMoveCenterType
    {
        eMoveCenterType_None,
        eMoveCenterType_Increas,// 递增
    }

    public class XUIList : XUIObject, IXUIList
    {
        public GameObject m_prefabListItem = null;
        public Transform m_itemsRoot = null;
        public bool m_bMultiSelect = false;
        public bool m_bWidthCenter = false;
        public bool m_bHideSrc = true;
        public EnumMoveCenterType m_eMoveCenterType = EnumMoveCenterType.eMoveCenterType_None;


        public List<XUIListItem> ListItems
        {
            get { return m_listXUIListItem; }
        }

        public Int32 Count
        {
            get
            {
                if (null == m_listXUIListItem)
                {
                    return 0;
                }
                return m_listXUIListItem.Count;
            }
        }

        public Transform ItemsRoot
        {
            get
            {
                return m_itemsRoot;
            }
        }

        public RectTransform ItemsRootRect
        {
            get
            {
                RectTransform result = null;
                if(m_ItemsRootRect == null)
                {
                    result = ItemsRoot.GetComponent<RectTransform>();
                }
                return result;
            }
        }

        public void Reposition()
        {
            if (null != m_scrollRect.content)
            {
                m_scrollRect.content.anchoredPosition3D = Vector2.zero;       
            }
        }

        public Vector2 GetNormalizedPosition()
        {
             if (null != m_scrollRect.content)
            {
                return m_scrollRect.normalizedPosition;
            }
             return Vector2.zero;
        }

        public  bool SetVerticalNormalizedPosition(float value)
        {
            if (null != m_scrollRect.content)
            {
                m_scrollRect.verticalNormalizedPosition = value;
                return true;
            }
            return false;
        }

        public void SetVerticalLastPostion()
        {
            float y = Mathf.Max(0, ItemsRootRect.sizeDelta.y - this.CachedRectTransform.sizeDelta.y);
            if(y != ItemsRootRect.anchoredPosition.y)
            {
                ItemsRootRect.anchoredPosition = new Vector2(ItemsRootRect.anchoredPosition.x, y);
            }
        }

        public void Refresh()
        {
        }

        public void SetSize(float cellWidth, float cellHeight)
        {
            //if (null == m_uiGrid)
            //    return;
            //m_uiGrid.cellWidth = cellWidth;
            //m_uiGrid.cellHeight = cellHeight;
            //m_uiGrid.Reposition();
        }

        public IXUIListItem GetItemByIndex(Int32 nIndex)
        {
            if (null == m_listXUIListItem)
            {
                return null;
            }

            if (0 > nIndex || nIndex >= m_listXUIListItem.Count)
            {
                return null;
            }


            return m_listXUIListItem[nIndex];
        }

        public IXUIListItem GetItemById(UInt32 unId)
        {
            foreach (XUIListItem item in m_listXUIListItem)
            {
                if (unId == item.id)
                {
                    return item;
                }
            }
            return null;
        }

        public IXUIListItem GetItemById(UInt32 unId, bool bVisible)
        {
            foreach (XUIListItem item in m_listXUIListItem)
            {
                if (unId == item.id && item.IsVisible() == bVisible)
                {
                    return item;
                }
            }
            return null;
        }

        public IXUIListItem GetItemByGUID(ulong ulId)
        {
            foreach (XUIListItem item in m_listXUIListItem)
            {
                if (ulId == item.GUID)
                {
                    return item;
                }
            }
            return null;
        }
        
        public IXUIListItem GetItemByIndexOrAdd(Int32 nIndex)
        {
            IXUIListItem result = GetItemByIndex(nIndex);
            if(result == null)
            {
                result = AddListItem();
            }
            return result;
        }

        public IXUIListItem[] GetAllItems()
        {
            return m_listXUIListItem.ToArray();
        }

        public IXUIListItem AddListItem(GameObject obj)
        {
            GameObject prefabListItem = obj;
            if (null == prefabListItem)
            {
                return null;
            }

            GameObject listItemObjNew = Instantiate(prefabListItem) as GameObject;
            listItemObjNew.name = string.Format("{0:0000}", Count);
            listItemObjNew.transform.SetParent(m_itemsRoot);
            listItemObjNew.transform.localPosition = Vector3.zero;
            listItemObjNew.transform.localScale = Vector3.one;
            listItemObjNew.transform.localRotation = Quaternion.identity;
            XUITool.SetLayer(listItemObjNew, CachedGameObject.layer);

            Toggle uiCheckbox = listItemObjNew.GetComponent<Toggle>();
            if (null != uiCheckbox && false == m_bMultiSelect)
            {
                uiCheckbox.group = uiCheckbox.transform.parent.GetComponent<ToggleGroup>();
            }

            XUIListItem uiListItem = listItemObjNew.GetComponent<XUIListItem>();
            if (null == uiListItem)
            {
                Debug.LogError("null == uiListItem");
                uiListItem = listItemObjNew.AddComponent<XUIListItem>();
            }
            uiListItem.Index = Count;
            uiListItem.parent = this;
            uiListItem.DlgBehaviour = this.DlgBehaviour;
            uiListItem.SetVisible(true);
            if (uiListItem.IsInited == false)
            {
                uiListItem.Init();
            }
            m_listXUIListItem.Add(uiListItem);
            
            Refresh();
            return (IXUIListItem)uiListItem;
        }

        public IXUIListItem AddListItem()
        {
            if (null != m_prefabListItem)
            {
                return AddListItem(m_prefabListItem);
            }
            return null;
        }

        public bool DelItemById(UInt32 unId)
        {
            IXUIListItem uiListItem = GetItemById(unId);

            return DelItem(uiListItem);
        }

        public bool DelItemByIndex(Int32 nIndex)
        {
            IXUIListItem uiListItem = GetItemByIndex(nIndex);


            return DelItem(uiListItem);
        }

        public bool DelItem(IXUIListItem iUIListItem)
        {
            XUIListItem uiListItem = iUIListItem as XUIListItem;
            if (null == uiListItem)
            {
                return false;
            }

            Int32 nIndexDeleted = uiListItem.Index;
            for (Int32 nIndex = nIndexDeleted + 1; nIndex < Count; nIndex++)
            {
                m_listXUIListItem[nIndex].name = string.Format("{0:0000}", nIndex - 1);
                m_listXUIListItem[nIndex].Index = nIndex - 1;
            }
            m_listXUIListItem.Remove(uiListItem);
            uiListItem.gameObject.transform.parent = null;
            GameObject.Destroy(uiListItem.gameObject);
            Refresh();
            return true;
        }

        public void Clear()
        {
            if (null == m_listXUIListItem)
            {
                return;
            }
            foreach (XUIListItem uiListItem in m_listXUIListItem)
            {
                if (m_prefabListItem != uiListItem.CachedGameObject)
                {
                    uiListItem.gameObject.transform.SetParent(null);
                    GameObject.Destroy(uiListItem.gameObject);
                }
                else
                {
                    uiListItem.SetVisible(false);
                }
            }
            m_listXUIListItem.Clear();
            Refresh();
        }

        public override void SetEnable(bool bEnable)
        {
            foreach (XUIListItem uiListItem in m_listXUIListItem)
            {
                uiListItem.SetEnable(bEnable);
            }
        }

        public override void Highlight(bool bTrue)
        {
            foreach (XUIListItem uiListItem in m_listXUIListItem)
            {
                uiListItem.Highlight(bTrue);
            }
        }

        public void OnSelectItem(XUIListItem uiListItem)
        {
            if (null != m_eventHandlerOnSelectStateChange)
            {
                m_eventHandlerOnSelectStateChange(uiListItem);
            }
        }

        public void OnUnSelectItem(XUIListItem uiListItem)
        {
            if (null != m_eventHandlerOnSelectStateChange)
            {
                m_eventHandlerOnSelectStateChange(uiListItem);
            }
        }

        public void RegisterSelectStateChangeEventHandler(UIEvent selectStateChangeHandler)
        {
            m_eventHandlerOnSelectStateChange = selectStateChangeHandler;
        }

        public void Highlight(List<UInt32> listIds)
        {
            //Highlight(false);
            //if (null == listIds)
            //{
            //    return;
            //}
            //foreach (XUIListItem uiListItem in m_listXUIListItem)
            //{
            //    if (listIds.Contains(uiListItem.id) == true)
            //    {
            //        uiListItem.Highlight(true);
            //    }
            //}
        }

        public void MoveToIndex(int index)
        {
            if(m_scrollRect != null && ItemsRootRect != null)
            {
                IXUIListItem item = GetItemByIndex(index);
                if (item != null)
                {
                    Vector3 itemLocalPos = item.CachedRectTransform.localPosition;
                    Vector2 contentSize = ItemsRootRect.sizeDelta;
                    Vector2 rectSize = m_scrollRect.GetComponent<RectTransform>().rect.size;
                    if (m_scrollRect.vertical)
                    {
                        float yy = Math.Max(0.0f, ((contentSize.y - rectSize.y) - Math.Abs(itemLocalPos.y))) / (contentSize.y - rectSize.y);
                        yy = Math.Min(1, yy);
                        m_scrollRect.normalizedPosition = new Vector2(0, yy);
                    }
                    else
                    {
                        float xx = Math.Max(0.0f, ((contentSize.x - rectSize.x) - Math.Abs(itemLocalPos.x))) / (contentSize.x - rectSize.x);
                        xx = Math.Min(1, xx);
                        m_scrollRect.normalizedPosition = new Vector2(xx, 0);
                    }
                }
            }
        }

        public void MoveTo(RectTransform itemTransform)
        {
            if (m_scrollRect != null && ItemsRootRect != null && itemTransform != null)
            {
                Vector3 itemLocalPos = itemTransform.localPosition;
                Vector2 contentSize = ItemsRootRect.sizeDelta;
                Vector2 rectSize = m_scrollRect.GetComponent<RectTransform>().rect.size;
                if (m_scrollRect.vertical)
                {
                    float fItemSizeY = itemTransform.sizeDelta.y;
                    if (contentSize.y <= rectSize.y)
                    {
                        return;
                    }

                    float fY = CachedRectTransform.localPosition.y;
                    float fSize = contentSize.y - rectSize.y;
                    float fY1 = Mathf.Abs(itemLocalPos.y) + fItemSizeY - rectSize.y;
                    float fY2 = Mathf.Abs(itemLocalPos.y);

                    fY = Mathf.Clamp(fY, fY1, fY2);
                    Vector3 localPos = CachedRectTransform.localPosition;
                    localPos.y = fY;
                    CachedRectTransform.localPosition = localPos;
                }
                else
                {
                    float fItemSizeX = itemTransform.sizeDelta.x;
                    if (contentSize.x <= rectSize.x)
                    {
                        return;
                    }

                    float fX = CachedRectTransform.localPosition.x;
                    float fSize = contentSize.x - rectSize.x;
                    float fX1 = Mathf.Abs(itemLocalPos.x) + fItemSizeX - rectSize.x;
                    float fX2 = Mathf.Abs(itemLocalPos.x);

                    fX = Mathf.Clamp(fX, fX1, fX2);
                    Vector3 localPos = CachedRectTransform.localPosition;
                    localPos.x = fX;
                    CachedRectTransform.localPosition = localPos;
                }
            }
        }

        public void MoveToHead()
        {
            MoveToIndex(0);
        }

        public void MoveToTail()
        { 
            MoveToIndex(m_listXUIListItem.Count-1);
        }


        public override void Init()
        {
            base.Init();

            if (m_itemsRoot == null)
            {
                m_itemsRoot = this.CachedTransform;
            }

            m_scrollRect = GetComponent<ScrollRect>();

            if(m_scrollRect == null)
            {
                m_scrollRect = transform.GetComponentInParent<ScrollRect>();
            }

            //if (null != m_scrollRect)
            //{
            //    m_itemsRoot = m_scrollRect.content.transform;
            //    //Debug.LogWarning("scrollRect is null");
            //    //return;
            //}

            
            m_layoutGroup = m_itemsRoot.GetComponent<GridLayoutGroup>();
            
            //if (null == m_layoutGroup)
            //{
            //    Debug.LogWarning("layoutGroup is null");
            //}
            m_listXUIListItem.Clear();
            for (int i = 0; i < m_itemsRoot.childCount; i++)
            {
                Transform child = m_itemsRoot.GetChild(i);

                if (m_bHideSrc && child.gameObject.GetHashCode() == m_prefabListItem.GetHashCode()){
                    child.gameObject.SetActive(false);
                    continue;
                }

                XUIListItem uiListItem = child.GetComponent<XUIListItem>();
                if (null == uiListItem)
                {
                    Debug.LogWarning(string.Format("null == uiListItem. path ={0}", XUITool.GetHierarchy(child.gameObject)));
                }
                else
                {
                    uiListItem.parent = this;
                    uiListItem.DlgBehaviour = this.DlgBehaviour;
                    this.m_listXUIListItem.Add(uiListItem);
                }
            }
            //m_listXUIListItem.Sort(SortByName);

            Int32 nIndex = 0;
            foreach (XUIListItem uiListItem in m_listXUIListItem)
            {
                uiListItem.name = string.Format("{0:0000}", nIndex);
                uiListItem.Index = nIndex;
                if (uiListItem.IsInited == false)
                {
                    uiListItem.Init();
                }

                Toggle uiCheckbox = uiListItem.GetComponent<Toggle>();
                if (null != uiCheckbox)
                {
                    if (false == m_bMultiSelect)
                    {
                        uiCheckbox.group = uiCheckbox.transform.parent.GetComponent<ToggleGroup>();
                    }
                }

                ++nIndex;
            }
            //if (null != m_uiTable)
            //{
            //    m_uiTable.Reposition();
            //}
            //RefreshAllItemStatus();
        }

        //private void RefreshAllItemStatus()
        //{
        //    switch (m_eMoveCenterType)
        //    {
        //        case EnumMoveCenterType.eMoveCenterType_Increas:
        //            {
        //                int nCurIndex = GetSelectedIndex();
        //                if (nCurIndex < 0) SetSelectedIndex(0); nCurIndex = 0;
        //                foreach (XUIListItem item in m_listXUIListItem) item.InitStatus(m_eMoveCenterType, nCurIndex);
        //            }
        //            break;
        //    }
        //}


        public void SetAllItemsVisible(bool active)
        {
            for (int index = 0; index < this.Count; index++)
            {
                this.GetItemByIndex(index).SetVisible(active);
            }
        }

        public Vector2 GetCellSize() {
            if (m_layoutGroup == null)
                return Vector2.zero;
            return m_layoutGroup.cellSize;
        }

        public Vector2 GetCellPadding() {
            if (m_layoutGroup == null)
                return Vector2.zero;
            return m_layoutGroup.spacing;
        }

        public int GetDisplayCellCount() {
            int count = 0;
            foreach (var it in m_listXUIListItem) {
                if (!it.IsVisible())
                    continue;
                count++;
            }
            return count;
        }

        public int GetRowCount() {
            int childCount = GetDisplayCellCount();

            // 获取容器宽度
            float totalWidth = CachedRectTransform.rect.width;

            float cellWidth = GetCellSize().x;
            float spacingX = GetCellPadding().x;

            int columns = Mathf.FloorToInt((totalWidth + spacingX) / (cellWidth + spacingX));
            if (columns <= 0) columns = 1;

            int rows = Mathf.CeilToInt((float)childCount / columns);

            return rows;
        }

        public void SetItemVisibleCount(int count) {
            for (int index = 0; index < m_listXUIListItem.Count; ++index) 
                m_listXUIListItem[index].SetVisible(index < count);
        }

        //private void MoveWidthCenter(int nIndex)
        //{
        //    if ((true == m_bMultiSelect) && (null == m_uiGrid || m_uiGrid.maxPerLine > 0))
        //        return;

        //    //if (true == m_bMoveFinish)
        //    //{
        //        m_vStartPos = transform.localPosition;
        //        m_vFinishPos = m_vStartPos;
        //        m_vFinishPos.x = -1 * m_uiGrid.cellWidth * nIndex;

        //        for (int i = 0; i < m_listXUIListItem.Count; i++) m_listXUIListItem[i].InitMoveCenterInfo(m_eMoveCenterType, nIndex);

        //        m_fStartTime = Time.unscaledTime;
        //        m_bMoveFinish = false;
        //    //}
        //}

        //private void RefreshItemSizeByTable() // uiTable存在时调用
        //{
        //    foreach (XUIListItem item in m_listXUIListItem)
        //    {
        //        if (null != item) item.RefreshSize();
        //    }
        //}

        static public int SortByName(XUIListItem a, XUIListItem b) { return string.Compare(a.name, b.name); }
        
        private List<XUIListItem> m_listXUIListItem = new List<XUIListItem>();//need to change into List<XUIListItem>

        private GridLayoutGroup m_layoutGroup = null;
        private ScrollRect m_scrollRect = null;
        private RectTransform m_ItemsRootRect = null;

        private bool m_bMoveFinish = true;
        private Vector3 m_vStartPos = Vector3.zero;
        private Vector3 m_vFinishPos = Vector3.zero;
        protected UIEvent m_eventHandlerOnSelectStateChange = null;
    }
}