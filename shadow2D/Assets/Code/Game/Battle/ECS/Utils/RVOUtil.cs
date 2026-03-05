

using Pathfinding.RVO;
using UnityEngine;

namespace ECS
{
    internal class RVOUtil
    {
        //=============================================================================================
        public static void UpDateRVO() {
            RVOMgr.Singleton.Update();
            //Simulator.Instance.doStep();
        }

        public static int AddAgent(Vector3 pos,float bodySize,float agentTimeHorizon = 2,int layer = 0) {
            return RVOMgr.Singleton.CreateNewAgent(pos,bodySize,agentTimeHorizon,layer);
        }

        public static void SetAgentStop(Entity e) {
            SetAgentStop(e, LogicUtils.GetPos(e));
        }

        public static void SetAgentStop(Entity e,Vector2 target){
            RVOComponent comp = e.GetComponentData<RVOComponent>();
            if (comp == null || comp.AgentNo == -1)
                return;
            RVOMgr.Singleton.SetTarget(comp.AgentNo, target, 3);
        }

        public static void SetAgentTarget(Entity e,Vector2 targetPos) {
            RVOComponent comp = e.GetComponentData<RVOComponent>();
            if (comp == null || comp.AgentNo == -1)
                return;

            RVOMgr.Singleton.SetTarget(comp.AgentNo, targetPos, AttrUtil.GetSpeed(e));
        }

        public static void LockEntityRVO(Entity e,bool isLock) {
            RVOComponent comp = e.GetComponentData<RVOComponent>();
            if (comp == null || comp.AgentNo == -1)
                return;
            RVOMgr.Singleton.SetLock(comp.AgentNo, isLock);
        }

        //============================================================================================= 

        public static void OnEntityDead(Entity e) {
            RVOComponent comp = e.GetComponentData<RVOComponent>();
            if (comp == null)
                return;

            RVOMgr.Singleton.DelAgent(comp.AgentNo);

            e.RemoveComponent<RVOComponent>();
        }
    }
}

