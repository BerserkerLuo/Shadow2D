using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Client.UI.UICommon
{
    public interface IXUIProgress : IXUIObject
    {
        float MaxValue { get; set; }
        float Value { get; set; }
        void TweenValue(float targetValue, float fTime, float fDelay=0.0f);
        Color Color { get; set; }
    }
}
