using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UObject = UnityEngine.Object;

namespace ProjectX
{
    public class AssetRequest : CustomYieldInstruction, IDisposable
    {
        public override bool keepWaiting { get { return !m_bFinished; } }

        public Type assetType;

        public string[] assetNames;

        public string abName;

        public UObject[] objs;

        public bool IsError
        {
            get { return m_bIsError; }
        }

        public bool RemoveQuickly
        {
            get { return m_bRemoveQuickly; }
            set
            {
                m_bRemoveQuickly = value;
            }
        }

        public AssetRequest(string abName, string[] assetNames, Action<AssetRequest> callBack)
        {
            this.abName = abName;
            this.assetNames = assetNames;
            m_eventHandlerRequestFinished = callBack;
        }

        public void OnAssetLoadFinished(UObject[] objs)
        {
            this.objs = objs;
            m_bFinished = true;
            if (null != m_eventHandlerRequestFinished)
            {
                m_eventHandlerRequestFinished(this);
            }
        }

        public void OnAssetBundleLoadFinished(AssetBundle assetBundle)
        {
            CoroutineRunner.Instance.StartCoroutine(LoadAssetsAsync(assetBundle));
        }

        public void LoadAssets(AssetBundle assetBundle)
        {
            List<UObject> result = new List<UObject>();
            if (null != assetBundle && assetNames != null)
            {
                for (int j = 0; j < assetNames.Length; j++)
                {
                    string assetPath = Path.GetFileName(assetNames[j]);
                    UObject uObject = assetBundle.LoadAsset(assetPath, assetType);
                    result.Add(uObject);
                }
            }
            else
            {
                m_bIsError = true;
            }
            OnAssetLoadFinished(result.ToArray());
        }

        private IEnumerator LoadAssetsAsync(AssetBundle assetBundle)
        {
            List<UObject> result = new List<UObject>();
            if (null != assetBundle && assetNames != null)
            {
                for (int j = 0; j < assetNames.Length; j++)
                {
                    string assetPath = Path.GetFileName(assetNames[j]);
                    AssetBundleRequest request = assetBundle.LoadAssetAsync(assetPath, assetType);
                    yield return request;
                    result.Add(request.asset);
                }
            }

            OnAssetLoadFinished(result.ToArray());
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
                ResourceMgr2.Singleton.DelAssetRequest(this);
            }

            m_bDisposed = true;
        }

        ~AssetRequest()
        {
            this.Dispose(false);
        }

        private bool m_bRemoveQuickly = false;
        private bool m_bFinished = false;
        private bool m_bIsError = false;
        private bool m_bDisposed = false;
        private Action<AssetRequest> m_eventHandlerRequestFinished = null;
    }
}

