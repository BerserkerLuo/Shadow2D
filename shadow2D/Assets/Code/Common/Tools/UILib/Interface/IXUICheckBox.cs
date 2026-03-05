using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.UI.UICommon
{
    //public delegate bool CheckBoxOnCheckEventHandler(IXUICheckBox iXUICheckBox);
    //public delegate bool ClickEventHandler(IXUIObject iXUIObject);

    public interface IXUICheckBox : IXUIObject
    {
        // Methods
        void RegisterOnCheckEventHandler(UIEvent eventHandler);
        //void RegisterClickEventHandler(ClickEventHandler eventHandler);
        // Properties
        bool bChecked { get; set; }
        object Data { get; set; }
    }
}
