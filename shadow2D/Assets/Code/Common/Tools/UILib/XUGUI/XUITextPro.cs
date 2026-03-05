using UnityEngine;
using UnityEngine.UI;
using Client.UI.UICommon;
using System.Collections;
using System;
using System.Text;
using DG.Tweening;
using TMPro;
using UnityEngine.Events;

namespace UILib
{
    public class XUITextPro : XUIObject, IXUILabel
    {
        enum EType
        {
            eType_None,
            eType_Timer,
            eType_Counter,
            eType_Tween,
        }

        public float m_fAlphaVar;
        public float AlphaVar
        {
            get { return m_fAlphaVar; }
        }

        public override float Alpha
        {
            get
            {
                if (null != m_uiLabel)
                {
                    return m_uiLabel.color.a;
                }
                return 1;
            }
            set
            {
                if (null != m_uiLabel)
                {
                    m_uiLabel.color = new Color(m_uiLabel.color.r, m_uiLabel.color.g, m_uiLabel.color.b, value);
                }
            }
        }

        public int fontSize
        {
            get
            {
                if (null != m_uiLabel)
                {
                    return (int)m_uiLabel.fontSize;
                }
                return 0;
            }
            set
            {
                if (null != m_uiLabel)
                {
                    m_uiLabel.fontSize = value;
                }
            }
        }

        public Color Color
        {
            get
            {
                if (null != m_uiLabel)
                {
                    return m_uiLabel.color;
                }
                return Color.white;
            }
            set
            {
                if (null != m_uiLabel)
                {
                    m_uiLabel.color = value;
                }
            }
        }

        public float preferredWidth
        {
            get
            {
                float result = 0;
                if (null != m_uiLabel)
                {
                    result = m_uiLabel.preferredWidth;
                }
                return result;
            }
        }

        public float preferredHeight
        {
            get
            {
                float result = 0;
                if (null != m_uiLabel)
                {
                    result = m_uiLabel.preferredHeight;
                }
                return result;
            }
        }

        public Text Text
        {
            get
            {
                return null;
            }
        }

        public TextMeshProUGUI TextMeshPro{
            get{
                return m_uiLabel;
            }
        }
        

        public string GetText(){
            if (null != m_uiLabel){
                return m_uiLabel.text;
            }
            return "";
        }

        public void SetText(string strText)
        {
            m_eType = EType.eType_None;
            if (null != m_coroutine)
            {
                StopCoroutine(m_coroutine);
                m_coroutine = null;
            }
            if (null != m_uiLabel)
            {
                if (string.IsNullOrEmpty(strText) == false)
                {
                    strText = strText.Replace("\\n", "\n");
                }
                m_uiLabel.text = strText;
            }
        }

        public void SetColor(string htmlString)
        {
            Color color;
            ColorUtility.TryParseHtmlString(htmlString, out color);

            if (null != m_uiLabel && color != null)
            {
                m_uiLabel.color = color;
            }
        }

        public void TweenValueFrom(float fStart, float targetValue, float fTime, string strFormat, float delay = 0.0f)
        {
            m_fLastValue = fStart;
            TweenValue(targetValue, fTime, strFormat, delay);
        }

        public void TweenValue(float targetValue, float fTime, string strFormat, float delay = 0.0f)
        {
            m_eType = EType.eType_Tween;
            if (null != m_coroutine)
            {
                StopCoroutine(m_coroutine);
                m_coroutine = null;
            }
            m_coroutine = StartCoroutine(DoTweenValue(targetValue, fTime, strFormat, delay));
        }

        private IEnumerator DoTweenValue(float end, float fTime, string strFormat, float delay)
        {
            string strText = string.Empty;
            float start = m_fLastValue;
            yield return new WaitForSeconds(delay);
            float fStartTime = Time.time;

            float fRatio = (Time.time - fStartTime) / fTime;
            float fValue = start;
            while (fRatio < 1.0f)
            {
                fValue = Mathf.Lerp(fValue, end, fRatio);
                strText = fValue.ToString(); //string.Format(strFormat, fValue);
                SetText(strText);
                m_fLastValue = fValue;
                yield return new WaitForEndOfFrame();
                fRatio = (Time.time - fStartTime) / fTime;
            }

            strText = end.ToString();//string.Format(strFormat, end);
            if (null != m_uiLabel)
                m_uiLabel.text = strText;
            m_fLastValue = end;
            m_coroutine = null;
        }

        public FontStyle fontStyle
        {
            get
            {
                if (null != this.m_uiLabel)
                {
                    return (FontStyle)this.m_uiLabel.fontStyle;
                }
                return FontStyle.Normal;
            }
            set
            {
                if (null != this.m_uiLabel)
                {
                    this.m_uiLabel.fontStyle = (FontStyles)value;
                }
            }
        }

        public TextAnchor alignment
        {
            get
            {
                //if (null != this.m_uiLabel)
                //{
                //    return this.m_uiLabel.alignment;
                //}
                return TextAnchor.UpperLeft;
            }
            set
            {
                //if (null != this.m_uiLabel)
                //{
                //    this.m_uiLabel.alignment = value;
                //}
            }
        }

        public override void Init()
        {
            base.Init();
            m_uiLabel = GetComponent<TextMeshProUGUI>();
            if (null == m_uiLabel){
                Debug.LogError("null == m_uiLabel");
            }
        }

        void Update()
        {
            if (m_eType == EType.eType_None || Time.unscaledTime - m_fLastUpdateTime < 0.1f)
                return;
            m_fLastUpdateTime = Time.unscaledTime;

            if (m_eType == EType.eType_Timer && m_bEnableTimer == true)
            {
                float nLeftSecond = m_nLeftSeconds - (Time.unscaledTime - m_fStartTime);
                if (nLeftSecond < 0)
                {
                    m_bEnableTimer = false;
                    nLeftSecond = 0;
                }
                //TimeSpan timespan = TimeSpan.FromSeconds(nLeftSecond);
                //StringBuilder strTimeInfo = new StringBuilder();
                //if (timespan.Days > 0)
                //{
                //    strTimeInfo.AppendFormat("{0}{1}{2:00}:{3:00}:{4:00} ", timespan.Days, m_strDay, timespan.Hours, timespan.Minutes, timespan.Seconds);
                //}
                //else if (timespan.Hours > 0)
                //{
                //    strTimeInfo.AppendFormat("{0:00}:{1:00}:{2:00}", timespan.Hours, timespan.Minutes, timespan.Seconds);
                //}
                //else if (timespan.Minutes > 0)
                //{
                //    strTimeInfo.AppendFormat("{0:00}:{1:00}", timespan.Minutes, timespan.Seconds);
                //}
                //else
                //{
                //    strTimeInfo.AppendFormat("{0}", timespan.Seconds);
                //}


                //m_uiLabel.text = strTimeInfo.ToString();
                m_uiLabel.text = string.Format(m_strFormat, ((int)nLeftSecond).ToString());
            }
            else if (m_eType == EType.eType_Counter)
            {
                float fEcliapseTime = Time.unscaledTime - m_fStartTime;
                long count = m_nStartCount + (long)(m_nCounterSpeed * fEcliapseTime);
                string strCount = count.ToString();
                m_uiLabel.text = strCount;
            }
        }


        public void StartTimer(int nLeftSeconds, string strFormat)
        {
            m_eType = EType.eType_Timer;
            m_nLeftSeconds = nLeftSeconds;
            m_strFormat = strFormat;
            m_bEnableTimer = true;
            m_fStartTime = Time.unscaledTime;
            Update();
        }

        /// <summary>
        /// StartCounter
        /// </summary>
        /// <param name="startCount">init count</param>
        /// <param name="speed">per second</param>
        public void StartCounter(long startCount, long speed)
        {
            m_eType = EType.eType_Counter;
            m_nStartCount = startCount;
            m_nCounterSpeed = speed;
            m_fStartTime = Time.unscaledTime;
            Update();
        }

        public void TypeText(string text, float printDelay = -1, UnityAction finish = null)
        {
            if (m_coroutine != null)
            {
                StopCoroutine(m_coroutine);
            }
            SetTypeTextSpeed(printDelay);
            m_coroutine = StartCoroutine(DoTypeText(text, finish));
        }

        private IEnumerator DoTypeText(string text, UnityAction finish)
        {
            if (m_uiLabel == null) yield break;

            m_uiLabel.text = ""; // 先清空文本

            if (PrintDelay <= 0)
            {
                m_uiLabel.text = text; // 立即显示完整文本
                finish?.Invoke(); // 触发回调
                yield break;
            }

            for (int i = 0; i <= text.Length; i++)
            {
                m_uiLabel.text = text.Substring(0, i);
                yield return new WaitForSeconds(PrintDelay); // 等待
            }

            finish?.Invoke(); // 触发回调
        }

        public void SetTypeTextSpeed(float printDelay) {
            PrintDelay = printDelay;
        }

        protected TextMeshProUGUI m_uiLabel = null;

        private string m_strFormat;
        private float m_fLastValue = 0.0f;

        private int m_nLeftSeconds = 0;
        private float m_fStartTime = 0;
        private bool m_bEnableTimer = false;
        private float m_fLastUpdateTime = 0;

        private long m_nStartCount = 0;
        private long m_nCounterSpeed = 0;
        private EType m_eType = EType.eType_None;
        private Coroutine m_coroutine = null;
        private float PrintDelay = 0.1f;
    }

}