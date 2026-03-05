using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;

namespace Client.UI.UICommon
{
    public interface IXUISlider : IXUIObject
    {
        float Value { get; set; }
        float MinValue { get; set; }
        float MaxValue { get; set; }
        bool Interactable { get; set; }
        void RegisterValueChangeEventHandler(UIEvent eventHandler);
    }
}
