


using Table;
using UnityEngine;
using System.Collections.Generic;

namespace ECS
{
    internal class ActionIdleRandMove : ATNodeLeaf
    {
        public int RunCount = 0;
        public Dictionary<int, int> Index2Eid = new();
        public Dictionary<int, int> Eid2Index = new();

        public ActionIdleRandMove() {
        }
        public override bool Run(Entity e)
        {
            var comp = e.GetComponentData<AIComponent>();
            if (comp == null)
                return false;

            RunCount++;
            if (RunCount == 100){
                ClearDead(e.EcsWorld);
                RunCount = 0;
            }
            Vector2 pos = GetRandPos(e);
            LogicDataUtils.SetFindPathToPosMove(e, pos, 0);

            int index = MakeIndex(pos);
            int oldIndex = Eid2Index.GetValueOrDefault(e.Eid,0);
            if (!Eid2Index.TryAdd(e.Eid, index)) Eid2Index[e.Eid] = index;
            Index2Eid.Remove(oldIndex);
            if (!Index2Eid.TryAdd(index, e.Eid)) Index2Eid[index] = e.Eid;

            Debug.Log("ActionIdleRandMove pos " + pos);

            return true;
        }

        public Vector2 GetRandPos(Entity e) {
            Vector2 retPos = GetRandPos(e.EcsWorld);
            for (int i = 0; i < 10; ++i) {
                int index = MakeIndex(retPos);
                int eid = Index2Eid.GetValueOrDefault(index,-1);
                if (LogicUtils.IsDead(e.EcsWorld, eid))
                    break;
                retPos = GetRandPos(e.EcsWorld);
            }
            return retPos;
        }

        public Vector2 GetRandPos(ECSWorld world) {
            Vector2 pos = MapUtil.GetRoleRandBronPos(world);
            return new Vector2((int)pos.x, (int)pos.y);
        }

        public int MakeIndex(Vector2 pos) {
            return (int)(pos.y * 5000 + pos.x);
        }
        public void ClearDead(ECSWorld world) {
            List<int> delList = new List<int>();
            foreach (var it in Index2Eid)
                if (LogicUtils.IsDead(world, it.Value))
                    delList.Add(it.Key);

            foreach (int key in delList)
                Index2Eid.Remove(key);
        }
    
    }
}
