
using System.Collections.Generic;

namespace ECS
{
    class StatusSystem : System
    {
        public StatusSystem(ECSWorld ecsWorld)
        {
            Init(ecsWorld);

            RequireComponent(typeof(StatusComponent));
        }

        public override void Update(){
            float now = EcsWorld.Time;
            foreach (var e in entities){
                onUpdateEntity(e, now);
            }
        }

        static List<int> removeList = new List<int>();
        public void onUpdateEntity(Entity le, float now) {
            var statusComp = le.GetComponentData<StatusComponent>();
            if (statusComp == null)
                return;

            removeList.Clear();
            foreach (var it in statusComp.m_dictStatus) {
                StatusInfo info = it.Value;
                if (info.SelfBreak)
                {
                    removeList.Add(info.StatusId);
                    continue;
                }

                if (info.EndTime < now)
                {
                    removeList.Add(info.StatusId);
                    continue;
                }

                if (info.LastTickTime + info.UnitTickTime > now)
                    continue;

                info.LastTickTime += info.UnitTickTime;
                info.script.OnStatusStepUpdate(le,info);
            }

            foreach (var it in removeList)
                StatusUtil.RemoveStatus(le, it);
        }
    }
}