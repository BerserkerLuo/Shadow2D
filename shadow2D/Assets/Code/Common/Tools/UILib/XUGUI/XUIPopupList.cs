using System;
using Client.UI.UICommon;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UILib
{
    public class XUIPopupList : XUIObject, IXUIPopupList
    {
        struct SPopupListItem
        {
            public string Name
            {
                get { return m_strName; }
            }

            public object Data
            {
                get { return m_data; }
            }


            public SPopupListItem(string strTitle, object data)
            {
                m_strName = strTitle;
                m_data = data;
            }

            private string m_strName;
            private object m_data;
        }

        public int SelectedIndex
        {
            get
            {
                return m_stSelectedIndex;
            }
            set
            {
                if (m_listItems.Count == 0)
                    return;
                m_stSelectedIndex = Mathf.Clamp(value, 0, m_listItems.Count - 1);
                m_strSelection = m_listItems[m_stSelectedIndex].Name;
                m_uiPopupList.value = m_stSelectedIndex;
            }
        }

        public string Selection
        {
            get
            {
                return m_strSelection;
            }
            set
            {
                for (short nIndex = 0; nIndex < m_listItems.Count; ++nIndex)
                {
                    if (m_listItems[nIndex].Name.Equals(value) == true)
                    {
                        m_strSelection = value;
                        SelectedIndex = nIndex;
                    }
                }
            }
        }

        public bool AddItem(string strItem)
        {
            return AddItem(strItem, null);
        }

        public bool AddItem(string strItem, object data)
        {
            SPopupListItem item = new SPopupListItem(strItem, data);
            m_listItems.Add(item);
            if (null != m_uiPopupList)
            {
                if (m_uiPopupList.options.Count == 0)
                {
                    m_uiPopupList.AddOptions(new List<Dropdown.OptionData>() { new Dropdown.OptionData(strItem) });
                }
                else
                {
                    m_uiPopupList.AddOptions(new List<Dropdown.OptionData>() { new Dropdown.OptionData(strItem) });
                }

            }
            return true;
        }

        public void Clear()
        {
            m_listItems.Clear();
            if (null != m_uiPopupList)
            {
                m_uiPopupList.ClearOptions();
            }
        }

        public object GetDataByIndex(int nIndex)
        {
            if (nIndex < m_listItems.Count)
            {
                SPopupListItem item = m_listItems[nIndex];
                return item.Data;
            }
            return null;
        }

        public void RegisterPopupListSelectEventHandler(UIEvent eventHandler)
        {
            m_popupListSelectEventHandler = eventHandler;
        }

        public override void Init()
        {
            base.Init();
            m_uiPopupList = GetComponent<Dropdown>();
            if (null != m_uiPopupList)
            {
                m_uiPopupList.onValueChanged.AddListener(OnSelectionChange);
                for (int nIndex = 0; nIndex < m_uiPopupList.options.Count; ++nIndex)
                {
                    SPopupListItem item = new SPopupListItem(m_uiPopupList.options[nIndex].text, null);
                    m_listItems.Add(item);
                }
            }
            else
            {
                Debug.LogError("null == m_uiPopupList:" + XUITool.GetHierarchy(CachedGameObject));
            }
        }

        private void OnSelectionChange(int index)
        {
            m_stSelectedIndex = index;
            m_strSelection = m_uiPopupList.options[index].text;
            if (null != m_popupListSelectEventHandler)
            {
                m_popupListSelectEventHandler(this);
            }
        }

        private int m_stSelectedIndex = 0;
        private string m_strSelection = "";
        private UIEvent m_popupListSelectEventHandler = null;
        private Dropdown m_uiPopupList = null;
        private List<SPopupListItem> m_listItems = new List<SPopupListItem>();
    }

}