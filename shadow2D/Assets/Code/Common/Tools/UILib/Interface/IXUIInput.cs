using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

namespace Client.UI.UICommon
{
    public interface IXUIInput : IXUIObject,ISelectHandler
    {
        bool IsSelected { get; set; }
        bool Interactable { get; set; }
        string GetText();
        void SetText(string strText);
        void Select();
        void RegisterSubmitEventHandler(UIEvent eventHandler);
        void RegisterOnValueChanged(UIEvent eventHandler);
        void RegisterOnInputSelectEventHandler(UIEvent eventHandler);
    }
}
