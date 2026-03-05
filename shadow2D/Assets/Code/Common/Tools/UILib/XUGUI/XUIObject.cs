using System;
using System.Collections.Generic;
using Client.UI.UICommon;
using UILib.Export;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UILib
{
    public class XUIObject : XUIObjectBase
    {
        public override Vector2 RelativeSize
        {
            get
            {
                if (null != CachedTransform)
                {
                    RectTransform rectTrans = CachedTransform as RectTransform;
                    if (null != rectTrans)
                    {
                        return rectTrans.sizeDelta;
                    }
                }
                return Vector2.zero;
            }
        }

        public override void Init()
        {
            base.Init();
        }

        public void SearchAllUIObject() {
            WidgetFactory.FindAllUIObjects(transform, this, ref m_dicId2UIObject, DlgBehaviour);
        }

        public override IXUIObject GetUIObject(string strPath)
        {
            if (m_dicId2UIObject.Count == 0 || null == strPath)
                return null;
            
            string strId = strPath;
            int nIndex = strPath.LastIndexOf('/');
            if (nIndex >= 0)
                strId = strPath.Substring(nIndex + 1);

            return m_dicId2UIObject.GetValueOrDefault(strId,null);
        }
        public Dictionary<string, XUIObjectBase> GetAllXUIObj()
        {
            return m_dicId2UIObject;
        }

        void OnDestroy() {
            foreach (IXUIObject uiObject in m_dicId2UIObject.Values){
                SafeXUIObject.OnDestoryXUIObject(uiObject);
            }
        }

        private Dictionary<string, XUIObjectBase> m_dicId2UIObject = new Dictionary<string, XUIObjectBase>();
    }
}
