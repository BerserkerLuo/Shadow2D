//CreatedBy zhanghaoyang 2018/1/23
using System;
using System.Collections.Generic;
using UnityEngine;
using UILib;

namespace Client.UI.UICommon
{
    public interface IXUIAnimator : IXUIObject
    {
        void Play(string stateName, [UnityEngine.Internal.DefaultValue("-1")] int layer, [UnityEngine.Internal.DefaultValue("float.NegativeInfinity")] float normalizedTime);
        void Play(string stateName);
    }
}