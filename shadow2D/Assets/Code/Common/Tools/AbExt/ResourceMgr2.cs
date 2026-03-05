// #define ENABLE_ASSETBUNDLE

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor;
#endif

using UObject = UnityEngine.Object;


namespace ProjectX
{
    public class ResourceMgr2
    {
        public static ResourceMgr2 Singleton
        {
            get
            {
                if (null == s_singleton)
                {
                    s_singleton = new ResourceMgr2();
                }
                return s_singleton;
            }
        }

        public AssetBundleManifest AbManifest { get { return m_AssetBundleManifest; } }

        public Material MatGray { get { return m_matGray; } }


        //public CMd5FileList FileList { get { return m_md5FileList; } }

        public uint GetAssetSize(string abName)
        {
            uint unSize = 0;
            //CFileInfo fileInfo = m_md5FileList.GetFileInfo(abName);
            //if (null != fileInfo)
            //{
            //    unSize = fileInfo.m_nSize;
            //}
            return unSize;
        }


        // Load AssetBundleManifest.
        public AssetRequest Initialize(string manifestName, Action initOK)
        {
#if !UNITY_EDITOR || ENABLE_ASSETBUNDLE
            Debug.Log("ResourceMgr Initialize : " + manifestName);
            return LoadAsset<AssetBundleManifest>(manifestName, new string[]{"AssetBundleManifest"}, delegate(AssetRequest assetRequest) {
                Debug.Log("ResourceMgr Initialize OK : " + manifestName);
                if (assetRequest.objs.Length > 0)
                {
                    Debug.Log("ResourceMgr Initialize OK : " + manifestName + " objs:" + assetRequest.objs.Length);
                    m_AssetBundleManifest = assetRequest.objs[0] as AssetBundleManifest;
                    m_AllManifest = m_AssetBundleManifest.GetAllAssetBundles();
                }
                if (initOK != null) initOK();
            });
#else
            if (initOK != null) initOK();
            return null;
#endif
        }

        //public IEnumerator PreLoad()
        //{
        //    yield return LoadMaterial("UI_Gray", (AssetRequest assetRequest) => { m_matGray = assetRequest.objs[0] as Material; });
        //}

        //public AssetRequest LoadScene(string abName)
        //{
        //    return LoadAsset<Scene>(abName.ToLower(), new string[]{ }, null);
        //}

        //public GameObject Instantiate(string abName, string assetName)
        //{
        //    AssetRequest assetReq = LoadPrefab(abName, assetName, null);
        //    if (assetReq.objs == null || assetReq.objs.Length == 0)
        //    {
        //        Debug.LogError(string.Format("assetReq.objs == null || assetReq.objs.Length == 0: {0}/{1}", abName, assetName));
        //        return null;
        //    }
        //    GameObject prefab = (GameObject)assetReq.objs[0];
        //    if (prefab == null)
        //    {
        //        Debug.LogError($"prefab == null {abName} {assetName}");
        //        return null;
        //    }
        //    GameObject objResult = GameObject.Instantiate(prefab);
        //    return objResult;
        //}

        //public AssetRequest LoadPrefab(string abName, string assetName, Action<AssetRequest> func)
        //{
        //    return LoadAsset<GameObject>(abName.ToLower(), new string[]{assetName.ToLower()}, func);
        //}

        //public AssetRequest LoadPrefab(string abName, string[] assetNames, Action<AssetRequest> func)
        //{
        //    return LoadAsset<GameObject>(abName.ToLower(), assetNames, func);
        //}

        //public AssetRequest LoadSprite(string abName, string[] assetNames, Action<AssetRequest> func)
        //{
        //    return LoadAsset<Sprite>(abName.ToLower(), assetNames, func);
        //}

        //public AssetRequest LoadTexture(string abName, string assetName, Action<AssetRequest> func)
        //{
        //    return LoadAsset<Texture>(abName.ToLower(), new string[]{assetName.ToLower()}, func);
        //}

        //public AssetRequest LoadAudio(string abName, string[] assetNames, Action<AssetRequest> func)
        //{
        //    return LoadAsset<AudioClip>(abName.ToLower(), assetNames, func);
        //}

        //public AssetRequest LoadMaterial(string assetName, Action<AssetRequest> func)
        //{
        //    return LoadAsset<Material>("Assets/Res_AssetBundle/Material_AB", new string[]{assetName.ToLower()}, func);
        //}

        //public AssetRequest LoadShader(string assetName, Action<AssetRequest> func)
        //{
        //    return LoadAsset<Shader>("Assets/Res_AssetBundle/Shader_AB", new string[]{assetName.ToLower()}, func);
        //}

        //同步加载接口待完善
        public AssetRequest LoadAsset<T>(string abName, string[] assetNames, Action<AssetRequest> func)
        {
            AssetRequest assetRequest = new AssetRequest(abName, assetNames, func);

            assetRequest.assetType = typeof(T);
            AssetBundleInfo assetBundleInfo = null;

#if !UNITY_EDITOR || ENABLE_ASSETBUNDLE
            Debug.Log("LoadAsset Bundle: " + String.Join("|", assetRequest.assetNames) + "\nabName:" + assetRequest.abName);
            // abName = GetRealAssetPath(abName);
            try {
                assetBundleInfo = LoadAssetBundle(abName, typeof(T));
                assetBundleInfo.AddRequest(assetRequest);
                //if (assetBundleInfo.IsComplete() == true)
                //{
                //    assetBundleInfo.DoFinishCallBack();//如果已经加载好则即时回调
                //}
                assetRequest.LoadAssets(assetBundleInfo.AssetBundle);
                // LoadAssetInBundle<T>(assetRequest);
            } catch(Exception e) {
                Debug.LogError(e.StackTrace.ToString());
                Debug.LogError("LoadAsset Failed, LoadAsset(Bundle) : " + String.Join("|", assetRequest.assetNames) + "\nabName:" + assetRequest.abName + " e:" + e.ToString());
            }
#else
            Debug.Log("LoadAsset : " + String.Join("|", assetRequest.assetNames) + "\nabName:" + assetRequest.abName);
            LoadAssetInEditor<T>(assetRequest);
#endif

            return assetRequest;
        }

//        /// <summary>
//        /// 载入素材
//        /// </summary>
//        private AssetRequest LoadAssetAsync<T>(string abName, string[] assetNames, Action<AssetRequest> action = null) //where T : UObject
//        {
//            AssetRequest assetRequest = new AssetRequest(abName, assetNames, action);
//            assetRequest.assetType = typeof(T);
//            if (UnityGameEntry.DebugMode == true)
//            {
//#if UNITY_EDITOR
//                if (typeof(T) == typeof(Scene))
//                {
//                    assetRequest.OnAssetLoadFinished(new UObject[] { });
//                    return assetRequest;
//                }

//                UnityGameEntry.Instance.StartCoroutine(LoadAssetInEditorAync<T>(assetRequest));
//#endif
//            }
//            else
//            {
//                //abName = GetRealAssetPath(abName);
//                AssetBundleInfo assetBundleInfo = LoadAssetBundleAsync(abName, typeof(T));
//                assetBundleInfo.AddRequest(assetRequest);
//                if (assetBundleInfo.IsComplete(true) == true)
//                {
//                    assetBundleInfo.DoFinishCallBack();//如果已经加载好则即时回调
//                }
//                //else
//                //{
//                //    m_linkedListNeedToLoad.AddLast(assetBundleInfo);
//                //}
//            }
//            return assetRequest;
//        }

#if UNITY_EDITOR
        private void LoadAssetInEditor<T>(AssetRequest assetRequest)
        {
            if (null == assetRequest || string.IsNullOrEmpty(assetRequest.abName) || assetRequest.assetNames == null)
                return;

            string strExt = ".prefab";
            if (typeof(T) == typeof(Sprite) || typeof(T) == typeof(Texture))
            {
                strExt = ".png";
            }
            else if (typeof(T) == typeof(AudioClip))
            {
                strExt = ".mp3";
            }
            else if (typeof(T) == typeof(Material))
            {
                strExt = ".mat";
            }
            else if (typeof(T) == typeof(Shader))
            {
                strExt = ".shader";
            }
            else if (typeof(T) == typeof(Font))
            {
                strExt = ".ttf";
            }

            UObject[] result = new UObject[assetRequest.assetNames.Length];
            for (int i = 0; i < assetRequest.assetNames.Length; ++i)
            {
                string strPath = "";
                if (System.IO.Path.HasExtension(assetRequest.assetNames[i]))
                {
                    strPath = string.Format("{0}/{1}", assetRequest.abName, assetRequest.assetNames[i]);
                }
                else
                {
                    strPath = string.Format("{0}/{1}{2}", assetRequest.abName, assetRequest.assetNames[i], strExt);
                }

                UObject obj = AssetDatabase.LoadAssetAtPath(strPath, typeof(T));
                if (null == obj)
                {
                    strPath = string.Format("{0}/{1}{2}", assetRequest.abName + "_AB", assetRequest.assetNames[i], strExt);
                    obj = AssetDatabase.LoadAssetAtPath(strPath, typeof(T));
                    if (null == obj)
                    {
                        Debug.LogError("null == obj:" + strPath);
                    }
                }
                result[i] = obj;
            }
            assetRequest.OnAssetLoadFinished(result);
        }
#endif


#if UNITY_EDITOR
        IEnumerator LoadAssetInEditorAync<T>(AssetRequest assetRequest)
        {
            if (null == assetRequest || string.IsNullOrEmpty(assetRequest.abName) || assetRequest.assetNames == null)
                yield break;

            string strExt = ".prefab";
            if (typeof(T) == typeof(Sprite) || typeof(T) == typeof(Texture))
            {
                strExt = ".png";
            }
            else if (typeof(T) == typeof(AudioClip))
            {
                strExt = ".ogg";
            }
            else if (typeof(T) == typeof(Material))
            {
                strExt = ".mat";
            }
            else if (typeof(T) == typeof(Shader))
            {
                strExt = ".shader";
            }

            UObject[] result = new UObject[assetRequest.assetNames.Length];
            for (int i = 0; i < assetRequest.assetNames.Length; ++i)
            {
                string strPath = "";
                if (System.IO.Path.HasExtension(assetRequest.assetNames[i]))
                {
                    strPath = string.Format("Assets/Res_AssetBundle/{0}/{1}", assetRequest.abName, assetRequest.assetNames[i]);
                }
                else
                {
                    strPath = string.Format("Assets/Res_AssetBundle/{0}/{1}{2}", assetRequest.abName, assetRequest.assetNames[i], strExt);
                }

                UObject obj = AssetDatabase.LoadAssetAtPath(strPath, typeof(T));
                if (null == obj)
                {
                    Debug.LogError("null == obj:" + strPath);
                }
                result[i] = obj;
                yield return new WaitForEndOfFrame();
            }
            yield return null;

            assetRequest.OnAssetLoadFinished(result);
        }
#endif


        private void LoadAssetInResource<T>(AssetRequest assetRequest)
        {
            if (null == assetRequest || string.IsNullOrEmpty(assetRequest.abName) || assetRequest.assetNames == null)
                return;

            UObject[] result = new UObject[assetRequest.assetNames.Length];
            for (int i = 0; i < assetRequest.assetNames.Length; ++i)
            {
                string strPath = string.Format("{0}/{1}", assetRequest.abName, assetRequest.assetNames[i]);
                ;

                UObject obj = Resources.Load(strPath, typeof(T));
                if (null == obj)
                {
                    Debug.LogError("null == obj:" + strPath);
                }
                result[i] = obj;
            }
            assetRequest.OnAssetLoadFinished(result);
        }

        private void LoadAssetInBundle<T>(AssetRequest assetRequest)
        {
            if (null == assetRequest || string.IsNullOrEmpty(assetRequest.abName) || assetRequest.assetNames == null)
                return;

            UObject[] result = new UObject[assetRequest.assetNames.Length];
            for (int i = 0; i < assetRequest.assetNames.Length; ++i)
            {
                string strPath = string.Format("{0}/{1}", assetRequest.abName, assetRequest.assetNames[i]);
                ;

                UObject obj = Resources.Load(strPath, typeof(T));
                if (null == obj)
                {
                    Debug.LogError("null == obj:" + strPath);
                }
                result[i] = obj;
            }
            assetRequest.OnAssetLoadFinished(result);
        }

        public void Update()
        {
            //Unload First
            if (m_bNeedToUnload == true)
            {
                m_bNeedToUnload = false;
                UnLoadNotUsedResource();
            }

            //////////////////////////////////////
            LinkedListNode<AssetBundleInfo> nodeAssetResource = m_linkedListAssetResourceInLoading.First;
            while (nodeAssetResource != null)
            {
                LinkedListNode<AssetBundleInfo> nodeNext = nodeAssetResource.Next;
                //如果尚未调用回调，并且下载完成，则调用
                AssetBundleInfo wwwRes = nodeAssetResource.Value;
                if (wwwRes.IsSelfDone == true)
                {
                    m_linkedListAssetResourceInLoading.Remove(nodeAssetResource);
                    wwwRes.OnSelfDownLoadFinished();
                }
                //else if (wwwRes.Canceled == true)
                //{
                //    m_linkedListAssetResourceInLoading.Remove(nodeAssetResource);
                //}
                else if (wwwRes.IsStart == false)
                {
                    wwwRes.LoadAsync();
                }
                nodeAssetResource = nodeNext;
            }

            //AssetLogger.Debug(string.Format("m_linkedListNeedToLoad.Count={0}, m_queueAssetResourceInLoading.Count={1}", m_linkedListNeedToLoad.Count, m_queueAssetResourceInLoading.Count));
            //加入到正式加载的多线程队列
            if (m_linkedListNeedToLoad.Count > 0 && m_linkedListAssetResourceInLoading.Count < 5)
            {
                AssetBundleInfo assetRes = m_linkedListNeedToLoad.First.Value;
                m_linkedListNeedToLoad.RemoveFirst();
                m_linkedListAssetResourceInLoading.AddLast(assetRes);
            }
        }

        public void DelAssetRequest(AssetRequest request)
        {
            AssetBundleInfo assetBundleInfo = null;
            if (m_dicAssetResourcesInfo.TryGetValue(request.abName, out assetBundleInfo) == true)
            {
                assetBundleInfo.DelRequest(request);
            }
        }

        public void Unload(string strAbName)
        {
            m_listNeedToDelAsset.Add(strAbName);
            m_bNeedToUnload = true;
        }

        public void UnInit()
        {
            foreach (AssetBundleInfo assetBundleInfo in m_dicAssetResourcesInfo.Values)
            {
                assetBundleInfo.Dispose();
            }
            m_dicAssetResourcesInfo.Clear();
            m_Dependencies.Clear();
            m_linkedListAssetResourceInLoading.Clear();
            m_linkedListNeedToLoad.Clear();
            m_AssetBundleManifest = null;
            m_AllManifest = null;
            m_listNeedToDelAsset.Clear();
            m_matGray = null;
        }

        public void Print()
        {
            int nCount = 0;
            foreach (AssetBundleInfo ab in m_dicAssetResourcesInfo.Values)
            {
                string strLine = string.Format("{0}AssetBundle:{1}#{2}", ++nCount, ab.RefCount, ab.AbName);
                Debug.Log(strLine);
            }
        }

        private AssetBundleInfo LoadAssetBundle(string abName, Type type) //Load
        {
            abName = abName.ToLower();
            abName = abName.Replace("_ab", "");
            AssetBundleInfo assetBundleInfo = null;
            if (m_dicAssetResourcesInfo.TryGetValue(abName, out assetBundleInfo) == true)
            {
                return assetBundleInfo;
            }

            uint unSize = GetAssetSize(abName);
            assetBundleInfo = new AssetBundleInfo(abName, unSize, type);
            m_dicAssetResourcesInfo[abName] = assetBundleInfo;
            assetBundleInfo.Load();
            
            Debug.Log("LoadAssetBundle : " + abName + " type:" + type.ToString() + " unSize:" + unSize);
            
            if (type != typeof(AssetBundleManifest))
            {
                if (m_AssetBundleManifest)
                {
                    string[] dependencies = m_AssetBundleManifest.GetDirectDependencies(abName);

                    assetBundleInfo.SetDepSize(dependencies.Length);
                    for (int nIndex = 0; nIndex < dependencies.Length; ++nIndex)
                    {
                        string dependAbName = dependencies[nIndex];
                        AssetBundleInfo assetBundleInfoDep = LoadAssetBundle(dependAbName, type);
                        assetBundleInfo.AddDep(assetBundleInfoDep, nIndex);
                    }
                }
            }
            return assetBundleInfo;
        }

        private AssetBundleInfo LoadAssetBundleAsync(string abName, Type type) //Load
        {
            AssetBundleInfo assetBundleInfo = null;
            if (m_dicAssetResourcesInfo.TryGetValue(abName, out assetBundleInfo) == true)
            {
                return assetBundleInfo;
            }

            uint unSize = GetAssetSize(abName);
            assetBundleInfo = new AssetBundleInfo(abName, unSize, type);
            m_dicAssetResourcesInfo[abName] = assetBundleInfo;
            m_linkedListNeedToLoad.AddLast(assetBundleInfo);

            if (type != typeof(AssetBundleManifest))
            {
                string[] dependencies = m_AssetBundleManifest.GetAllDependencies(abName);

                assetBundleInfo.SetDepSize(dependencies.Length);
                for (int nIndex = 0; nIndex < dependencies.Length; ++nIndex)
                {
                    string dependAbName = dependencies[nIndex];
                    AssetBundleInfo assetBundleInfoDep = LoadAssetBundleAsync(dependAbName, type);
                    assetBundleInfo.AddDep(assetBundleInfoDep, nIndex);
                }
            }
            return assetBundleInfo;
        }


        private void UnLoadNotUsedResource()
        {
            //unload only one asset in once time
            //--------------------------------m_dicAllAssetRes--------------------------------------//
            for (int nIndex = 0; nIndex < m_listNeedToDelAsset.Count; ++nIndex)
            {
                string strAbName = m_listNeedToDelAsset[nIndex];
                AssetBundleInfo assetBundleInfo = null;
                if (m_dicAssetResourcesInfo.TryGetValue(strAbName, out assetBundleInfo) == true)
                {
                    if (assetBundleInfo.RefCount <= 0)
                    {
                        assetBundleInfo.Dispose();
                        m_dicAssetResourcesInfo.Remove(strAbName);
                    }
                }
            }
            m_listNeedToDelAsset.Clear();
        }


        private string GetRealAssetPath(string abName)
        {
            if (abName.Equals("StreamingAssets"))
            {
                return abName;
            }
            abName = abName.ToLower();
            //if (!abName.EndsWith(AppConst.ExtName)) {
            //    abName += AppConst.ExtName;
            //}
            if (abName.Contains("/"))
            {
                return abName;
            }
            //string[] paths = m_AssetBundleManifest.GetAllAssetBundles();  产生GC，需要缓存结果
            for (int i = 0; i < m_AllManifest.Length; i++)
            {
                int index = m_AllManifest[i].LastIndexOf('/');
                string path = m_AllManifest[i].Remove(0, index + 1); //字符串操作函数都会产生GC
                if (path.Equals(abName))
                {
                    return m_AllManifest[i];
                }
            }
            Debug.LogError("GetRealAssetPath Error:>>" + abName);
            return null;
        }


        //private CMd5FileList m_md5FileList = new CMd5FileList();

        string[] m_AllManifest = null;
        AssetBundleManifest m_AssetBundleManifest = null;
        private Material m_matGray = null;
        private Shader m_shaderGray = null;
        Dictionary<string, string[]> m_Dependencies = new Dictionary<string, string[]>();
        Dictionary<string, AssetBundleInfo> m_dicAssetResourcesInfo = new Dictionary<string, AssetBundleInfo>();


        private LinkedList<AssetBundleInfo> m_linkedListNeedToLoad = new LinkedList<AssetBundleInfo>();
        private LinkedList<AssetBundleInfo> m_linkedListAssetResourceInLoading = new LinkedList<AssetBundleInfo>();
        private List<string> m_listNeedToDelAsset = new List<string>();

        private float m_fTimeLastClearUp = 0;
        private bool m_bNeedToUnload = false;
        private static ResourceMgr2 s_singleton = null;
    }
}
