using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools;
using UnityEngine;
using UnityEngine.Networking;

namespace ProjectX
{
    class AssetBundleInfo
    {
        public delegate void LoadComplete(AssetBundleInfo assetSO);

        //public AssetPRI PRIType
        //{
        //    get { return mPRIType; }
        //    set { mPRIType = value; }
        //}

        public LoadComplete CallBackLoadCompleteFinishForDepend
        {
            get { return m_callbackLoadCompleteFinishForDepend; }
            set { m_callbackLoadCompleteFinishForDepend = value; }
        }

        public bool IsSelfDone
        {
            get{ return m_bSelfDone; }
        }

        public bool HasCallBacked
        {
            get
            {
                return m_bHasCallBacked;
            }
        }

        public AssetBundle AssetBundle
        {
            get { return m_assetBundle; }
        }

        public int RefCount
        {
            get { return m_nRefCount; }
        }

        public float progress
        {
            get
            {
                if (m_assetBundle != null)
                {
                    return 1.0f;
                }
                return 0.0f;
            }
        }

        public int Size
        {
            get
            {
                return 0;
            }
        }

        public bool IsStart
        {
            get { return m_bStarted; }
        }

        public string AbName { get { return m_strAbName; } }

        public AssetBundleInfo(string name, uint unSize, Type type)
        {
            m_strAbName = name;
            m_unSize = unSize;
            m_type = type;
        }

        public void AddRequest(AssetRequest request)
        {
            if (m_hashSetRequest.Contains(request) == false)
            {
                m_hashSetRequest.Add(request);
                m_callbackLoadCompleteFinish += request.OnAssetBundleLoadFinished;
                AddRef();
            }
        }

        public void DelRequest(AssetRequest request)
        {
            if (m_hashSetRequest.Remove(request) == true)
            {
                m_callbackLoadCompleteFinish -= request.OnAssetBundleLoadFinished;
                DelRef();
            }
        }

        public void SetDepSize(int length)
        {
            if (length > 0)
            {
                if (this.m_DepAssets != null)
                {
                    Debug.LogError("this.m_DepAssets != null");
                }
                this.m_DepAssets = new AssetBundleInfo[length];
            }
        }

        public void AddDep(AssetBundleInfo assetResource, int index)
        {
            AssetBundleInfo depSO = assetResource;
            if ((((depSO != null) && (this.m_DepAssets != null)) && ((index < this.m_DepAssets.Length) && (index >= 0))) && !this.Contains(depSO))
            {
                this.m_DepAssets[index] = depSO;
                depSO.AddRef();
                if (depSO.IsComplete() == false)
                {
                    depSO.CallBackLoadCompleteFinishForDepend += OnDependLoadFinished;                //注册回调
                }
            }
        }

        public bool Contains(AssetBundleInfo asset)
        {
            if (this == asset)
            {
                return true;
            }
            if (this.m_DepAssets != null)
            {
                for (int i = 0; i < this.m_DepAssets.Length; i++)
                {
                    AssetBundleInfo ex = this.m_DepAssets[i];
                    if (asset == ex)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void Load()
        {
            m_bStarted = true;
            if (s_hashSet.Contains(m_strAbName) == true)// For Test
            {
                Debug.Log("s_hashSet.Contains(m_strName) == true: " + m_strAbName);
            }
            else
            {
                //加载
                // string assetBundlePath = string.Format("data/assets/res_assetbundle/{0}", m_strAbName);
                string assetBundlePath = string.Format("data/{0}", m_strAbName);
                string url = Util.GetPath(assetBundlePath.ToLower(), true);
                url = url.Replace("_ab", "");
                // Debug.Log("streamingAssetsPath:" + Application.streamingAssetsPath);
                // Debug.Log("AssetBundleInfo.Load:" + url);
                try
                {
                    Debug.Log("AssetResource Load: " + url);
                    m_assetBundle = AssetBundle.LoadFromFile(url);
                    Debug.Log("AssetResource Load ok: " + m_assetBundle.name);
                }
                catch (Exception e)
                {
                    Debug.LogError(e.StackTrace);
                }
                s_hashSet.Add(m_strAbName);
                m_bSelfDone = true;
            }
        }

        public void LoadAsync()
        {
            m_bStarted = true;
            CoroutineRunner.Instance.StartCoroutine(_LoadAsync());//改为同步测试看看
        }

        private IEnumerator _LoadAsync()
        {
            if (s_hashSet.Contains(m_strAbName) == true)// For Test
            {
                Debug.Log("s_hashSet.Contains(m_strName) == true: " + m_strAbName);
            }
            else
            {
                //加载
                string assetBundlePath = string.Format("data/{0}", m_strAbName);
                string url = Util.GetPath(assetBundlePath, true);
                Debug.Log("BeginDownLoad:" + url);

                AssetBundleCreateRequest assetBundleCreateRequest = AssetBundle.LoadFromFileAsync(url);

                s_hashSet.Add(m_strAbName);

                yield return assetBundleCreateRequest;
                m_assetBundle = assetBundleCreateRequest.assetBundle;
                m_bSelfDone = true;
            }

        }

        public void OnSelfDownLoadFinished()
        {
            Debug.Log("OnDownLoadFinished:" + m_strAbName);
            if (m_assetBundle == null)
            {
                Debug.LogError("m_assetBundle == null:" + m_strAbName);
            }

            CheckComplete();
        }

        private void OnDependLoadFinished(AssetBundleInfo depend)
        {
            CheckComplete();
        }

        private void CheckComplete()
        {
            if (false == IsComplete(true))
            {
                return;
            }

            DoFinishCallBack();
        }

        public void DoFinishCallBack()
        {
            if (null != m_callbackLoadCompleteFinishForDepend)
            {
                m_callbackLoadCompleteFinishForDepend(this);
                m_callbackLoadCompleteFinishForDepend = null;
            }

            if (null != m_callbackLoadCompleteFinish)
            {
                m_callbackLoadCompleteFinish(m_assetBundle);
                m_callbackLoadCompleteFinish = null;
            }
        }

        public bool IsComplete(bool bRecurse=false)
        {
            if (bRecurse == false)
            {
                return m_bSelfDone;
            }

            if (m_bSelfDone == false)
            {
                return false;
            }

            if (null != m_DepAssets)
            {
                for (int nIndex = 0; nIndex < m_DepAssets.Length; ++nIndex)
                {
                    AssetBundleInfo assetBundleInfo = m_DepAssets[nIndex];
                    if (assetBundleInfo == null)
                    {
                        return false;
                    }
                    if (assetBundleInfo.IsComplete() == false)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public int AddRef()
        {
            ++m_nRefCount;
            //Debug.Log(string.Format("AddRef[{0}]:{1}", m_nRefCount, m_strAbName));
            return m_nRefCount;
        }

        public int DelRef()
        {
            --m_nRefCount;
            if (0 >= m_nRefCount)
            {
                m_nRefCount = 0;
                ResourceMgr2.Singleton.Unload(m_strAbName);
            }
            //Debug.Log(string.Format("DelRef[{0}]:{1}", m_nRefCount, m_strAbName));
            return m_nRefCount;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (m_bDisposed)
            {
                return;
            }

            if (disposing)
            {
                //执行清理代码
                Unload();
            }
            m_bDisposed = true;
        }

        ~AssetBundleInfo()
        {
            this.Dispose(false);
        }


        private void Unload()
        {
            if (s_hashSet.Remove(m_strAbName) == false)
            {
                Debug.LogError("s_hashSet.Remove(m_strAbName) == false: " + m_strAbName);
            }

            Debug.Log("Unload:"+m_strAbName);

            if (null != m_assetBundle)
            {
                m_assetBundle.Unload(false);
                m_assetBundle = null;
            }
            m_bStarted = false;
            m_bSelfDone = false;

            //去除依赖
            if (null != m_DepAssets)
            {
                for (int nIndex = 0;nIndex<m_DepAssets.Length;++nIndex)
                {
                    AssetBundleInfo assetDepend = m_DepAssets[nIndex];
                    assetDepend.DelRef();
                }
                m_DepAssets = null;
            }

            m_hashSetRequest.Clear();
        }


        private Action<AssetBundle> m_callbackLoadCompleteFinish;//可以去除掉，直接用m_hashSetRequest；
        private LoadComplete m_callbackLoadCompleteFinishForDepend;
        private bool m_bCancel = false;
        private int m_nRefCount = 0;
        private bool m_bHasCallBacked = false;

        //protected AssetPRI mPRIType = AssetPRI.DownloadPRI_Plain;
        //protected bool mRemoveQuickly;
        private bool m_bDisposed = false;
        private AssetBundle m_assetBundle = null;
        private bool m_bStarted = false;
        private bool m_bSelfDone = false;

        private string m_strErrorInfo = string.Empty;
        private string m_strAbName = string.Empty;
        private uint m_unSize = 0;
        private Type m_type = null;

        private AssetBundleInfo[] m_DepAssets = null;

        private HashSet<AssetRequest> m_hashSetRequest = new HashSet<AssetRequest>();

        static HashSet<string> s_hashSet = new HashSet<string>();
    }
}
