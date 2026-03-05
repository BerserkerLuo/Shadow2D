using UnityEngine;
using UnityEngine.UI;
using Client.UI.UICommon;
using System.Collections;
using DG.Tweening;

namespace UILib
{
    public class XUIProgress : XUIObject, IXUIProgress
    {
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

        public float Value
        {
            get
            {
                return m_uiSlider.value;
            }
            set
            {
                m_uiSlider.value = value;
                m_fLastValue = value;
                StopAllCoroutines();
            }
        }

        public UnityEngine.Color Color
        {
            get
            {
                if (null != m_uiSpriteFG)
                {
                    return m_uiSpriteFG.color;
                }
                return Color.white;
            }
            set
            {
                if (null != m_uiSpriteFG)
                {
                    m_uiSpriteFG.color = value;
                }
            }
        }

        public override void Init()
        {
            base.Init();
            m_uiSlider = GetComponent<Slider>();
            if (null == m_uiSlider)
            {
                Debug.LogError("null == m_uiSlider");
            }
            else
            {
                if (m_uiSlider.fillRect != null)
                {
                    m_uiSpriteFG = m_uiSlider.fillRect.GetComponent<Image>();
                }
            }

            if (null == m_uiSpriteFG)
            {
                Debug.LogError("null == m_uiSpriteFG");
            }
        }

        public void TweenValue(float targetValue, float fTime, float fDelay = 0.0f)
        {
            StopAllCoroutines();
            StartCoroutine(DoTweenValue(targetValue, fTime, fDelay));
        }

        private IEnumerator DoTweenValue(float end, float fTime, float delay)
        {
            float start = m_fLastValue;
            yield return new WaitForSeconds(delay);
            float fStartTime = Time.time;

            float fRatio = (Time.time - fStartTime) / fTime;
            float fValue = start;
            while (fRatio < 1.0f)
            {
                fValue = Mathf.Lerp(start, end, fRatio);
                m_uiSlider.value = fValue;
                m_fLastValue = fValue;
                yield return new WaitForSeconds(0.1f);
                fRatio = (Time.time - fStartTime) / fTime;
            }
            m_uiSlider.value = end;
            m_fLastValue = end;
        }

        private Slider m_uiSlider = null;
        private Image m_uiSpriteFG = null;
        private float m_fLastValue = 0.0f;
    }
}
