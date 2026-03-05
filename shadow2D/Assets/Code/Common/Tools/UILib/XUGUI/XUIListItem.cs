using System;
using Client.UI.UICommon;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UILib.Export;

namespace UILib
{
    public class XUIListItem : XUIObject, IXUIListItem
    {
        public Image bgSprite = null;

        public Int32 Index
        {
            get
            {
                return m_nIndex;
            }
            set
            {
                m_nIndex = value;
            }
        }

        public UInt32 id
        {
            get { return m_unId; }
            set { m_unId = value; }
        }

        public UInt64 GUID
        {
            get { return m_unGUID; }
            set { m_unGUID = value; }
        }

        public object Data
        {
            get { return m_data; }
            set { m_data = value; }
        }

        public bool IsSelected
        {
            get
            {
                if (null != m_uiCheckBox)
                {
                    return m_uiCheckBox.isOn;
                }
                return false;
            }
            set
            {
                if (null != m_uiCheckBox)
                {
                    if (m_uiCheckBox.isOn != value)
                    {
                        m_uiCheckBox.isOn = value;
                        //if (true == value)
                        //{
                        //    ParentXUIList.SelectItem(this, false);
                        //}
                        //else
                        //{
                        //    ParentXUIList.UnSelectItem(this, false);
                        //}
                    }
                }
            }
        }

        public XUIList ParentXUIList
        {
            get
            {
                XUIList uiList = parent as XUIList;
                if (null == uiList)
                {
                    Debug.LogError("null == uiList");
                }
                return uiList;
            }
        }

        public Color HighlightColor
        {
            get
            {
                return m_colorHighlight;
            }
            set
            {

            }
        }

        public void SetSelected(bool bTrue)
        {
            if (null != m_uiCheckBox)
            {
                if (m_uiCheckBox.isOn != bTrue)
                {
                    m_uiCheckBox.isOn = bTrue;
                }
            }
        }

        public void SetVisible(string strId, bool bVisible)
        {
            IXUIObject uiObject = GetUIObject(strId);
            if (null != uiObject)
            {
                uiObject.SetVisible(bVisible);
            }
        }

        public void SetLabelText(string strId, string strText)
        {
            IXUILabel uiLabel = GetUIObject(strId) as IXUILabel;
            if (null != uiLabel)
            {
                uiLabel.SetText(strText);
            }
        }

        public void SetSpriteColor(string strId, Color color)
        {
            IXUISprite uSprite = GetUIObject(strId) as IXUISprite;
            if (null != uSprite)
            {
                uSprite.Color = color;
            }
        }

        public void SetSpriteColor(string strId, string htmlString)
        {
            Color color;
            ColorUtility.TryParseHtmlString(htmlString, out color);
            IXUISprite uSprite = GetUIObject(strId) as IXUISprite;
            if (null != uSprite && color != null)
            {
                uSprite.Color = color;
            }
        }

        public void SetLabelColor(string strId, Color color)
        {
            IXUILabel uiLabel = GetUIObject(strId) as IXUILabel;
            if (null != uiLabel)
            {
                uiLabel.Color = color;
            }
        }

        public void SetSprite(string strId, string abName, string resName)
        {
            IXUISprite uSprite = GetUIObject(strId) as IXUISprite;
            if (null != uSprite)
            {
                uSprite.SetSprite(abName, resName);
            }
        }

        public void SetFillAmount(string strId, float fValue)
        {
            IXUISprite uSprite = GetUIObject(strId) as IXUISprite;
            if (null != uSprite)
            {
                uSprite.SetFillAmount(fValue);
            }
        }

        public void SetChildGray(string strId, bool bGray)
        {
            IXUIObject uiObject = GetUIObject(strId);
            if (null != uiObject)
            {
                uiObject.SetGray(bGray);
            }
        }

        public void RegisterObjectClickEventHandler(string strId, UIEvent btnClickHandler)
        {
            IXUIObject uObject = GetUIObject(strId);
            if (null != uObject)
            {
                uObject.RegisterClickEventHandler(btnClickHandler);
            }
        }

        public void SetEnableSelect(bool bEnable)
        {
            //if (false == bEnable)
            //{
            //    if (null != m_uiCheckBox)
            //    {
            //        m_uiCheckBox.isChecked = false;
            //    }
            //}
            //Highlight(bEnable);
            //if (null != m_uiCheckBox)
            //{
            //    m_uiCheckBox.enabled = bEnable;
            //}
        }

        //public void SetEnableMultiSelect(bool bEnable)
        //{
        //    if (null != m_uiCheckBox)
        //    {
        //        if (false == bEnable)
        //        {
        //            m_uiCheckBox.radioButtonRoot = m_uiCheckBox.transform.parent;
        //        }
        //        else
        //        {
        //            m_uiCheckBox.radioButtonRoot = null;
        //        }
        //    }
        //}

        public void SetColor(Color color)
        {
            if (null != bgSprite)
            {
                bgSprite.color = color;
            }
        }

        public void SetColor(string strId, Color color)
        {
            IXUISprite uSprite = GetUIObject(strId) as IXUISprite;
            if (null != uSprite)
            {
                uSprite.Color = color;
            }
        }

        public void SetColor(string strId, string htmlString)
        {
            Color color;
            ColorUtility.TryParseHtmlString(htmlString, out color);
            IXUISprite uSprite = GetUIObject(strId) as IXUISprite;

            if (null != uSprite && color != null)
            {
                uSprite.Color = color;
            }
        }

        public override void SetEnable(bool bEnable)
        {
            //if (null != m_uiButton)
            //{
            //    m_uiButton.interactable = bEnable;
            //}
            base.SetEnable(bEnable);
        }

        public void Clear()
        {
            if (null != bgSprite)
            {
                bgSprite.enabled = false;
            }
            Tip = null;
        }

        private void OnSelectStateChange(bool bSelected)
        {
            if (true == bSelected)
            {
                ParentXUIList.OnSelectItem(this);
            }
            else
            {
                ParentXUIList.OnUnSelectItem(this);
            }
        }

        public virtual void SetDepth(int nDepth)
        {

        }


        public override void Init()
        {
            base.Init();

            SearchAllUIObject();

            if (null == bgSprite){
                Transform transIcon = transform.Find("Sprite_BG");
                if (null != transIcon)
                    bgSprite = transIcon.GetComponent<Image>();
            }
            m_uiCheckBox = GetComponent<Toggle>();
            if (null != m_uiCheckBox){
                m_uiCheckBox.onValueChanged.AddListener(OnSelectStateChange);
            }

            Highlight(false);
        }

        void OnDestroy()
        {
            SafeXUIObject.OnDestoryXUIObject(this);
        }

        public void SetEnableOpen(bool bOpen)
        {
            
        }

        public Int32 m_nIndex = -1;
        public UInt32 m_unId = 0;
        public UInt64 m_unGUID = 0;
        protected object m_data = null;
        private Toggle m_uiCheckBox = null;
        private LayoutElement m_element = null;

        private Color m_colorHighlight = Color.clear;
    }
}
