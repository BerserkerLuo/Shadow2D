/// <summary>
/// XUI button.
/// Copyright © 2012-2014 lybns
/// Any bugs/comments/suggestions, please contact to xuguangxiao@gmail.com
/// </summary>
using Client.UI.UICommon;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using ProjectX;
using Tool;
using UILib.Export;
using System.Collections.Generic;

namespace UILib
{
    using ButtonClickEventHandler = Action<IXUIObject>;
    public class XUIButton : XUIObject, IXUIButton,IPointerEnterHandler,IPointerExitHandler
    {
        public string additionalInfo
        {
            get
            {
                return m_additionalInfo;
            }
            set
            {
                m_additionalInfo = value;
            }
        }

        public int id
        {
            get
            {
                return m_nId;
            }
            set
            {
                m_nId = value;
            }
        }

        public object Data
        {
            get { return m_data; }
            set { m_data = value; }
        }

        public long GUID
        {
            get { return m_longId; }
            set { m_longId = value; }
        }

        public void SetCaption(string strText)
        {

            if (null != m_captionLabel)
            {
                m_captionLabel.text = strText;
            }
        }

        public Image UiSpriteBG
        {
            get { return m_uiSpriteBG; }
            set { m_uiSpriteBG = value; }
        }

        public bool SetSprite(Sprite newSprite)
        {
            m_uiSpriteBG.sprite = newSprite;
            return true;
        }

        public void SetSprite(string abName, string spriteName)
        {
            SetSprite(abName, spriteName, false);
        }

        public void SetSprite(string abName, string spriteName, bool bNativeSize = false, Action finish = null)
        {
            if (spriteName == "")
                return;

            abName = abName.ToLower(); spriteName = spriteName.ToLower();
            if (m_strAbName == abName && m_strSpriteName == spriteName && m_uiSpriteBG != null)
                return;

            ResourceMgr.Singleton.Load<Sprite>($"{abName}/{spriteName}", (sprite) => {
                if (null != sprite)
                    m_uiSpriteBG.sprite = sprite;

                if (true == bNativeSize)
                    m_uiSpriteBG.SetNativeSize();

                if (null != finish)
                    finish();
            });
        }

        public void SetSpriteNativeSize()
        {
            if (m_uiSpriteBG != null)
                m_uiSpriteBG.SetNativeSize();
        }

        public override void Init()
        {
            base.Init();

            SearchAllUIObject();

            m_captionLabel = GetComponentInChildren<Text>();
            if (null == m_captionLabel)
            {
                //Debug.Log("null == m_captionLabel");
            }

            m_uiSpriteBG = GetComponentInChildren<Image>();
            if (null == m_uiSpriteBG)
            {
                //Debug.Log("null == m_uiSpriteBG");
            }
        } 

        //=====================================================================================================================
        public void SetSelect(bool flag) {
            Transform trans = CachedRectTransform.Find("Push");
            if (trans == null)
                return;
            trans.gameObject.SetActive(flag);
        }

        //=====================================================================================================================

        public float scale = 1.1f;
        public GameObject EnterHidObj; 
        public void OnPointerEnter(PointerEventData eventData){
            CachedRectTransform.localScale = new Vector3(scale, scale, scale);
            if (EnterHidObj != null) EnterHidObj.SetActive(false);
        }

        public void OnPointerExit(PointerEventData eventData){
            CachedRectTransform.localScale = new Vector3(1f, 1f, 1f);
            if (EnterHidObj != null) EnterHidObj.SetActive(true);
        }
        //=====================================================================================================================
        private string m_strSpriteName = string.Empty;
        private string m_strAbName = string.Empty;
        private AssetRequest m_assetRequest = null;

        private Text m_captionLabel = null;
        private Image m_uiSpriteBG = null;      
        private int m_nId = 0;
        private long m_longId = 0;
        private object m_data;
        private string m_additionalInfo = string.Empty;
        //private CanvasGroup m_canvasGroup = null;
    }
}

