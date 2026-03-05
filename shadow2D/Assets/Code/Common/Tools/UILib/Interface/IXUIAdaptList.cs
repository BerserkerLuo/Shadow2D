using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client.UI.UICommon
{
    public interface IXUIAdaptList : IXUIObject
    {
        bool IsDragging { get; }
        int StartIndex { get; }
        int EndIndex { get; }
        int TotalCount { get; }
        void Init(int dataCount, UIEvent updateCellCB, int nOffset = 0, bool bFromEnd = false);
        void AddListItem(int nCount = 1, bool bAddToEnd = true);
        void RegisterScrollValueChange(UIEvent uiEvent);
        void RegisterScrollDragEnd(UIEvent uiEvent);
        Vector2 NormalizedPosition { get; }
        void MoveToLast();
        float GetDragBottomPixel();
        float GetDragTopPixel();
    }

}
