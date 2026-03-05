using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ECS
{
    internal class DebugMgr
    {
        public static void OnEndtiyBeKill(Entity e) {
            OnRemoveTestTarget(e);
        }

        //===================================================================================
        //画目标点

        private static Dictionary<int, ECSGameObject> objs = new Dictionary<int, ECSGameObject>();

        public static ECSGameObject GetGameObject(int eId) {
            if (!objs.ContainsKey(eId))
            {
                ECSGameObject temp = ECSGameObject.Get("prefabs/Test/TestBlock");
                temp.OnActive();
                objs.Add(eId, temp);
                return temp;
            }
            return objs.GetValueOrDefault(eId,null);
        }

        public static void UpdateTargetPos(Entity e,Vector3 pos) {UpdateTargetPos(e.Eid, pos);}
        public static void UpdateTargetPos(int id, Vector3 pos){
            ECSGameObject obj = GetGameObject(id);
            obj.transform.position = pos;
        }

        public static void OnRemoveTestTarget(Entity e) {OnRemoveTestTarget(e.Eid); }
        public static void OnRemoveTestTarget(int id)
        {
            if (!objs.ContainsKey(id))
                return;
            ECSGameObject obj = objs.GetValueOrDefault(id, null);
            obj.Destory();
            objs.Remove(id);
        }

        //===================================================================================
        //画路径
        private static Dictionary<int, GameObject> PathObjs = new Dictionary<int, GameObject>();
        private static GameObject GetPathObj(int eId) {
            GameObject obj = PathObjs.GetValueOrDefault(eId,null);
            if (obj == null) {
                obj = new GameObject();
                obj.AddComponent<LineRenderer>();
                PathObjs.Add(eId,obj);
            }
            return obj;
        }
        public static void DrawPath(Entity e,List<Vector3> path) {
            GameObject obj = GetPathObj(e.Eid);
            LineRenderer lineRenderer = obj.GetComponent<LineRenderer>();
            lineRenderer.positionCount = path.Count+1;
            lineRenderer.SetPosition(0,LogicUtils.GetPos(e));
            for (int i = 0; i < path.Count; ++i)
                lineRenderer.SetPosition(i+1, path[i]);
        }

    }
}
