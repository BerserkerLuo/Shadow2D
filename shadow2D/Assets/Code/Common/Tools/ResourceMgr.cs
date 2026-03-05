using System;
using System.Collections;
using UnityEngine;

namespace Tool
{
    public class ResourceMgr : SingletonBase<ResourceMgr>
    {
        public void Load<T>(string path,Action<T> callBack) where T : UnityEngine.Object{
            Debug.Log("Load " + path);

            CoroutineRunner.Instance.StartCoroutine(LoadAsync<T>(path,callBack));
        }

        IEnumerator LoadAsync<T>(string path, Action<T> callBack) where T : UnityEngine.Object{

            Debug.Log("LoadAsync "+path);
            if (path == "")
                callBack?.Invoke(null);

            ResourceRequest request = Resources.LoadAsync<T>(path);
            // 等待异步加载完成
            yield return request;

            if (request.asset == null)
                Debug.LogError("Load Resource Error ! Path :[" + path + "]");

            callBack?.Invoke(request.asset as T);
        }
    }
}