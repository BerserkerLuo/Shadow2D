using UnityEngine;
using UnityEngine.UI;
using Client.UI.UICommon;
using UnityEngine.EventSystems;

namespace UILib
{
    public class XUIInput : XUIObject, IXUIInput
    {
        public bool IsSelected
        {
            get
            {
                if (null != m_uiInput)
                {
                    return m_uiInput.isFocused;
                }
                return false;
            }
            set
            {
                if (null != m_uiInput)
                {
                    if (value == true)
                    {
                        m_uiInput.ActivateInputField();
                    }
                    else
                    {
                        m_uiInput.DeactivateInputField();
                    }
                }
            }
        }

        public bool Interactable
        {
            get
            {
                bool result = false;
                if(m_uiInput != null)
                {
                    result = m_uiInput.interactable;
                }
                return result;
            }
            set
            {
                if(m_uiInput != null)
                {
                    m_uiInput.interactable = value;
                }
            }
        }

        public override void Init()
        {
            base.Init();
            m_uiInput = GetComponent<InputField>();
            if (null != m_uiInput)
            {
                m_uiInput.onEndEdit.AddListener(this._OnSubmit);
                m_uiInput.onValueChanged.AddListener(this._OnValueChanged);
          
            }
            else
            {
                Debug.LogError(string.Format("null == m_uiInput:{0}", XUITool.GetHierarchy(CachedGameObject)));
            }
        }

        public string GetText()
        {
            if (null != m_uiInput)
            {
                return m_uiInput.text;
            }
            return "";
        }

        public void SetText(string strText)
        {
            if (null != m_uiInput)
            {
                m_uiInput.text = strText;
            }
        }

        public void Select()
        {
            m_uiInput.Select();
        }

        public void RegisterSubmitEventHandler(UIEvent eventHandler)
        {
            m_inputSubmitEventHandler = eventHandler;
        }

        public void RegisterOnValueChanged(UIEvent eventHandler)
        {
            m_inputOnValueChangedEventHandler = eventHandler;
        }

        public void RegisterOnInputSelectEventHandler(UIEvent eventHandler)
        {
            m_inputOnSelectEventHandler = eventHandler;
        }

        void _OnSubmit(string strText)
        {
            Debug.Log("_OnSubmit");
            if (null != m_inputSubmitEventHandler)
            {
                if (m_inputSubmitEventHandler(this) == true)
                {
                    //XUITool.Instance.IsEventProcessed = true;
                }
            }
        }

        void _OnValueChanged(string strText)
        {
            if (null != m_inputOnValueChangedEventHandler)
            {
                if (m_inputOnValueChangedEventHandler(this) == true)
                {
                    //XUITool.Instance.IsEventProcessed = true;
                }
            }
        }

        public void OnSelect(BaseEventData eventData)
        {
            if(null != m_inputOnSelectEventHandler)
            {
                m_inputOnSelectEventHandler(this);
            }
        }

        private InputField m_uiInput = null;
        private UIEvent m_inputSubmitEventHandler = null;
        private UIEvent m_inputOnValueChangedEventHandler = null;
        private UIEvent m_inputOnSelectEventHandler = null;
    }

}