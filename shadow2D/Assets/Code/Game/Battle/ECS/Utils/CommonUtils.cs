
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Profiling;

namespace ECS
{
    partial class LogicUtils
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BeginSample(string str){
            Profiler.BeginSample(str);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void EndSample(){
            Profiler.EndSample();
        }

        //计算两个单位的平方距离
        public static float GetSqrDistance(Entity e1, Entity e2, bool is2d = true)
        {
            var pos1 = GetPos(e1);
            var pos2 = GetPos(e2);
            return GetSqrDistance(pos1, pos2, is2d);
        }

        //计算两个坐标的平方距离
        public static float GetSqrDistance(Vector3 pos1, Vector3 pos2, bool is2D = true)
        {
            var del = pos1 - pos2;
            if (is2D) del.z = 0;
            var ret = del.sqrMagnitude;
            return ret;
        }

        public static float GetRand(float min, float max)
        {
            return Random.Range(min, max);
        }

        public static int GetRand(int min, int max)
        {
            return Random.Range(min, max);
        }

        public static List<Vector2> GetBezierPath20(params Vector2[] args) { return GetBezierPath(20, args); }

        //获取贝塞尔曲线路径点[count 个点]
        public static List<Vector2> GetBezierPath(int count, params Vector2[] args)
        {
            float fStep = 1f / count;
            List<Vector2> list = new List<Vector2>();
            int loop = 0;
            for (float t = 0; t <= 1.0001f && loop < 1000; loop++, t += fStep){
                list.Add(GetBezierPosition(t, args));
            }
            return list;
        }

        //获取一个贝塞尔曲线上的点
        public static Vector2 GetBezierPosition(float t, params Vector2[] args)
        {
            if (args.Length < 1)
                return Vector2.zero;

            if (args.Length < 2)
                return args[0];

            List<Vector2> lis = new List<Vector2>();
            for (var i = 1; i < args.Length; ++i)
                lis.Add((1 - t) * args[i - 1] + t * args[i]);
            
            return GetBezierPosition(t, lis.ToArray());
        }
        public static void StartCoroutine(IEnumerator routine)
        {
            UnityGameEntry.Instance.StartCoroutine(routine);
        }

        //=======================================================================================
        public static SelectObjectComponent GetSelectObjectComponent(ECSWorld logicEcsWorld)
        {
            if (logicEcsWorld == null || logicEcsWorld.GlobalEntity == null)
                return null;

            return logicEcsWorld.GlobalEntity.GetComponentData<SelectObjectComponent>();
        }
        public static void SetSelectEntity(List<Entity> selectList) {

            SelectObjectComponent component = GetSelectObjectComponent(UIUtils.GetECSWorld());
            if (component == null)
                return;

            component.SelectList = selectList;
            if (selectList.Count > 0)
                component.ShowEntity = selectList[0];
            else
                component.ShowEntity = null;
        }

        public static void OnControlMove(Vector3 pos) {
            SelectObjectComponent component = GetSelectObjectComponent(UIUtils.GetECSWorld());
            if (component == null || component.SelectList.Count == 0)
                return;
             
            int lenth = (int)Mathf.Sqrt(component.SelectList.Count);
            if (lenth * lenth < component.SelectList.Count)
                lenth += 1; 

            float step = 2f;

            float offSet = lenth / 2.0f * step - step/2;

            for (int index = 0; index < component.SelectList.Count; ++index) {
                int x = index % lenth;
                int y = index / lenth;
                Vector3 newPos = new Vector3(x * step - offSet,y * step - offSet,0) + pos;
                LogicDataUtils.SetFindPathToPosMove(component.SelectList[index],newPos);

                AIDataUtils.SetAIState(component.SelectList[index], AIState.AI_MoveControl);
            }
        } 

    }
}
