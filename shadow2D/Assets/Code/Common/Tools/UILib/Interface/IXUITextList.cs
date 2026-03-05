using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.UI.UICommon
{
    public interface IXUITextList : IXUIObject
    {
        int OffsetLine { get; set; }
        int TotalLine { get; }
        int MaxShowLine { get; }
        void Clear();
        void Add(string text);
    }
}
