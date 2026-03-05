using System.Collections.Generic;

using UnityEngine;

namespace ECS
{
    class SuspendSystem : System
    {
        public SuspendSystem(ECSWorld ecsWorld)
        {
            Init(ecsWorld);
        }

        private static List<int> removeList = new List<int>();
        public override void Update()
        {
            SuspendComponent SuspendComp = LogicUtils.GetSuspendComponent(EcsWorld);
            if (SuspendComp == null)
                return;

            float now = Time.time;

            foreach (var nt in SuspendComp.AddDic)
            {
                SuspendComp.DictSuspends.Add(nt.Key, nt.Value);
            }

            SuspendComp.AddDic.Clear();

            foreach (var it in SuspendComp.DictSuspends)
            {
                SuspendData s = it.Value;
                if (now < s.nextTickTime)
                    continue;

                if (s.intervalTime <= 0)
                {
                    for (int i = 0; i < s.residueCount; ++i)
                        s.callBackFun(EcsWorld, s);
                    s.residueCount = 0;
                }
                else
                    s.callBackFun(EcsWorld, s);

                if (--s.residueCount < 1)
                    removeList.Add(it.Key);
                else
                    s.nextTickTime += s.intervalTime;
            }

            if (removeList.Count < 1)
                return;

            foreach (var it in removeList)
                SuspendComp.DictSuspends.Remove(it);

            removeList.Clear();
        }

    }
}