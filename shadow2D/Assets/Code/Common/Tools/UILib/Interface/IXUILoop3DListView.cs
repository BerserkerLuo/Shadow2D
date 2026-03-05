using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UILib;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Client.UI.UICommon
{
    public interface IXUILoop3DListView : IXUIObject
    {
        void Clear();
        void Init(int iTotalItemCount, Action<IXUIListItem, int> func);
        void RegistNearestChangeCallback(Action<IXUIListItem, int> funcSnapNearestChange);
        void RegistFinishedCallback(Action<IXUIListItem, int> funcSnapFinish);
        void SetItemCount(int iCount,bool bResetPos);
        void MoveToItemIndex(int iIndex);
        IXUIListItem[] ArrayViewItem { get; }
    }
}
