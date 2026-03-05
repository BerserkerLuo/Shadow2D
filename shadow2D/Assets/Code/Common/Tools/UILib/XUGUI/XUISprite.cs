using System;
using Client.UI.UICommon;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using ProjectX;
using Tool;

namespace UILib
{
    [AddComponentMenu("XUI/XUISprite")]
    public class XUISprite : XUIObject,IXUISprite
    {
        public Image CachedImage
        {
            get { return m_image; }
        }

        public int Id
        {
            get { return m_nId; }
            set { m_nId = value; }
        }

        public object Data
        {
            get { return m_data; }
            set { m_data = value; }
        }

        public Vector2 InitPos
        {
            get { return m_initPos; }
        }

        // Properties
        public override float Alpha
        {
            get
            {
                if (null != this.m_image)
                {
                    return this.m_image.color.a;
                }
                return 0f;
            }
            set
            {
                if (null != this.m_image)
                {
                    this.m_image.color = new Color(m_image.color.r, m_image.color.g, m_image.color.b, value);
                }
            }
        }

        public Color Color
        {
            get
            {
                if (null != this.m_image)
                {
                    return this.m_image.color;
                }
                return Color.white;
            }
            set
            {
                if (null != this.m_image)
                {
                    this.m_image.color = value;
                }
            }
        }

        public void SetColor(string htmlString)
        {
            Color color;
            ColorUtility.TryParseHtmlString(htmlString, out color);

            if (null != m_image && color != null)
            {
                m_image.color = color;
            }
        }

        public string spriteName
        {
            get
            {
                if (null != this.m_image)
                {
                    return this.m_image.sprite.name;
                }
                return null;
            }
        }

        public Sprite sprite
        {
            set{
                SetSprite(value);
            }

            get {
                if (m_image != null && CachedGameObject != null)
                    return m_image.sprite;
                return null;
            }
        }

        // Methods
        public override void Init()
        {
            base.Init();
            this.m_image = base.GetComponent<Image>();
            if (null == this.m_image)
            {
                string strPath = XUITool.GetHierarchy(this.gameObject);
                Debug.LogError("null == image:" + strPath);
            }
            m_initPos = CachedRectTransform.anchoredPosition;
        }

        public override void SetEnable(bool bEnable){
            if (null != m_image){
                m_image.enabled = bEnable;
            }
        }

        public bool SetSprite(Sprite newSprite){
            if(m_image != null && CachedGameObject != null)
                m_image.sprite = newSprite;
            return true;
        }

        public void SetSprite(string path, string spriteName)
        {
            SetSprite(path, spriteName, false);
        }

        public void SetSprite(string path)
        {
            SetSprite("", path, false);
        }

        public void SetSprite(string path, string spriteName, bool bNativeSize = false, Action finish = null)
        {
            if (spriteName == "")
                return;

            ResourceMgr.Singleton.Load<Sprite>(spriteName,(sprite) => {
                if (null != sprite)
                     m_image.sprite = sprite;

                 if (true == bNativeSize){
                     AutoSetSize(sprite);
                     //m_image.SetNativeSize();
                 }

                if (null != finish)
                    finish();
            });
        }

        public float originalHeight = 0;
        public float originalWidth = 0;
        public void AutoSetSize(Sprite sprite) {
            RectTransform rectTransform = CachedRectTransform;
            if (originalHeight < 0.01f) originalHeight = rectTransform.rect.height;
            if (originalWidth < 0.01f) originalWidth = rectTransform.rect.width;

            CachedImage.SetNativeSize();

            Rect rect = rectTransform.rect;
            float step = Mathf.Min(originalHeight / rect.height, originalWidth / rect.width);
            rectTransform.sizeDelta = new Vector2(rect.width * step, rect.height * step);
        }


         public bool SetNativeSize()
        {
             if(null != this.m_image)
             {
                 this.m_image.SetNativeSize();
                 return true;
             }
             return false;
        }

        public void SetSize(Vector2 size)
        {
            if (null != m_image)
            {
                CachedRectTransform.sizeDelta = size;
                m_image.SetAllDirty();
            }
        }

        public void SetFillAmount(float amount)
        {
            if (null != this.m_image)
            {
                m_image.fillAmount = amount;
            }
        }

        void OnDestroy()
        {
            if (string.IsNullOrEmpty(m_strAbName) == false)
            {
                m_strAbName = string.Empty;
                m_strSpriteName = string.Empty;
            }

            if (null != m_assetRequest)
            {
                m_assetRequest.Dispose();
                m_assetRequest = null;
            }
        }

        // Fields
        private int m_nId;
        private object m_data;
        private string m_spriteNameCached = string.Empty;
        private Image m_image;
        private string m_strSpriteName = string.Empty;
        private string m_strAbName = string.Empty;
        private AssetRequest m_assetRequest = null;
        private Vector2 m_initPos;
    }

}