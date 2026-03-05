//#define DEBUG_LOG
using System;
using System.Collections.Generic;
using Client.UI;
using Client.UI.UICommon;
using UILib.Export;
using UnityEngine;
using UnityEngine.UI;

namespace UILib
{
    public interface IStackDlg
    {
        void SetVisible(bool bVisible);
    }

    public class UIManager2
    {
        public static UIManager2 Singleton
        {
            get
            {
                if (null == s_instance)
                {
                    s_instance = new UIManager2();
                }
                return s_instance;
            }
        }

        public Camera UICamera
        {
            get { return m_uiCamera; }
        }


        public Canvas RootCanvas
        {
            get { return m_uiRootCanvas; }
        }

        public CanvasScaler RootCanvasScaler
        {
            get { return m_uiRootCanvasScaler; }
        }

        public void Init()
        {
            //GameObject objUICamera = GameObject.FindGameObjectWithTag("UICamera");


            //判断是3D后的UI还是3D前的UI

            m_uiCamera = UnityGameEntry.Instance.UICamera;//objUICamera.GetComponent("CameraObj") as CameraObj;

            GameObject objCanvasRoot = null;

            objCanvasRoot= UnityGameEntry.Instance.UIRoot.gameObject; //GameObject.Find("Canvas_Root");

            if(objCanvasRoot == null)
            {
                Debug.LogError("找不到Canvas_Root");
            }
            else
            {
                m_uiRootCanvas = objCanvasRoot.GetComponent<Canvas>();
                m_uiRootCanvasScaler = objCanvasRoot.GetComponent<CanvasScaler>();
            }
        }

        public bool AddDlg(IXUIDlg dlg)
        {
            //if (true == m_dicDlgs.ContainsKey(dlg.fileName))
            //{
            //    Debug.LogError("true == m_dicDlgs.ContainsKey(dlg.fileName): " + dlg.fileName);
            //    return false;
            //}

            //m_dicDlgs.Add(dlg.fileName, dlg);
            //m_listAllDlg.Add(dlg);
  
            return true;
        }

        public void OnDlgVisible(IXUIDlg uiDlg, bool bVisible)//逻辑
        {
 
        }


        public void OnDlgShow(IXUIDlg uiDlg)//表现
        {
            //TutorialTipManager.singleton.OnDlgShow(uiDlg);
        }

        public void OnDlgRefresh(IXUIDlg uiDlg)
        {
            //TutorialTipManager.singleton.OnDlgShow(uiDlg);
        }

        public virtual void OnDlgHide(IXUIDlg uiDlg)
        {

        }

        public virtual void OnDlgUptate()
        {
            for (int i = 0; i < m_listAllDlg.Count; ++i)//待完善
            {
                IXUIDlg dlg = m_listAllDlg[i];
                dlg._Update();
            }
        }

        public void PushStack(IStackDlg uiDlg)
        {
            if (null == uiDlg)
                return;

            int nGroup = 0;
            Stack<IStackDlg> stackDlg = null;
            if (m_dicStackDlg.TryGetValue(nGroup, out stackDlg) == false)
            {
                stackDlg = new Stack<IStackDlg>();
                m_dicStackDlg[nGroup] = stackDlg;
            }
            if (stackDlg.Count > 0)
            {
                IStackDlg topDlg = stackDlg.Peek();
                if (topDlg == uiDlg)
                {
                    return;//特殊逻辑（出栈导致）
                }
                stackDlg.Push(uiDlg);
                topDlg.SetVisible(false);
            }
            else
            {
                stackDlg.Push(uiDlg);
            }
        }

        public bool PopStack(IStackDlg uiDlg)
        {
            int nGroup = 0;
            Stack<IStackDlg> stackDlg = null;
            if (m_dicStackDlg.TryGetValue(nGroup, out stackDlg) == false)
            {
                stackDlg = new Stack<IStackDlg>();
                m_dicStackDlg[nGroup] = stackDlg;
            }

            if (stackDlg.Count > 0)
            {
                IStackDlg topDlg = stackDlg.Peek();
                if (topDlg == uiDlg)
                {
                    stackDlg.Pop();
                    if (stackDlg.Count > 0)
                    {
                        topDlg = stackDlg.Peek();
                        topDlg.SetVisible(true);
                    }
                    return true;
                }
                else
                {
                    //排除入栈情况
                }
            }
            return false;
        }

        public void ClearStack()//清除某个层级的显示堆栈
        {
            int nLayer = 0;
            Stack<IStackDlg> stackDlg = null;
            if (m_dicStackDlg.TryGetValue(nLayer, out stackDlg)== true)
            {
                stackDlg.Clear();
            }
        }

        //public void OnAddListItem(IXUIDlg uiDlg, IXUIList uiList, IXUIListItem uiListItem)
        //{
        //    TutorialTipManager.singleton.OnAddListItem(uiDlg, uiList, uiListItem);

        //}

        public void UnInit()
        {
            for (int i=0; i<m_listAllDlg.Count; ++i)//待完善
            {
                IXUIDlg dlg = m_listAllDlg[i];
                dlg.SetVisible(false);
                dlg.UnLoad();
            }
        }

        public void CloseAllDlg()
        {
            for (int i=0; i<m_listAllDlg.Count; ++i)
            {
                IXUIDlg dlg = m_listAllDlg[i];
                if (dlg.IsVisible() == true)
                {
                    dlg.SetVisible(false);
                }
            }
        }

        public void CloseAllDlg(uint unDlgType, uint unDlgTypeExclude)
        {
            for (int i=0; i<m_listAllDlg.Count; ++i)
            {
                IXUIDlg dlg = m_listAllDlg[i];
                if ((dlg.Type & unDlgType) > 0)
                {
                    if ((dlg.Type & unDlgTypeExclude) == 0)
                    {
                        dlg.SetVisible(false);
                    }
                }
            }
        }

        public void ShowAllDlg()
        {
            for (int i=0; i<m_listAllDlg.Count; ++i)
            {
                IXUIDlg dlg = m_listAllDlg[i];
                if (dlg.IsVisible() == false)
                {
                    dlg.SetVisible(true);
                }
            }
        }

        public void ShowAllDlg(uint unDlgType, uint unDlgTypeExclude)
        {
            for (int i=0; i<m_listAllDlg.Count; ++i)
            {
                IXUIDlg dlg = m_listAllDlg[i];
                if ((dlg.Type & unDlgType) > 0)
                {
                    if ((dlg.Type & unDlgTypeExclude) == 0)
                    {
                        dlg.SetVisible(true);
                    }
                }
            }
        }

        public void RefreshAllDlg(uint unDlgType, uint unDlgTypeExclude)
        {
            for (int i=0; i<m_listAllDlg.Count; ++i)
            {
                IXUIDlg dlg = m_listAllDlg[i];
                if ((dlg.Type & unDlgType) > 0)
                {
                    if ((dlg.Type & unDlgTypeExclude) == 0 && dlg.IsVisible() == true)
                    {
                        dlg.Refresh();
                    }
                }
            }
        }

        public void ResetAllDlg(uint unDlgType, uint unDlgTypeExclude)
        {
            for (int i=0; i<m_listAllDlg.Count; ++i)
            {
                IXUIDlg dlg = m_listAllDlg[i];
                if ((dlg.Type & unDlgType) > 0)
                {
                    if ((dlg.Type & unDlgTypeExclude) == 0)
                    {
                        dlg.ResetDlg();
                    }
                }
            }
        }

        public void UnLoadAllDlg(uint unDlgType, uint unDlgTypeExclude)
        {
            for (int i=0; i<m_listAllDlg.Count; ++i)
            {
                IXUIDlg dlg = m_listAllDlg[i];
                if ((dlg.Type & unDlgType) > 0)
                {
                    if ((dlg.Type & unDlgTypeExclude) == 0)
                    {
                        dlg.UnLoad();
                    }
                }
            }
        }

        public void LoadAllDlg(uint unDlgType, uint unDlgTypeExclude)
        {
            for(int i=0; i<m_listAllDlg.Count; ++i)
            {
                IXUIDlg dlg = m_listAllDlg[i];
                if ((dlg.Type & unDlgType) > 0)
                {
                    if ((dlg.Type & unDlgTypeExclude) == 0)
                    {
                        dlg.Load();
                    }
                }
            }
        }

        public void Compositor(IXUIDlg dlg)
        {
            //List<IXUIDlg> listDlg = null;
            //int index = 0;
            //foreach (int key in m_dicUILayer.Keys)
            //{
            //    if (key < dlg.layer)
            //    {
            //        index += m_dicUILayer[key].Count;
            //    }
            //}
            //if (m_dicUILayer.TryGetValue(dlg.layer, out listDlg) == true)
            //{
            //    //dlg.SetSiblingIndex(index);
            //    //dlg.SetTopInCanvas();

            //    //if (listDlg.IndexOf(dlg) != 0)
            //    //{
            //    //    listDlg.Remove(dlg);
            //    //    listDlg.Insert(0, dlg);
            //    //}

            //    ////int nDepthZ = 0;
            //    //foreach (IXUIDlg eachDlg in listDlg)
            //    //{
            //    //    if (eachDlg.IsVisible() == true)
            //    //    {
            //    //        //eachDlg.SetDepthZ(nDepthZ + 10 * dlg.layer);
            //    //        //++nDepthZ;
            //    //        ++index;
            //    //    }
            //    //}
            //    dlg.SetSiblingIndex(index);
        }

        public virtual void TipShow(bool bShow, IXUIObject uiObject)
        {

        }

        public IXUIDlg GetDlg(string strDlgName)
        {
            if (string.IsNullOrEmpty(strDlgName) == true)
            {
                return null;
            }

            IXUIDlg uiDlg = null;
            m_dicDlgs.TryGetValue(strDlgName, out uiDlg);
            return uiDlg;
        }


        public bool IsUIVisable(string filename)
        {
            bool result = false;
            if(m_dicDlgs.ContainsKey(filename))
            {
                result = m_dicDlgs[filename].IsVisible();
            }
            return result;
        }

        //Need To Do
        /// <summary>
        /// 通过"对话框名#控件名"去取得控件
        /// </summary>
        /// <param name="strUIObjectId"></param>
        /// <returns></returns>
        public IXUIObject GetUIObject(string strUIObjectId)
        {
            if (string.IsNullOrEmpty(strUIObjectId) == true)
            {
                return null;
            }

            string[] listName = strUIObjectId.Split('#');

            IXUIDlg uiDlg = GetDlg(listName[0]);
            if (null == uiDlg || null == uiDlg.uiBehaviourBase.CachedGameObject || listName.Length < 2)
            {
                return null;
            }
            string strName = listName[1];
            IXUIObject uiObject = uiDlg.uiBehaviourBase.GetUIObject(strName);
            try
            {
                for (int nIndex = 1; nIndex <listName.Length; nIndex++)
                {
                    strName = listName[nIndex];
                    if (nIndex > 1)
                    {
                        uiObject = uiObject.GetUIObject(strName);
                    }
                    IXUIList uiList = uiObject as IXUIList;
                    if (null != uiList)
                    {
                        ++nIndex;
                        if (nIndex < listName.Length)
                        {
                            UInt32 unItemId = Convert.ToUInt32(listName[nIndex]);
                            uiObject = uiList.GetItemById(unItemId);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogException(ex);
                Debug.LogError(string.Format("GetUIObject:{0}", strUIObjectId));
            }
            return uiObject;
        }

        public string GetUIObjectId(IXUIObject uiObject)
        {
            return WidgetFactory.GetUIObjectId(uiObject);
        }


        /// <summary>
        /// 根据目标对象和自适应对象计算出自适应对象的世界坐标
        /// </summary>
        /// <param name="target">目标对象</param>
        /// <param name="adaptTarget">自适应对象</param>
        /// <param name="gap">间距</param>
        /// <returns></returns>
        public Vector3 GetAdaptPos(RectTransform target,RectTransform adaptTarget,float gap = 0)
        {
            //目标屏幕坐标

            Vector3[] corners = new Vector3[4];
            target.GetWorldCorners(corners);//顺序是左下、左上、右上、右下
            for(int i=0; i<corners.Length; ++i)
            {
                corners[i] = m_uiCamera.WorldToScreenPoint(corners[i]);
            }
            //Vector3 targetScreenPos = m_uiCamera.WorldToScreenPoint(target.position);
            //屏幕缩放比
            float scaleFace = Screen.width / m_uiRootCanvasScaler.referenceResolution.x;
            ////目标真实宽度
            //float targetWidth = target.sizeDelta.x * scaleFace;
            ////目标真实高度
            //float targetHeight = target.sizeDelta.y * scaleFace;

            //自适应对象真实宽度
            float adaptTargetWidht = adaptTarget.sizeDelta.x * scaleFace;
            //自适应对象真实高度
            float adaptTargetHeight = adaptTarget.sizeDelta.y * scaleFace;

            Vector3 result = Vector3.zero;
            result.z = corners[0].z;

            //bool bRight = (Screen.width - targetScreenPos.x - (targetWidth / 2) - gap) >= adaptTargetWidht;
            bool bRight = (Screen.width - corners[2].x - gap) >= adaptTargetWidht;

            //bool bLeft = (targetScreenPos.x - (targetWidth / 2)) > adaptTargetWidht;
            bool bLeft = corners[0].x > adaptTargetWidht;

            //bool bBottom = (targetScreenPos.y - (targetHeight / 2)) > adaptTargetHeight;
            bool bBottom = corners[0].y  > adaptTargetHeight;

            //bool bTop = (Screen.height - targetScreenPos.y - (targetHeight / 2)) > adaptTargetHeight;
            bool bTop = (Screen.height - corners[1].y) > adaptTargetHeight;

            //右下 最优先
            if (bRight && bBottom)
            {
                result.x = corners[2].x + (adaptTargetWidht / 2) - gap;
                result.y = corners[2].y - (adaptTargetHeight / 2);
            }
            //右上
            else if (bRight && bTop)
            {
                result.x = corners[3].x + (adaptTargetWidht / 2) - gap;
                result.y = corners[3].y + (adaptTargetHeight / 2);
            }

            //左下
            else if (bLeft && bBottom)
            {
                result.x = corners[1].x - (adaptTargetWidht / 2) - gap;
                result.y = corners[1].y - (adaptTargetHeight / 2);
            }
            //左上
            else if (bLeft && bTop)
            {
                result.x = corners[0].x - (adaptTargetWidht / 2) - gap;
                result.y = corners[0].y + (adaptTargetHeight / 2);
            }
            //下
            else if (bBottom)
            {
                if(corners[3].x >= adaptTargetWidht)
                {
                    result.x = corners[3].x - (adaptTargetWidht / 2);
                }
                else
                {
                    result.x = Screen.width / 2;
                }
                result.y = corners[3].y - (adaptTargetHeight / 2) - gap;
            }
            //上
            else if (bTop)
            {
                if (corners[2].x >= adaptTargetWidht)
                {
                    result.x = corners[2].x - (adaptTargetWidht / 2);
                }
                else
                {
                    result.x = Screen.width / 2;
                }
                result.y = corners[2].y + (adaptTargetHeight / 2) + gap;
            }

            return m_uiCamera.ScreenToWorldPoint(result);
        }

        /// <summary>
        /// 根据UGUI的世界坐标拿到在UIRoot下的平面坐标
        /// </summary>
        /// <param name="uiWorldPos"></param>
        /// <returns></returns>
        public Vector2 GetUIInUIRootLocalPos(Vector3 uiWorldPos)
        {
            Vector2 result = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(m_uiRootCanvas.transform as RectTransform, UICamera.WorldToScreenPoint(uiWorldPos), UICamera, out result);
            return result;
        }

        public void SetGray(Image img,bool isGray)
        {
            img.material = isGray ? new Material(Shader.Find("UI/Gray")) : null;
        }

        public void SetCanvasRoots2Active(bool b)
        {
            //默认Canvas_Root2和 CameraUI2 的Active为 false
            //第一次添加时Canvas_Root2和 CameraUI2 的Active置为true
            if (m_ObjCanvasRoot2 == null)
            {
                #if DEBUG_LOG
                Debug.Log("m_ObjCanvasRoot2 == null");
                #endif
                return;
            }
            if (m_ObjCanvasRoot2.activeInHierarchy == b)
            {
                return;
            }
            #if DEBUG_LOG
            Debug.Log($"SetCanvasRoots2Active {b}");
            #endif

            //Canvas_Root2和 CameraUI2
            if (m_ObjCameraUI2) m_ObjCameraUI2.SetActive(b);
            m_ObjCanvasRoot2.SetActive(b);
        }

        public void SetBlurFeatureActive(bool b)
        {
            if (b == m_enableBlur) return;
            m_enableBlur = b;
            //UnityGameEntry.Instance.SetFeatureActive("KawaseBlur", m_enableBlur);
        }

        public void AddCanvasRoot2Dlg(IXUIDlg dlg)
        {
            if (dlg == null) return;

            SetCanvasRoots2Active(true);
            SetBlurFeatureActive(true);

            if (GetCanvas2DlgByFileName(dlg.fileName) == null)
            {
                m_listCanvas2Dlg.Add(dlg);
                if (IsUIBlur(dlg.uiBehaviourBase.CachedGameObject))
                {
                    AddBlurDlg(dlg);
                    CacheTopVisibleBlurDlg();
                }
            }
        }

        public void CheckInactiveCanvasRoot2()
        {
            bool inactive = true;
            for (int i = 0; i < m_listCanvas2Dlg.Count; i++)
            {
                IXUIDlg temp = m_listCanvas2Dlg[i];
                if (temp != null && temp.IsVisible())
                {
                    inactive = false;//只要Canvas2上有任何一个Dlg可见，都不禁用Canvas2
                    break;
                }
            }
            if (inactive)
            {
#if DEBUG_LOG
                Debug.Log("CheckInactiveCanvasRoot2 inactive");
#endif
                SetCanvasRoots2Active(false);
                SetBlurFeatureActive(false);
            }
        }

        public IXUIDlg GetCanvas2DlgByFileName(string filename)
        {
            IXUIDlg result = null;
            for (int i = 0; i < m_listCanvas2Dlg.Count; i++)
            {
                IXUIDlg temp = m_listCanvas2Dlg[i];
                if (temp != null && temp.fileName.Equals(filename))
                {
                    result = temp;
                    break;
                }
            }
            return result;
        }

        public IXUIDlg GetBlurDlgByFileName(string filename)
        {
            IXUIDlg result = null;
            for (int i = 0; i < m_listBlurDlg.Count; i++)
            {
                IXUIDlg temp = m_listBlurDlg[i];
                if (temp != null && temp.fileName.Equals(filename))
                {
                    result = temp;
                    break;
                }
            }
            return result;
        }

        public void AddBlurDlg(IXUIDlg dlg)
        {
            if (dlg == null) return;
            //防止重复添加
            if (GetBlurDlgByFileName(dlg.fileName) == null)
            {
                m_listBlurDlg.Add(dlg);
            }
        }

        public void RemoveBlurDlg(IXUIDlg dlg)
        {
            if (dlg == null) return;
            for (int i = 0; i < m_listBlurDlg.Count; i++)
            {
                if (dlg != null && m_listBlurDlg[i] == dlg)
                {
#if DEBUG_LOG
                    Debug.Log($"RemoveBlurDlg {dlg.fileName}");
#endif
                    m_listBlurDlg.RemoveAt(i);
                    return;
                }
            }
        }

        public bool IsUIBlur(GameObject obj)
        {
            return obj ? obj.CompareTag("UIBlur") : false;
        }

        //判断CanvasA是否在CanvasB之后渲染，layerA表示是否指定A的 sortingOrder
        public bool IsCanvasARenderAfterB(ref Canvas canvasA,ref Canvas canvasB, int layerA = int.MinValue)
        {
            if (canvasA && canvasB)
            {
                if (canvasA.cachedSortingLayerValue > canvasB.cachedSortingLayerValue)
                {
                    return true;
                }
                else if (canvasA.cachedSortingLayerValue == canvasB.cachedSortingLayerValue)
                {
                    if (layerA == int.MinValue) //目前这个参数只控制 sortingOrder
                    {
                        return canvasA.sortingOrder >= canvasB.sortingOrder;
                    }
                    return layerA >= canvasB.sortingOrder; //layer != 0
                }
            }
            return false;
        }

        public void CacheTopVisibleBlurDlg()
        {
            m_topUIBlurDlg = GetTopVisibleBlurDlg();
#if DEBUG_LOG
            Debug.Log($"m_topUIBlurDlg : {m_topUIBlurDlg}");
#endif

        }

        //查找最后渲染的可见模糊层
        public IXUIDlg GetTopVisibleBlurDlg()
        {
            //不存在可见的模糊层
            if (m_listBlurDlg.Count == 0) return null;

            IXUIDlg topDlg = null;
            Canvas topCanvas = null;
            for (int i = 0; i < m_listBlurDlg.Count; ++i)
            {
                IXUIDlg dlg = m_listBlurDlg[i];
                if (dlg != null && dlg.IsVisible())//只比对可见的模糊层
                {
                    DlgBehaviourBase behaviour = dlg.uiBehaviourBase;
                    if (behaviour?.CachedGameObject && behaviour.CachedGameObject.activeInHierarchy)
                    {
                        Canvas canvas = behaviour.canvas;
                        if (canvas && canvas.enabled)
                        {
                            //if (canvas.overrideSorting) //是否重写排序规则
                            //{
                            //
                            if (topCanvas == null || IsCanvasARenderAfterB(ref canvas, ref topCanvas))
                            {
                                topCanvas = canvas;
                                topDlg = dlg;
                            }
                            //其他渲染顺序更早的情况不需要赋值
                            //}
                        }
                    }
                }
            }
#if DEBUG_LOG
            if (topDlg != null && topCanvas != null)
                Debug.Log($"GetTopVisibleBlurDlg {topDlg.fileName} overrideSort: {topCanvas.overrideSorting} sortLayer: {topCanvas.cachedSortingLayerValue } sortOrder: {topCanvas.sortingOrder} ");
#endif
            return topDlg;
        }

  /*
    Canvas2存在的意义是使Canvas1的后处理画面作为模糊的背景，在Canva2上的Dlg先于模糊层渲染都是没有意义的，并不会出现在画面上，所以：
    标记为模糊的UI只能在Canvas2节点上最先渲染，其他任何渲染顺序在它之前的非模糊层都应该被移动到Canvas1上，这里渲染顺序由 
    界面是否启用，SortLayer大小，SortInLayer大小三个因素决定，Canvas2存在多个模糊层时以最上方的模糊层参数为准
*/
        public bool IfAddToCanvas2(DlgBehaviourBase dlgBehaviour, int layer = int.MinValue)
        {
            if (dlgBehaviour == null) return false;

            //模糊层必然放在Canvas2
            if (dlgBehaviour.CachedGameObject && IsUIBlur(dlgBehaviour.CachedGameObject))
            {
#if DEBUG_LOG
                Debug.Log($"IfAddToCanvas2 : {dlgBehaviour.CachedGameObject.name} UIBlur");
#endif
                return true;
            }
             
            if (m_topUIBlurDlg == null)
            {
#if DEBUG_LOG
                Debug.Log($"IfAddToCanvas2 : {dlgBehaviour.CachedGameObject.name} m_topUIBlurDlg == null");
#endif
                return false;
            }

            Canvas topCanvas = m_topUIBlurDlg.uiBehaviourBase?.canvas; 
            
            //topCanvas存在，表示启用了模糊层
            Canvas dlgCanvas = dlgBehaviour.canvas;
            if (topCanvas != null && dlgCanvas) 
            {
                bool b = IsCanvasARenderAfterB(ref dlgCanvas, ref topCanvas, layer);
#if DEBUG_LOG
                Debug.Log($"IfAddToCanvas2 : {dlgBehaviour.CachedGameObject.name} layer : {layer} renderAfterTopUIBlurDlg: {b}");
#endif
                return b;
            }
#if DEBUG_LOG
            Debug.Log($"IfAddToCanvas2 : {dlgBehaviour.CachedGameObject.name} false");
#endif
            return false;
        }

        //重新排列Canvas2上的Dlg，将UIBlur层之前渲染的Dlg移动到Canva1上
        public void RepostionCanvas2Dlgs()
        {
            if (m_listCanvas2Dlg.Count == 0)
            {
#if DEBUG_LOG
                Debug.Log("RepostionCanvas2Dlgs m_listCanvas2Dlg.Count == 0");
#endif
                return;
            }
            if (m_topUIBlurDlg == null)
            {
#if DEBUG_LOG
                Debug.Log("RepostionCanvas2Dlgs m_topUIBlurDlg == null");
#endif
                return;
            }
#if DEBUG_LOG
            Debug.Log("RepostionCanvas2Dlgs");
#endif

            Canvas topCanvas = m_topUIBlurDlg.uiBehaviourBase?.canvas;
            
            for (int i = m_listCanvas2Dlg.Count - 1; i > 0; --i)
            {
                IXUIDlg dlg = m_listCanvas2Dlg[i];
                if (dlg != null && dlg.IsVisible() && dlg.uiBehaviourBase !=null && !IsUIBlur(dlg.uiBehaviourBase.CachedGameObject))
                {
                    Canvas dlgCanvas = dlg.uiBehaviourBase.canvas;
                    if (IsCanvasARenderAfterB(ref dlgCanvas, ref topCanvas) == false)
                    {
                        Debug.LogWarning($"{dlg.fileName} 层级不应该在{m_topUIBlurDlg.fileName}层之前渲染，请调整调用顺序优先SetVisible UIBlur层 以防止出现意料之外的层级错误!!!");
                        MoveDlgToCanvasRoot1(dlg);
#if DEBUG_LOG
                        Debug.Log($"RemoveCanvasRoot2Dlg {dlg.fileName}");
#endif
                        m_listCanvas2Dlg.RemoveAt(i);
                    }
                }
            }
        }

        public void MoveDlgToCanvasRoot1(IXUIDlg dlg)
        {
            //if(dlg.Layer==LayerMask.NameToLayer("BackUI"))
            //    dlg.uiBehaviourBase.CachedRectTransform.SetParent(UnityGameEntry.Instance.UIRoot_Back);
            //else
                dlg.uiBehaviourBase.CachedRectTransform.SetParent(UnityGameEntry.Instance.UIRoot);


            dlg.uiBehaviourBase.IsOnCanvasRoot2 = false;//设置标记
        }

        private Camera m_uiCamera = null;
        private Canvas m_uiRootCanvas = null;
        private CanvasScaler m_uiRootCanvasScaler = null;

        private Dictionary<string, IXUIDlg> m_dicDlgs = new Dictionary<string, IXUIDlg>();
        //private Dictionary<int, List<IXUIDlg>> m_dicUILayer = new Dictionary<int, List<IXUIDlg>>();
        private List<IXUIDlg> m_listAllDlg = new List<IXUIDlg>();

        private Dictionary<int, Stack<IStackDlg>> m_dicStackDlg = new Dictionary<int, Stack<IStackDlg>>();
        private static UIManager2 s_instance = null;

        private GameObject m_ObjCameraUI2 = null;
        private GameObject m_ObjCanvasRoot2 = null;

        private List<IXUIDlg> m_listBlurDlg = new List<IXUIDlg>(); //Canvas2上的UIBlur Dlg
        private List<IXUIDlg> m_listCanvas2Dlg = new List<IXUIDlg>();//Canvas2上的所有 Dlg
        private IXUIDlg m_topUIBlurDlg = null;//渲染顺序最后的可见UIBlurDlg
        private bool m_enableBlur = false;//
    }
}
