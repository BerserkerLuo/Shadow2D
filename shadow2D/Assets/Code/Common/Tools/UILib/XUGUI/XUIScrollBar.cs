using UnityEngine;
using UnityEngine.UI;
using Client.UI.UICommon;

namespace UILib
{
    public class XUIScrollBar : XUIObject, IXUIScrollBar
    {

        public float Value
        {
            get { return m_uiScrollBar.value; }
            set { m_uiScrollBar.value = value; }
        }

        public float Size
        {
            get { return m_uiScrollBar.size; }
            set { m_uiScrollBar.size = value; }
        }

        public void RegisterScrollBarChangeEventHandler(UIEvent eventHandler)
        {
            m_scrollBarChangeEventHandler = eventHandler;
        }


        public override void Init()
        {
            base.Init();
            m_uiScrollBar = GetComponent<Scrollbar>();
            if (null == m_uiScrollBar)
            {
                Debug.LogError("null == m_uiScrollBar");
            }
            else
            {
                m_uiScrollBar.onValueChanged.AddListener(this.OnValueChange);
            }
        }

        private void OnValueChange(float barValue)
        {
            if (null != m_scrollBarChangeEventHandler)
            {
                m_scrollBarChangeEventHandler(this);
            }
        }

        private Scrollbar m_uiScrollBar = null;
        private UIEvent m_scrollBarChangeEventHandler = null;
    }
}
