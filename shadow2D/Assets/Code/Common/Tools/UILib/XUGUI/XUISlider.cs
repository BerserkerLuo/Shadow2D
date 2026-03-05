using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Client.UI.UICommon;
using System;

namespace UILib
{
    public class XUISlider : XUIObject, IXUISlider
    {
        public float Value
        {
            get
            {
                return m_uiSlider.value;
            }
            set
            {
                m_uiSlider.value = value;
            }
        }

        public float MinValue
        {
            get
            {
                return m_uiSlider.minValue;
            }
            set
            {
                m_uiSlider.minValue = value;
            }
        }

        public float MaxValue
        {
            get
            {
                return m_uiSlider.maxValue;
            }
            set
            {
                m_uiSlider.maxValue = value;
            }
        }

        public bool Interactable
        {
            get
            {
                return m_uiSlider.interactable;
            }
            set
            {
                m_uiSlider.interactable = value;
            }
        }

        public override void Init()
        {
            base.Init();
            this.m_uiSlider = base.GetComponent<Slider>();
            if (null != this.m_uiSlider)
            {
                m_uiSlider.onValueChanged.AddListener(this.OnValueChange);
            }
            else
            {
                Debug.LogError("null == m_uiSlider");
            }
        }

        public void RegisterValueChangeEventHandler(UIEvent eventHandler)
        {
            m_valueChangeEventHandler = eventHandler;
        }

        private void OnValueChange(float val)
        {
            if (null != m_valueChangeEventHandler)
            {
                m_valueChangeEventHandler(this);
            }
        }

        public Slider GetSlider()
        {
            return this.m_uiSlider;
        }

        private UIEvent m_valueChangeEventHandler = null;
        private Slider m_uiSlider = null;
    }
}
