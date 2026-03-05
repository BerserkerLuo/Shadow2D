using UnityEngine;
using System.Collections;
using Client.UI.UICommon;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UILib
{
    [AddComponentMenu("XUI/XUICheckBox")]
    public class XUICheckBox : XUIObject, IXUIObject, IXUICheckBox
    {
        // Properties
        public bool bChecked
        {
            get
            {
                return ((null != this.m_uiCheckBox) && this.m_uiCheckBox.isOn);
            }
            set
            {
                if (null != this.m_uiCheckBox)
                {
                    this.m_uiCheckBox.isOn = value;
                }
            }
        }

        public object Data
        {
            get { return m_data; }
            set { m_data = value; }
        }

        // Methods
        public override void Init()
        {
            base.Init();
            this.m_uiCheckBox = base.GetComponent<Toggle>();
            if (null == this.m_uiCheckBox)
            {
                Debug.LogError("null == m_uiCheckBox");
                return;
            }
            this.m_uiCheckBox.onValueChanged.AddListener(this.OnStateChange);
        }



        public void RegisterOnCheckEventHandler(UIEvent eventHandler)
        {
            m_eventHandlerOnCheck = eventHandler;
        }

        public override void SetEnable(bool bEnable)
        {
            base.SetEnable(bEnable);
            if (null != this.m_uiCheckBox)
            {
                this.m_uiCheckBox.interactable = bEnable;
            }
        }

        private void OnStateChange(bool value)
        {
            if (null != m_eventHandlerOnCheck)
            {
                m_eventHandlerOnCheck(this);
            }
        }

        // Fields
        private UIEvent m_eventHandlerOnCheck;
        private Toggle m_uiCheckBox;
        protected object m_data = null;
    }

}