
using System.Collections.Generic;
using UnityEngine;

namespace ECS
{

    public static class ECSObjectPoolMgr
    {
        static Dictionary<string, IECSObjectPool> dictPools = new Dictionary<string, IECSObjectPool>();
        
        static HashSet<string> setPathInvalidIds = new HashSet<string>();

        static int ExpansionType = 1; //1固定扩容 2百分比扩容
        static int Expansionparam = 10;

        static Transform ObjPoolParent = null;

        static ECSObjectPoolMgr() {
            ObjPoolParent = new GameObject("ECSObjectPoolMgr").transform;
            GameObject.DontDestroyOnLoad(ObjPoolParent);

            dictPools.Clear();

            DebugUtils.DebugLog("ECSObjectPoolMgr dictPools.Count {}", dictPools.Count);
        }

        public static void SetExpansion(int type, int param)
        {
            ExpansionType = type;
            Expansionparam = param;

            foreach (var it in dictPools) 
                it.Value.SetExpansion(type,param);
        } 

        public static IECSObjectPool GetObjectPoolByPath<T>(string path) where T : ECSBaseObject, new() {
            //检查此路径是否已经无效
            if (setPathInvalidIds.Contains(path))
                return null;

            IECSObjectPool retPool = null;

            //尝试获取一下 如果已经存在就直接返回
            dictPools.TryGetValue(path, out retPool);
            if (retPool != null)
                return retPool;

            //没有就添加对象池 
            AddNewObjectPool<T>(path);

            //在尝试获取一下 如果添加成功就直接返回
            dictPools.TryGetValue(path, out retPool);
            if (retPool != null) 
                return retPool;

            DebugUtils.DebugLog("GetObjectPoolByPath Faild ! {}", path);

            //添加失败 加入失败列表 下次直接拒绝
            setPathInvalidIds.Add(path);

            return null;
        }

        public static void AddNewObjectPool<T>(string path) where T : ECSBaseObject, new()
        {
            GameObject prefab = Resources.Load<GameObject>(path);
            if (prefab == null) 
                return;

            DebugUtils.DebugLog("AddNewObjectPool {}", path);

            ECSObjectPool<T> pool = new ECSObjectPool<T>( prefab);
            dictPools.Add(path, pool);

            pool.SetExpansion(ExpansionType,Expansionparam);

            pool.SetPoolParent(ObjPoolParent);
        }

        public static void Clear() {
            foreach (var it in dictPools)
                it.Value.ReturnAll();
        }

        public static void ShowPoolInfo() {
            string str = "\nECSObjectPoolInfo : \n";
            foreach (var it in dictPools) 
                str += it.Key + it.Value.BriefInfo() + "\n";

            Debug.Log(str);
        }
    }
}
