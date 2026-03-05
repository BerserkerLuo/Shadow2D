using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI.UICommon
{
    public interface IXUIPicture : IXUIObject
    {
        Rect uvRect { get; set; }
        Color Color { get; set; }
        RawImage CachedImage { get; }
        Texture MainTexture { get; set; }
        void SetTexture(string strAbName, string strTextureFile, bool bNativeSize = false, Action finish = null);
        void SetTexture(Texture tex);

        int CalculateOpaquePixel();
        int ModifyPixels(Vector3 screenPos, int nBurshSize);

    }
}
