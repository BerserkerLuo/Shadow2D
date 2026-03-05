using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UILib;

namespace Client.UI.UICommon
{
    public interface IXUIListItem : IXUIObject
    {
        UInt32 id { get; set; }
        UInt64 GUID { get; set; }
        object Data { get; set; }
        Int32 Index { get; }
        bool IsSelected { get; set; }
        Color HighlightColor { get; set; }
        void SetVisible(string strId, bool bVisible);
        void SetLabelText(string strId, string strText);
        void SetLabelColor(string strId, Color color);
        void SetSpriteColor(string strId, Color color);
        void SetSpriteColor(string strId, string htmlString);
        void SetSprite(string strId, string abName, string resName);
        void SetFillAmount(string strId, float fValue);
        void SetColor(Color color);
        void SetColor(string strId, Color color);
        void SetColor(string strId, string htmlString);
        void SetEnableSelect(bool bEnable);
        void SetChildGray(string strId, bool bGray);
        void RegisterObjectClickEventHandler(string strId, UIEvent btnClickHandler);
        void Clear();
        Dictionary<string, XUIObjectBase> GetAllXUIObj();
    }
}
