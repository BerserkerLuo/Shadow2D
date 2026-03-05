using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

namespace Client.UI.UICommon
{
    using ButtonClickEventHandler = Action<IXUIObject>;
    //public delegate bool ButtonDownEventHandler(IXUIObject iXUIButton);
    //public delegate bool ButtonUpEventHandler(IXUIObject iXUIButton);
    //public delegate bool ButtonEnterEventHandler(IXUIObject iXUIButton);
    //public delegate bool ButtonExitEventHandler(IXUIObject iXUIButton);


    public interface IXUIButton : IXUIObject
    {
        string additionalInfo { get; set; }
        int id { get; set; }
        long GUID { get; set; }
        object Data { get; set; }
        Image UiSpriteBG { get; set; }
        bool SetSprite(Sprite newSprite);
        void SetSprite(string abName, string spriteName);
        void SetSprite(string abName, string spriteName, bool bNativeSize = false, Action finish = null);
        void SetCaption(string strText);

        void SetSelect(bool flag);
    }
}
