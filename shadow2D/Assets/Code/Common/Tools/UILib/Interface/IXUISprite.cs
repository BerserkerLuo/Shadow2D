using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI.UICommon
{
    public interface IXUISprite : IXUIObject
    {
        // Properties
        Image CachedImage { get; }
        int Id { get; set; }
        object Data { get; set; }
        Vector2 InitPos { get; }
        Color Color { get; set; }
        string spriteName { get; }
        Sprite sprite { set; }
        // Methods
        void SetEnable(bool bEnable);
        void SetColor(string htmlString);
        bool SetSprite(Sprite newSprite);
        void SetSprite(string path, string spriteName);
        void SetSprite(string path, string spriteName, bool bNativeSize=false, Action finish = null);
        void SetFillAmount(float amount);
        bool SetNativeSize();
        void SetSize(Vector2 size);
    }
}
