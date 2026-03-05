
using PlayerSystemData;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ECS
{
    internal class UnityCollideUtils
    {
        //==========================================================================================================
        //生成唯一碰撞Key
        public static long OnMakeUniqueCollideKey(int v1, int v2)
        {
            if (v1 > v2)
                return OnMakeCollideKey(v1, v2);
            else
                return OnMakeCollideKey(v2, v1);
        }

        //生成碰撞Key
        public static long OnMakeCollideKey(int v1, int v2)
        {
            return v2 + (long)v1 * 100000;
        }

        //==========================================================================================================

        public static ObjectCollideScript TryAddCollideScript(GameObject obj)
        {
            ObjectCollideScript script = obj.GetComponent<ObjectCollideScript>();
            if (script == null)
                script = obj.AddComponent<ObjectCollideScript>();
            return script;

        }

        public static void InitCollideScript(GameObject obj, Entity e)
        {
            ObjectCollideScript script = TryAddCollideScript(obj);
            script.Init(e);
        }

        //==========================================================================================================
        //获取碰撞列表
        public static bool GetCollideInfoList(Entity e, Dictionary<long, CollideInfo> OutMap)
        {
            AvatarComponent avatarComp = e.GetComponentData<AvatarComponent>();
            if (avatarComp == null)
                return false;

            foreach (var it in avatarComp.EcsObjs)
            {
                GetGameobjCollideInfos(it.Value, OutMap);
            }

            return true;
        }

        public static void GetGameobjCollideInfos(ECSBaseObject obj, Dictionary<long, CollideInfo> retCollideInfoList)
        {
            //PersonalUtils.BeginSample("GetGameobjCollideInfos GetComponent");
            ObjectCollideScript script = obj.GetCollideScript();
            //PersonalUtils.EndSample();
            if (script == null)
                return;

            foreach (var it in script.DictCollideInfos)
            {
                if (retCollideInfoList.ContainsKey(it.Key))
                    continue;

                retCollideInfoList.Add(it.Key, it.Value);
            }
        }

        //清理碰撞缓存
        public static void ClearCollideInfoCache(Entity e)
        {
            AvatarComponent avatarComp = e.GetComponentData<AvatarComponent>();
            if (avatarComp == null)
                return;

            Dictionary<string, ECSBaseObject> dictEcsObjs = avatarComp.EcsObjs;
            foreach (var it in dictEcsObjs)
                ClearGameobjCollideInfos(it.Value);
        }

        public static void ClearGameobjCollideInfos(ECSBaseObject obj)
        {
            ObjectCollideScript script = obj.GetCollideScript();
            if (script == null)
                return;

            script.ClearCollideCache();
        }

    }

}
