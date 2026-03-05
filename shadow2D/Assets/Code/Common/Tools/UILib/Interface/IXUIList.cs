using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Client.UI.UICommon
{
    public interface IXUIList : IXUIObject
    {
        Int32 Count { get; }
        Transform ItemsRoot { get; }
        void Clear();
        void Reposition();

        Vector2 GetNormalizedPosition();

        bool SetVerticalNormalizedPosition(float value);
        void SetVerticalLastPostion();

        void Refresh();

        IXUIListItem GetItemByGUID(ulong ulId);
        IXUIListItem GetItemById(UInt32 unId);
        IXUIListItem GetItemById(UInt32 unId, bool bVisible);
        IXUIListItem GetItemByIndex(Int32 nIndex);
        IXUIListItem GetItemByIndexOrAdd(Int32 nIndex);
        IXUIListItem[] GetAllItems();
        IXUIListItem AddListItem(GameObject obj);
        IXUIListItem AddListItem();
        void SetAllItemsVisible(bool active);
        bool DelItem(IXUIListItem iUIListItem);
        bool DelItemById(UInt32 unId);
        bool DelItemByIndex(Int32 nIndex);
        void Highlight(List<UInt32> listIds);
        void SetSize(float cellWidth, float cellHeight);
        void MoveToIndex(int index);
        void MoveTo(RectTransform childTrans);
        void SetItemVisibleCount(int count);
        void MoveToHead();
        void MoveToTail();

        void RegisterSelectStateChangeEventHandler(UIEvent btnClickHandler);
    }
}
