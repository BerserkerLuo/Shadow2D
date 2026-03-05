using System;
using UnityEngine;
using UILib;
using System.Collections;
using UnityEngine.UI;
using ProjectX;
using Game;
using System.Collections.Generic;

namespace Client.UI.UICommon
{
    enum EnumDlgType
    {
        eDlgType_Login = 1 << EnumGameState.estate_Login,
        eDlgType_Lobby = 1 << EnumGameState.eState_Lobby,
        eDlgType_Battle = 1 << (EnumGameState.eState_Battle),
        eDlgType_AllState = 0xFFFFFF,//can be visible in all state
    }

    public enum ECanvasLayer
    {
        eNormal,
        eTop
    }

    public interface IStackDlg
    {
        void SetVisible(bool bVisible);
    }

    public class IXUIDlg : IStackDlg
    {
        public virtual DlgBehaviourBase uiBehaviourBase { get; }
        public virtual string fileName { get; }
        public virtual uint Type { get; }
        public virtual int Layer { get; }
        public virtual bool EnableFilter { get; set; }
        public virtual void _Init() { }
        public virtual void _ReInit() { }
        public virtual void _Update() { }
        public virtual void _LateUpdate() { }
        public virtual AssetRequest Load() { return null; }
        public virtual bool UnLoad() { return true; }
        //void SetVisible(bool bVisible);
        public virtual bool IsVisible() { return true; }
        public virtual void ResetDlg() { }
        public virtual void Refresh() { }
        public virtual void SetSiblingIndex(int index) { }

        public virtual void SetVisible(bool bVisible){}
    }

    public class DlgBaseNew : IXUIDlg
    {
        public DlgBaseNew()
        {

        }

        public override string fileName
        {
            get { return m_uiBehaviour.FileName; }
        }

        public override int Layer
        {
            get { return 0; }
        }

        public override DlgBehaviourBase uiBehaviourBase
        {
            get { return m_uiBehaviour; }
        }

        public bool Prepared
        {
            get { return (null != m_uiBehaviour.CachedGameObject); }
        }

        public override uint Type
        {
            get { return (int)EnumDlgType.eDlgType_Lobby; }
        }

        public virtual bool IsPersist
        {
            get { return false; }
        }

        public virtual bool CanClear
        {
            get { return true; }
        }

        //是否启用引导交互限制
        public override bool EnableFilter
        {
            get { return m_uiBehaviour.EnableFilter; }
            set { m_uiBehaviour.EnableFilter = value; }
        }

        private bool m_bNeedReint = false;

        public string GetUIObjectId(string strId)
        {
            return string.Format("{0}#{1}", fileName, strId);
        }

        public override void _Init()
        {
            //try
            {
                m_bNeedReint = false;
                Init();
                RegisterEvent();
                m_uiBehaviour.SetVisible(m_bVisible, m_bReal);
                if (true == m_bVisible)
                {
                    //OnShow();//同步加载需注释  异步加载可以解开
                    //UIManager.Singleton.OnDlgShow(this);
                }


            }
            //catch (System.Exception ex)
            //{
            //    Debug.LogError(ex.ToString());
            //}
        }

        public override void _Update()
        {
            try
            {
                if (m_bVisible == true)
                {
                    this.Update();
                }
            }
            catch (Exception exception)
            {
                Debug.LogError(exception.ToString());
            }
        }

        public override void _LateUpdate()
        {
            //try
            {
                if (m_bVisible == true)
                {
                    LateUpdate();
                }
            }
            //catch (Exception exception)
            //{
            //    Debug.LogError(exception.ToString());
            //}
        }

        public virtual void Update()
        {

        }


        public virtual void LateUpdate()
        {

        }

        public void ToggleVisible() {
            SetVisible(!m_bVisible);
        }


        public override void SetVisible(bool bIsVisible){
            SetVisible(bIsVisible, true);
            if (true == bIsVisible && m_uiBehaviour != null && m_uiBehaviour.CanvasGroup != null && m_bFade == false)
            {
                m_uiBehaviour.CanvasGroup.alpha = 0;
                //Tweener tweener = m_uiBehaviour.CanvasGroup.DOFade(1.0f, 2.0f);
                //tweener.SetEase(Ease.Linear);
                //tweener.SetAutoKill(true);
                m_bFade = true;
            }
        }

        public virtual void SetVisible(bool bIsVisible, bool bReal)
        {
            if (m_bVisible == bIsVisible && m_bReal == bReal)
                return;

            bool oldVisible = m_bVisible;
            m_bVisible = bIsVisible;
            m_bReal = bReal;
            UIManager uiMgr = UIManager.Singleton;
            uiMgr.OnDlgVisible(this, m_bVisible);

            if (true == m_bVisible) {
                Load();
                uiMgr.Compositor(this);
            }
            else {
                UnLoad();
            }

            if (true == Prepared) {
                m_uiBehaviour.SetVisible(m_bVisible, bReal);

                if (!m_bVisible && oldVisible == true)
                {
                    uiMgr.OnDlgHide(this);
                    OnHide();
                }
            }


            if (true == m_bVisible && true == Prepared)
            {
                //Reset之后，然后再打开,执行Reinit
                if (m_bNeedReint)
                {
                    _ReInit();
                }
                OnShow();
                uiMgr.OnDlgShow(this);
            }
        }


        //animation scale from 1 to 0
        public void GoOut(float time, float delay = 0.0f)
        {
            GraphicRaycaster raycaster = m_uiBehaviour.CachedGameObject.GetComponent("GraphicRaycaster") as GraphicRaycaster;
            raycaster.enabled = false;
            m_uiBehaviour.CachedRectTransform.localScale = Vector3.one;
            UnityGameEntry.Instance.StartCoroutine(GoInOrOutCorout(m_uiBehaviour.CachedGameObject, 0, time, delay, () =>
            {
                m_uiBehaviour.CachedRectTransform.localScale = Vector3.one;
                raycaster.enabled = true;
                SetVisible(false);

            }));

        }

        //animation scale from 0 to 1
        public void GoIn(float time, float delay = 0.0f)
        {
            SetVisible(true);
            m_uiBehaviour.CachedRectTransform.localScale = Vector3.zero;
            UnityGameEntry.Instance.StartCoroutine(GoInOrOutCorout(m_uiBehaviour.CachedGameObject, 1, time, delay, () =>
            {
                m_uiBehaviour.CachedRectTransform.localScale = Vector3.one;
                //SetVisible(true);
            }));

        }

        public void RegisterTips(IXUIObject uiObject,string tipContent, float tipsTime = 0.8f) {
            uiObject.Tips = tipContent;
            uiObject.RegisterShowTipsEvent(OnShowTips, tipsTime);
            uiObject.RegisterHideTipsEvent(OnHideTips);
        }
         
        public bool OnShowTips(IXUIObject uiObject) {
            UIManager.Singleton.ShowTips(uiObject.Tips);
            return true;
        }

        public bool OnHideTips(IXUIObject uiObject){
            UIManager.Singleton.HideTips();
            return true;
        }

        //do the animation scale
        IEnumerator GoInOrOutCorout(GameObject obj, float scale, float time, float delay, Action callback)
        {
            obj.SetActive(true);

            yield return new WaitForSeconds(delay);

            var originalScale = obj.transform.localScale;
            var targetScale = Vector3.one * scale;
            var originalTime = time;

            while (time > 0.0f)
            {
                time -= Time.deltaTime;
                obj.transform.localScale = Vector3.Lerp(targetScale, originalScale, time / originalTime);
                yield return 0;
            }

            if (callback != null)
                callback();
        }


        protected virtual void OnShow()
        {
            m_uiBehaviour.EnableRaycaster(true);
            if (true == m_bNeedToRefresh)
            {
                m_bNeedToRefresh = false;
                OnRefresh();
                //UIManager.Singleton.OnDlgRefresh(this);
            }
        }

        protected virtual void OnHide() { 
        }

        public override void Refresh()
        {
            //try
            {
                if (true == Prepared && m_bVisible == true)
                {
                    OnRefresh();
                    //UIManager.Singleton.OnDlgRefresh(this);
                }
                else
                {
                    m_bNeedToRefresh = true;
                }
            }
            //catch (System.Exception ex)
            //{
            //    Debug.LogError(ex.ToString());
            //}
        }

        protected virtual void OnRefresh()
        {

        }

        public override bool IsVisible()
        {
            return m_bVisible;
        }

        public bool IsMouseIn()
        {
            return (this.Prepared && this.m_uiBehaviour.IsMouseIn());
        }

        //public void SetDepthZ(int nDepthZ)
        //{
        //    m_fDepthZ = nDepthZ * 10; // Max for 48
        //    if (true == Prepared)
        //    {
        //        Vector3 localPos = uiBehaviour.transform.localPosition;
        //        localPos.z = m_fDepthZ;
        //        uiBehaviour.transform.localPosition = localPos;
        //    }
        //}

        public override void SetSiblingIndex(int index)
        {
            if (this.Prepared == true)
                m_uiBehaviour.SetSiblingIndex(index);
        }

        public void SetAsFirstSibling()
        {
            if (this.Prepared == true)
                m_uiBehaviour.SetAsFirstSibling();
        }

        public void SetAsLastSibling()
        {
            if (this.Prepared == true)
                m_uiBehaviour.SetAsLastSibling();
        }

        public override void ResetDlg()
        {
            m_bNeedReint = true;
            Reset();
        }

        public override void _ReInit()
        {
            ReInit();
            m_bNeedReint = false;
        }

        public virtual void Init()
        {

        }
        
        public virtual void ReInit()
        {
            
        }

        public virtual void Reset()
        { 
            
        }

        public override AssetRequest Load()
        {
            if (m_bLoaded)
                return m_assetRequest;

            m_bLoaded = true;

            GameObject obj = Resources.Load<GameObject>(string.Format("UI/{0}/{1}", m_uiBehaviour.AbName.ToLower(), m_uiBehaviour.FileName));
            if(obj != null)
                this.OnLoadRes(obj);


            //m_assetRequest = ResourceMgr.Singleton.LoadPrefab(string.Format("UI/{0}", m_uiBehaviour.AbName.ToLower()), m_uiBehaviour.FileName.ToLower(), (AssetRequest assetRequest) =>
            //{
            //    if (assetRequest.objs != null && assetRequest.objs.Length > 0)
            //    {
            //        GameObject obj = assetRequest.objs[0] as GameObject;
            //        //GameObject.DontDestroyOnLoad(obj);
            //        this.OnLoadRes(obj);
            //    }
            //    else
            //    {
            //        Debug.LogError("objs == null || objs.Length == 0:" + fileName);
            //    }

            //});

            return null;
        }

        private void OnLoadRes(GameObject obj)
        {
            GameObject objUI = null;
            if (null != obj)
            {
                objUI = GameObject.Instantiate(obj) as GameObject;
            }
            else
            {
                objUI = new GameObject(fileName);
                Debug.LogError(string.Format("null == assetResource: {0}", fileName));
            }

            objUI.SetActive(true);

            if (null != objUI)
            {
                //GameObject.DontDestroyOnLoad(objUI);
                objUI.name = fileName;

                m_uiBehaviour.Dlg = this;
                m_uiBehaviour.Init(objUI, Layer);

                _Init();
            }
            //}
            //catch (System.Exception ex)
            //{
            //    Debug.LogException(ex);
            //}
        }

        public virtual bool Close()
        {
            if (true == Prepared && m_bVisible == true)
            {
                m_uiBehaviour.EnableRaycaster(false);

                OnClose();
                return true;
            }
            return false;
        }

        protected virtual void OnClose()
        {
            SetVisible(false);
        }

        public override bool UnLoad()
        {
            //try
            {
                if (Prepared == true && IsVisible() == false)
                {
                    //try
                    {
                        OnUnLoad();
                    }
                    //catch (System.Exception ex)
                    //{
                    //    Debug.LogError(ex.ToString());
                    //}
                    GameObject obj = m_uiBehaviour.CachedGameObject;
                    m_uiBehaviour.Destroy();
                    GameObject.Destroy(obj);
                    m_bLoaded = false;
                    if (null != m_assetRequest)
                    {
                        m_assetRequest.Dispose();
                        m_assetRequest = null;
                    }
                    return true;
                }
            }
            //catch (System.Exception ex)
            //{
            //    Debug.LogError(ex.ToString());	
            //}
            return false;
        }

        protected virtual void OnUnLoad(){}
        public virtual void RegisterEvent(){ }

        public List<GameObject> GetUIUnderPos(Vector2 pos) {
            return uiBehaviourBase.GetUIUnderPos(pos);
        }

        protected DlgBehaviourBase m_uiBehaviour = null;
        private static object s_objLock = new object();
        protected bool m_bVisible = false;
        private bool m_bReal = false;//是否真的SetActive?
        protected bool m_bLoaded = false;
        private bool m_bFade = false;

        private bool m_bNeedToRefresh = false;

        private AssetRequest m_assetRequest = null;
    }
}
