using UnityEngine;
using System.Collections;
using Client.UI.UICommon;
using UILib;
using UILib.Export;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;

public class XUIGroup : XUIObject, IXUIGroup
{
    public override void Init()
    {
        base.Init();

        WidgetFactory.FindAllUIObjects(transform, this, ref m_dicId2UIObject, DlgBehaviour);
        //foreach (XUIObjectBase uiObject in m_dicId2UIObject.Values)
        //{
        //    uiObject.parent = this;
        //    uiObject.RootCanvas = this.RootCanvas;
        //    //if (null != XUITool.Instance)
        //    //{
        //    //    //string strID = WidgetFactory.GetUIObjectId(uiObject);
        //    //    //string strTip = XUITool.Instance.GetTip(strID);
        //    //    //if (string.IsNullOrEmpty(strTip) == false)
        //    //    //{
        //    //    //    uiObject.Tip = strTip;
        //    //    //}
        //    //}

        //    if (uiObject.IsInited == false)
        //    {
        //        uiObject.Init();
        //    }
        //    //Debug.Log("here " + uiObject.name);
        //}
    }

    public void SetVisible(string strId, bool bVisible)
    {
        IXUIObject uiObject = GetUIObject(strId);
        if (null != uiObject)
        {
            uiObject.SetVisible(bVisible);
        }
    }

    public void SetLabelText(string strId, string strText)
    {
        IXUILabel uiLabel = GetUIObject(strId) as IXUILabel;
        if (null != uiLabel)
        {
            uiLabel.SetText(strText);
        }
    }

    public void SetSprite(string strId, string abName, string resName)
    {
        IXUISprite uSprite = GetUIObject(strId) as IXUISprite;
        if (null != uSprite)
        {
            uSprite.SetSprite(abName, resName);
        }
    }

    public void SetFillAmount(string strId, float fValue)
    {
        IXUISprite uSprite = GetUIObject(strId) as IXUISprite;
        if (null != uSprite)
        {
            uSprite.SetFillAmount(fValue);
        }
    }

    public Tweener DoFillAmount(string strId, float fValue, float fDuration)
    {
        IXUISprite uSprite = GetUIObject(strId) as IXUISprite;
        if (null != uSprite)
        {
            Tweener tweener = uSprite.CachedTransform.GetComponent<Image>().DOFillAmount(fValue, fDuration);
            return tweener;
        }
        return null;
    }

    public void RegisterObjectClickEventHandler(string strId, UIEvent btnClickHandler)
    {
        IXUIObject uObject = GetUIObject(strId);
        if (null != uObject)
        {
            uObject.RegisterClickEventHandler(btnClickHandler);
        }
    }

    public override IXUIObject GetUIObject(string strPath)
    {
        if (null == strPath)
        {
            return null;
        }
        string strId = strPath;
        int nIndex = strPath.LastIndexOf('/');
        if (nIndex >= 0)
        {
            strId = strPath.Substring(nIndex + 1);
        }
        XUIObjectBase uiObject = null;
        if (true == m_dicId2UIObject.TryGetValue(strId, out uiObject))
        {
            return uiObject;
        }
        return null;
    }

    public Dictionary<string, XUIObjectBase> GetAllXUIObj()
    {
        return m_dicId2UIObject;
    }
    public int id
    {
        get { return m_nId; }
        set { m_nId = value; }
    }
    private int m_nId = 0;
    private Dictionary<string, XUIObjectBase> m_dicId2UIObject = new Dictionary<string, XUIObjectBase>();

}

