using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI.UICommon
{
    public interface IXUILoopList : IXUIObject
    {
        Int32 TotalCount { get; }
        int StartIndex { get; }
        int EndIndex { get; }
        Transform ItemsRoot { get; }
        Vector2 NormalizedPosition { get; set; }
        void Init(int dataCount, UIEvent updateCellCB, float cellPadding = 0f);
        void AddListItem(int nCount = 1);
        void Reposition(int index=0);
        void ResetSize(int dataCount);
        void RegisterScrollValueChange(UIEvent uiEvent);
        void RegisterScrollDragEnd(UIEvent uiEvent);
        void MoveToIndex(int index);
        void UpdateList(int index = -1);
        float GetDragBottomPixel();
        void StopMovement();
    }
}

