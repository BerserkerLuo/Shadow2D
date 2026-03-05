using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Client.UI.UICommon
{
    public interface IXUILabel : IXUIObject
    {
        Color Color { get; set; }
        string GetText();
        int fontSize { get; set; }
        void SetText(string strText);
        void SetColor(string htmlString);
        void StartTimer(int nLeftSeconds, string strDay = "{0}s");
        /// <summary>
        /// StartCounter
        /// </summary>
        /// <param name="startCount">init count</param>
        /// <param name="speed">per second</param>
        void StartCounter(long startCount, long speed);
        void TweenValue(float targetValue, float fTime, string strFormat, float delay = 0.0f);
        void TweenValueFrom(float fStart, float targetValue, float fTime, string strFormat, float delay = 0.0f);
        void TypeText(string text, float printDelay = -1, UnityAction finish = null);
        void SetTypeTextSpeed(float printDelay);

        Text Text { get; }

        FontStyle fontStyle { get; set; }

        TextAnchor alignment { get; set; }
        float preferredHeight { get;}
        float preferredWidth { get; }
    }
}
