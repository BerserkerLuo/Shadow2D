using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.UI.UICommon;
using DG.Tweening;

namespace Client.UI.UICommon
{
    public interface IXUIGroup : IXUIObject
    {
        void SetVisible(string strId, bool bVisible);

        void SetLabelText(string strId, string strText);
        void SetSprite(string strId, string abName, string resName);
        void SetFillAmount(string strId, float fValue);
        Tweener DoFillAmount(string strId, float fValue, float fDuration);
        void RegisterObjectClickEventHandler(string strId, UIEvent btnClickHandler);
        int id { get; set; }
    }
}
