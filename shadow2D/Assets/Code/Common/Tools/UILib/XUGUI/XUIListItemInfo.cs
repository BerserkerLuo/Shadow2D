using System;
using Client.UI.UICommon;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UILib.Export;

namespace UILib
{
    public class XUIListItemInfo  : XUIListItem
    {
        XUISprite sprite;
        XUITextPro textPro; 

        public override void Init() {
            base.Init();
            sprite = (XUISprite)GetUIObject("Icon");
            textPro = (XUITextPro)GetUIObject("Name");
        }

        public float originalHeight = 0;
        public float originalWidth = 0;

        public void SetSprite(string path) {
            sprite.SetVisible(path != "");
            if (path != "") sprite.SetSprite("", path,false,()=> {
                RectTransform rectTransform = sprite.CachedRectTransform;
                if (originalHeight < 0.01f) originalHeight = rectTransform.rect.height;
                if (originalWidth < 0.01f) originalWidth = rectTransform.rect.width;

                sprite.CachedImage.SetNativeSize();

                Rect rect = rectTransform.rect;
                float step = Mathf.Min(originalHeight / rect.height, originalWidth / rect.width);
                rectTransform.sizeDelta = new Vector2(rect.width * step,rect.height * step);
            });

            Debug.Log($"SetSprite {path}");
        }

        public void SetText(string text) {
            textPro.SetVisible(text != "");
            if (text != "") textPro.SetText(text);
        }
    }
}
