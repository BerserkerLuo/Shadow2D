using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.UI.UICommon;
using UnityEngine;

namespace UILib.Export
{
    public class SafeXUIObject
    {
        public IXUIObject UIObject 
        {
            get { return m_uiObject; }
            private set { m_uiObject = value; }
        }

        public GameObject Obj
        {
            get
            {
                if (null != m_uiObject)
                {
                    return m_uiObject.CachedGameObject;
                }
                return null;
            }
        }

        private SafeXUIObject(IXUIObject uiObject)
        {
            m_uiObject = uiObject;
        }

        public static SafeXUIObject GetSafeXUIObject(IXUIObject uiObject)
        {
            if (null == uiObject)
            {
                return null;
            }
            SafeXUIObject safeXUIObject = null;
            if (true == s_dicAllSafeXUIObject.TryGetValue(uiObject, out safeXUIObject))
            {
                return safeXUIObject;
            }
            safeXUIObject = new SafeXUIObject(uiObject);
            s_dicAllSafeXUIObject.Add(uiObject, safeXUIObject);
            return safeXUIObject;
        }
        
        public static void OnDestoryXUIObject(IXUIObject uiObject)
        {
            SafeXUIObject safeXUIObject = null;
            if (s_dicAllSafeXUIObject.TryGetValue(uiObject, out safeXUIObject) == true)
            {
                safeXUIObject.UIObject = null;
                s_dicAllSafeXUIObject.Remove(uiObject);
            }
        }

        private IXUIObject m_uiObject = null;
        private static Dictionary<IXUIObject, SafeXUIObject> s_dicAllSafeXUIObject = new Dictionary<IXUIObject,SafeXUIObject>();
    }
}
