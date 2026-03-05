

using System.Collections.Generic;

namespace ECS
{
    class DeadSystem : System
    {
        public DeadSystem(ECSWorld world)
        {
            Init(world);
            RequireComponent(typeof(LifeTimeComponent));
        }

        public override void Update() {
            UpdateLifeTimeKill();
            UpdateDelayKill();
        }

        public void UpdateLifeTimeKill() {
            float Now = EcsWorld.Time;
            foreach (var e in entities) {
                LifeTimeComponent comp = e.GetComponentData<LifeTimeComponent>();
                if (Now < comp.DeadTime)
                    continue;
                LogicUtils.TryDelayKill(e,0.1f);
            }
        }

        public void UpdateDelayKill() {
            DelayDeadComponent delayDeadComponent = EcsWorld.GlobalEntity.GetComponentData<DelayDeadComponent>();
            if (delayDeadComponent == null)
                return;

            float now = EcsWorld.Time;
            List<int> removeList = new List<int>();
            foreach (var it in delayDeadComponent.delayDeadTimes){
                if (it.Value > now)
                    continue;
                removeList.Add(it.Key);
                Entity e = LogicUtils.GetEntity(EcsWorld, it.Key);
                if (e == null)
                    continue;
                EcsWorld.KillEntity(e);
            }

            foreach (int eid in removeList)
                delayDeadComponent.delayDeadTimes.Remove(eid);
        }
      
    }
}
