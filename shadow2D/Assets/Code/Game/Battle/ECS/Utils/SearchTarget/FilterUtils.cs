
using System.Collections.Generic;
using UnityEngine;

namespace ECS
{
    internal class FilterUtils
    {
        public static List<Entity> ConvertEntityList(List<SearchInfo> sList,int num = -1) {
            List<Entity> eList = new List<Entity>();
            if (num == -1) num = sList.Count;
            for (int i = 0; i < sList.Count; ++i) {
                if (i < num)
                    eList.Add(sList[i].e);
                SearchInfo.Return(sList[i]);
            }
            return eList;
        }

        //对多个目标进行圆形筛选
        public static List<SearchInfo> filterRange(Vector3 pos, float range, List<Entity> entities, int exceptEId = -1, bool bIs3D = false){
            List<SearchInfo> result = new List<SearchInfo>();
            foreach (Entity entity in entities)
            {
                if (exceptEId == entity.Eid)
                    continue;
                if (LogicUtils.IsDead(entity))
                    continue;

                //两个entity的实际距离
                float tdistance = LogicUtils.GetSqrDistance(pos, LogicUtils.GetPos(entity), bIs3D);
                float bodysize = AttrUtil.GetBodySize(entity);
                float curRange = range + bodysize;
                float sqrDisRange = curRange * curRange;

                if (tdistance > sqrDisRange)
                    continue;

                SearchInfo temp = SearchInfo.Get();
                temp.e = entity;
                temp.distance = tdistance;

                result.Add(temp);
            }

            result.Sort((a, b) => a.distance.CompareTo(b.distance));

            return result;
        }

        //对多个目标进行矩形选取
        public static List<SearchInfo> filterRectangle(Vector3 pos,Vector3 forward,float length,float width, List<Entity> entities, int exceptEId = -1) {
            List<SearchInfo> result = new List<SearchInfo>();

            Vector3 uv = forward.normalized;

            float halfLength = length / 2;
            float halfWidth = width / 2;

            Vector3 centerPos = pos + uv * halfLength;

            foreach (Entity entity in entities){
                if (exceptEId == entity.Eid)
                    continue;

                Vector3 tpos = LogicUtils.GetPos(entity);
                float dx = tpos.x - centerPos.x;
                float dy = tpos.y - centerPos.y;

                float tLength = Mathf.Abs(dx * uv.x + dy * uv.y) ;
                float tWidth = Mathf.Abs(dx * uv.y + dy * -uv.x);

                if (tLength > halfLength || tWidth > halfWidth)
                    continue;

                SearchInfo temp = SearchInfo.Get();
                temp.e = entity;
                temp.distance = LogicUtils.GetSqrDistance(pos, tpos);
                result.Add(temp);
            }

            result.Sort((a, b) => a.distance.CompareTo(b.distance));

            return result;
        }


        //public static 
    }
}
