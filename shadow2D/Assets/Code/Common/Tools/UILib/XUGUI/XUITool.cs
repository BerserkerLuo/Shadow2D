using UnityEngine;
using System.Collections;
using Client.UI.UICommon;
using System.Collections.Generic;
using System;
using UILib.Export;

namespace UILib
{
    public class XUITool
    {

        public static XUITool Instance 
        {
            get 
            {
                if (null == s_instance)
                {
                    s_instance = new XUITool();
                }
                return s_instance; 
            } 
        }

        /// <summary>
        /// Returns the hierarchy of the object in a human-readable format.
        /// </summary>

        static public string GetHierarchy(GameObject obj)
        {
            string path = obj.name;

            while (obj.transform.parent != null)
            {
                obj = obj.transform.parent.gameObject;
                path = obj.name + "/" + path;
            }
            return "\"" + path + "\"";
        }

        /// <summary>
        /// Recursively set the game object's layer.
        /// </summary>
        static public void SetLayer(GameObject go, int layer)
        {
            go.layer = layer;

            Transform t = go.transform;

            for (int i = 0, imax = t.childCount; i < imax; ++i)
            {
                Transform child = t.GetChild(i);
                SetLayer(child.gameObject, layer);
            }
        }

        public static void GetAllUIObjects(Transform trans, ref Dictionary<string, XUIObjectBase> dicAllUIObject)
        {
            for (int nIndex = 0; nIndex < trans.childCount; ++nIndex)
            {
                Transform child = trans.GetChild(nIndex);
                XUIObjectBase uiObjectBase = child.GetComponent<XUIObjectBase>();
                if (null != uiObjectBase)
                {
                    if (dicAllUIObject.ContainsKey(uiObjectBase.name) == true)
                    {
                        Debug.LogError("m_dicId2UIObject.ContainsKey:" + GetHierarchy(child.gameObject));
                    }
                    dicAllUIObject[uiObjectBase.name] = uiObjectBase;
                }
                GetAllUIObjects(child, ref dicAllUIObject);
            }
        }

        static XUITool s_instance = null;
    }
}
