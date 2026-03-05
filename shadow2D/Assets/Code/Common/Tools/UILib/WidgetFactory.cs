using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.UI.UICommon;
using UnityEngine;
using UILib;

namespace UILib.Export
{
    public class WidgetFactory
    {
        static WidgetFactory()
        {
            s_dicAllErrorWidget.Add(typeof(IXUIObject), XObject.Error);
            s_dicAllErrorWidget.Add(typeof(IXUIButton), XButton.Error);
            s_dicAllErrorWidget.Add(typeof(IXUICheckBox), XCheckBox.Error);
            s_dicAllErrorWidget.Add(typeof(IXUIGroup), XGroup.Error);
            s_dicAllErrorWidget.Add(typeof(IXUIInput), XInput.Error);
            s_dicAllErrorWidget.Add(typeof(IXUILabel), XLable.Error);
            s_dicAllErrorWidget.Add(typeof(IXUIList), XList.Error);
            s_dicAllErrorWidget.Add(typeof(IXUILoopList), XLoopList.Error);
            s_dicAllErrorWidget.Add(typeof(IXUIListItem), XListItem.Error);
            s_dicAllErrorWidget.Add(typeof(IXUIPicture), XPicture.Error);
            s_dicAllErrorWidget.Add(typeof(IXUIPopupList), XPopupList.Error);
            s_dicAllErrorWidget.Add(typeof(IXUIProgress), XProgress.Error);
            s_dicAllErrorWidget.Add(typeof(IXUIScrollBar), XScrollBar.Error);
            s_dicAllErrorWidget.Add(typeof(IXUISlider), XSlider.Error);
            s_dicAllErrorWidget.Add(typeof(IXUISprite), XSprite.Error);
            s_dicAllErrorWidget.Add(typeof(IXUITextList), XTextList.Error);
            s_dicAllErrorWidget.Add(typeof(IXUIRichLabel), XRichLable.Error);
        }

        public static T CreateWidget<T>() where T : class, IXUIObject
        {
            IXUIObject widget = null;
            if (s_dicAllErrorWidget.TryGetValue(typeof(T), out widget) == true)
            {

            }
            
            return widget as T;
        }

        public static void FindAllUIObjects(Transform trans, IXUIObject parent, ref Dictionary<string, XUIObjectBase> dicAllUIObject, object dlgBehaviour)
        {
            for (int nIndex = 0; nIndex < trans.childCount; ++nIndex)
            {
                Transform child = trans.GetChild(nIndex);
                XUIObjectBase uiObjectBase = child.GetComponent<XUIObjectBase>();
                if (null != uiObjectBase)
                {
                    if (uiObjectBase.GetType().GetInterface("IXUIListItem") != null){
                        continue;
                    }

                    uiObjectBase.parent = parent;
                    uiObjectBase.DlgBehaviour = dlgBehaviour;

                    if (uiObjectBase.IsInited == false)
                    {
                        uiObjectBase.Init();
                    }

                    if (dicAllUIObject.ContainsKey(uiObjectBase.name) == true)
                    {
                        //Debug.LogError("m_dicId2UIObject.ContainsKey:" + GetUIObjectId(uiObjectBase));
                    }
                    dicAllUIObject[uiObjectBase.name] = uiObjectBase;
                    uiObjectBase.parent = parent;
                    //uiObjectBase.RootCanvas = parent.RootCanvas;
                    if (typeof(IXUIGroup).IsAssignableFrom(uiObjectBase.GetType()) == true)
                    {
                        continue;
                    }
                }


                FindAllUIObjects(child, parent, ref dicAllUIObject, dlgBehaviour);
            }
        }

        public static string GetUIObjectId(IXUIObject uiObject)
        {
            if (null == uiObject)
            {
                return string.Empty;
            }
            string strUIObjectId = uiObject.CachedGameObject.name;
            IXUIListItem uiListItem = uiObject as IXUIListItem;
            if (uiListItem != null)
            {
                strUIObjectId = uiListItem.Index.ToString();
            }


            while (null != uiObject.parent)
            {
                uiObject = uiObject.parent;
                string strTempId = uiObject.CachedGameObject.name;
                uiListItem = uiObject as IXUIListItem;
                if (null != uiListItem)
                {
                    strTempId = uiListItem.Index.ToString();
                }
                strUIObjectId = string.Format("{0}#{1}", strTempId, strUIObjectId);
            }
            return strUIObjectId;
        }

        private static Dictionary<Type, IXUIObject> s_dicAllErrorWidget = new Dictionary<Type,IXUIObject>();
    }
}
