using System;

namespace Client.UI.UICommon
{
    public delegate bool PopupListSelectEventHanler(IXUIPopupList iXUIPopupList);
    public interface IXUIPopupList : IXUIObject
    {
        int SelectedIndex { get; set; }
        string Selection { get; set; }

        bool AddItem(string strItem);
        bool AddItem(string strItem, object data);
        void Clear();
        object GetDataByIndex(int index);
        void RegisterPopupListSelectEventHandler(UIEvent eventHandler);
    }
}
