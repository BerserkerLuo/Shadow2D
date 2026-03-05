using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.UI.UICommon
{
    public delegate bool ScrollBarChangeEventHandler(IXUIScrollBar iXUIScrollBar);
    public delegate bool ScrollBarDragFinishedEventHandler();
    public interface IXUIScrollBar : IXUIObject
    {
        float Value { get; set; }
        float Size { get; set; }
        void RegisterScrollBarChangeEventHandler(UIEvent eventHandler);
    }
}
